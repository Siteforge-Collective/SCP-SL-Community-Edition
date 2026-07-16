namespace PlayerRoles.FirstPersonControl
{
	public class FirstPersonMovementModule : global::UnityEngine.MonoBehaviour, global::GameObjectPools.IPoolSpawnable, global::GameObjectPools.IPoolResettable, global::CursorManagement.ICursorOverride
	{
		public global::System.Action OnServerPositionOverwritten;

		public global::System.Action OnGrounded;

		public global::UnityEngine.GameObject CharacterModelTemplate;

		public float CrouchSpeed;

		public float SneakSpeed;

		public float WalkSpeed;

		public float SprintSpeed;

		public float JumpSpeed;

		public global::PlayerRoles.FirstPersonControl.CharacterControllerSettingsPreset CharacterControllerSettings;

		public float CrouchHeightRatio;

		private global::UnityEngine.Transform _transform;

		private global::PlayerRoles.FirstPersonControl.PlayerMovementState _speedState;

		private bool _syncGrounded;

		private global::UnityEngine.Vector3 _cachedPosition;

		private static global::System.Action _activeUpdates;

		public virtual global::CursorManagement.CursorOverrideMode CursorOverride => global::CursorManagement.CursorOverrideMode.Centered;

		public virtual bool LockMovement => false;

		public global::UnityEngine.CharacterController CharController { get; private set; }

		public bool ModuleReady { get; private set; }

		public global::PlayerRoles.FirstPersonControl.FpcMotor Motor { get; protected set; }

		public global::PlayerRoles.FirstPersonControl.FpcNoclip Noclip { get; protected set; }

		public global::PlayerRoles.FirstPersonControl.FpcMouseLook MouseLook { get; protected set; }

		public global::PlayerRoles.FirstPersonControl.FpcStateProcessor StateProcessor { get; protected set; }

		public MovementTracer Tracer { get; private set; }

		public global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel CharacterModelInstance { get; private set; }

		public global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData LastSentData { get; internal set; }

		public float MaxMovementSpeed => VelocityForState(ValidateMovementState(_speedState), applyCrouch: true);

		public global::PlayerRoles.FirstPersonControl.PlayerMovementState CurrentMovementState
		{
			get
			{
				return ValidateMovementState(SyncMovementState);
			}
			set
			{
				SyncMovementState = value;
			}
		}

		public global::PlayerRoles.FirstPersonControl.PlayerMovementState SyncMovementState { get; private set; }

		public bool IsGrounded
		{
			get
			{
				if ((global::Mirror.NetworkServer.active || Hub.isLocalPlayer) ? CharController.isGrounded : _syncGrounded)
				{
					return !Noclip.IsActive;
				}
				return false;
			}
			set
			{
				if (_syncGrounded != value)
				{
					_syncGrounded = value;
					if (value)
					{
						OnGrounded?.Invoke();
					}
				}
			}
		}

		public global::UnityEngine.Vector3 Position
		{
			get
			{
				return _cachedPosition;
			}
			set
			{
				_transform.position = value;
				_cachedPosition = value;
			}
		}

		protected ReferenceHub Hub { get; private set; }

		protected global::PlayerRoles.PlayerRoleBase Role { get; private set; }

		public static event global::System.Action OnPositionUpdated;

		protected virtual void UpdateMovement()
		{
			SyncMovementState = StateProcessor.UpdateMovementState(CurrentMovementState);
			Motor.UpdatePosition(out var sendJump);
			Noclip.UpdateNoclip();
			MouseLook.UpdateRotation();
			if (SyncMovementState != global::PlayerRoles.FirstPersonControl.PlayerMovementState.Crouching)
			{
				_speedState = SyncMovementState;
			}
			if (Hub.isLocalPlayer)
			{
				float walkSpeed = VelocityForState(global::PlayerRoles.FirstPersonControl.PlayerMovementState.Walking, applyCrouch: false);
				StateProcessor.ClientUpdateInput(this, walkSpeed, out var valueToSend);
				Motor.ReceivedPosition = new global::RelativePositioning.RelativePosition(Position);
				global::Mirror.NetworkClient.Send(new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFromClientMessage(Motor.ReceivedPosition, valueToSend, sendJump, MouseLook));
			}
		}

		private void FixedUpdate()
		{
			if (global::Mirror.NetworkServer.active)
			{
				Tracer.Record(Position);
			}
		}

		private void OnRoleDisabled(global::PlayerRoles.RoleTypeId rid)
		{
			if (!global::GameObjectPools.PoolManager.Singleton.TryReturnPoolObject(CharacterModelInstance.gameObject))
			{
				global::UnityEngine.Debug.LogError("FPC model could not be returned to the pool.");
			}
			Noclip.ShutdownModule();
		}

		protected virtual global::PlayerRoles.FirstPersonControl.PlayerMovementState ValidateMovementState(global::PlayerRoles.FirstPersonControl.PlayerMovementState state)
		{
			switch ((byte)state)
			{
			case 0:
				if (CrouchSpeed != 0f)
				{
					break;
				}
				goto IL_0047;
			case 1:
				if (SneakSpeed != 0f)
				{
					break;
				}
				goto IL_0047;
			case 3:
				{
					if (SprintSpeed != 0f)
					{
						break;
					}
					goto IL_0047;
				}
				IL_0047:
				return global::PlayerRoles.FirstPersonControl.PlayerMovementState.Walking;
			}
			return state;
		}

		protected virtual void SetupModules()
		{
			Motor = new global::PlayerRoles.FirstPersonControl.FpcMotor(Hub, this, !Hub.IsSCP());
			Noclip = new global::PlayerRoles.FirstPersonControl.FpcNoclip(Hub, this);
			MouseLook = new global::PlayerRoles.FirstPersonControl.FpcMouseLook(Hub, this);
			StateProcessor = new global::PlayerRoles.FirstPersonControl.FpcStateProcessor(Hub, this);
		}

		public void ServerOverridePosition(global::UnityEngine.Vector3 position, global::UnityEngine.Vector3 deltaRotation)
		{
			Position = position;
			Hub.connectionToClient.Send(new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcOverrideMessage(position, deltaRotation.y));
			OnServerPositionOverwritten();
		}

		public virtual float VelocityForState(global::PlayerRoles.FirstPersonControl.PlayerMovementState state, bool applyCrouch)
		{
			float num = 0f;
			switch (state)
			{
			case global::PlayerRoles.FirstPersonControl.PlayerMovementState.Crouching:
				num = CrouchSpeed;
				break;
			case global::PlayerRoles.FirstPersonControl.PlayerMovementState.Sneaking:
				num = SneakSpeed;
				break;
			case global::PlayerRoles.FirstPersonControl.PlayerMovementState.Sprinting:
				num = SprintSpeed;
				break;
			case global::PlayerRoles.FirstPersonControl.PlayerMovementState.Walking:
				num = WalkSpeed;
				break;
			}
			if (applyCrouch)
			{
				num = global::UnityEngine.Mathf.Lerp(num, CrouchSpeed, StateProcessor.CrouchPercent);
			}
			num *= Hub.inventory.MovementSpeedMultiplier;
			float num2 = Hub.inventory.MovementSpeedLimit;
			for (int i = 0; i < Hub.playerEffectsController.EffectsLength; i++)
			{
				if (Hub.playerEffectsController.AllEffects[i] is global::PlayerRoles.FirstPersonControl.IMovementSpeedModifier movementSpeedModifier && movementSpeedModifier.MovementModifierActive)
				{
					num2 = global::UnityEngine.Mathf.Min(num2, movementSpeedModifier.MovementSpeedLimit);
					num *= movementSpeedModifier.MovementSpeedMultiplier;
				}
			}
			return global::UnityEngine.Mathf.Min(num, num2);
		}

		public virtual void SpawnObject()
		{
			if (!TryGetComponent<global::PlayerRoles.PlayerRoleBase>(out var component) || !component.TryGetOwner(out var hub))
			{
				throw new global::System.InvalidOperationException("Movement module failed to initiate. Unable to find owner of the role.");
			}
			_activeUpdates = (global::System.Action)global::System.Delegate.Combine(_activeUpdates, new global::System.Action(UpdateMovement));
			Hub = hub;
			Role = component;
			_transform = Hub.transform;
			_speedState = global::PlayerRoles.FirstPersonControl.PlayerMovementState.Walking;
			SyncMovementState = global::PlayerRoles.FirstPersonControl.PlayerMovementState.Walking;
			if (global::Mirror.NetworkServer.active || Hub.isLocalPlayer)
			{
				CharController = Hub.GetComponent<global::UnityEngine.CharacterController>();
				CharacterControllerSettings.Apply(CharController);
				if (global::Mirror.NetworkServer.active)
				{
					Tracer = new MovementTracer(15, 3, 50f);
				}
				if (Hub.isLocalPlayer)
				{
					global::CursorManagement.CursorManager.Register(this);
				}
			}
			SetupModules();
			global::PlayerRoles.PlayerRoleBase role = Role;
			role.OnRoleDisabled = (global::System.Action<global::PlayerRoles.RoleTypeId>)global::System.Delegate.Combine(role.OnRoleDisabled, new global::System.Action<global::PlayerRoles.RoleTypeId>(OnRoleDisabled));
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved += Motor.OnElevatorMoved;
			if (global::GameObjectPools.PoolManager.Singleton.TryGetPoolObject(CharacterModelTemplate, _transform, out var poolObject) && poolObject is global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel characterModel)
			{
				CharacterModelInstance = characterModel;
				global::UnityEngine.Transform transform = CharacterModelTemplate.transform;
				global::UnityEngine.Transform obj = characterModel.transform;
				obj.localPosition = transform.position;
				obj.localScale = transform.localScale;
				obj.localRotation = transform.rotation;
			}
			else
			{
				global::UnityEngine.Debug.LogError("Can't spawn '" + CharacterModelTemplate.name + "' - FPC models must derive from CharacterModel.");
			}
			ModuleReady = true;
		}

		public virtual void ResetObject()
		{
			global::CursorManagement.CursorManager.Unregister(this);
			_activeUpdates = (global::System.Action)global::System.Delegate.Remove(_activeUpdates, new global::System.Action(UpdateMovement));
			global::PlayerRoles.PlayerRoleBase role = Role;
			role.OnRoleDisabled = (global::System.Action<global::PlayerRoles.RoleTypeId>)global::System.Delegate.Remove(role.OnRoleDisabled, new global::System.Action<global::PlayerRoles.RoleTypeId>(OnRoleDisabled));
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved -= Motor.OnElevatorMoved;
			ModuleReady = false;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			StaticUnityMethods.OnUpdate += delegate
			{
				if (_activeUpdates != null)
				{
					_activeUpdates();
					global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule.OnPositionUpdated?.Invoke();
				}
			};
		}
	}
}
