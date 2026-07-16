namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173MovementModule : global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule
	{
		private float _normalSpeed;

		private float _fastSpeed;

		private float _observerSpeed;

		private float _jumpSpeed;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173Role _role;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility _breakneckSpeeds;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observersTracker;

		private static int _snapMask;

		private readonly global::System.Diagnostics.Stopwatch _lookStopwatch = global::System.Diagnostics.Stopwatch.StartNew();

		private const float ObserverSpeedMultiplier = 2f;

		private const float ServerStopTime = 0.4f;

		private const int GlassLayerMask = 16384;

		private const float GlassRaycastDis = 0.3f;

		private const float RaycastFloorHeight = 3.6f;

		private const float RaycastCeilHeight = 7.2f;

		private const float RaycastPilotRadius = 0.025f;

		private const float RaycastFloorDot = 0.15f;

		private const float RaycastCcRadiusMultiplier = 1.2f;

		private const float RaycastStabilityRadiusRatio = 0.5f;

		private const float RaycastStabilityDistance = 0.5f;

		private float MovementSpeed
		{
			set
			{
				SneakSpeed = value;
				WalkSpeed = value;
				SprintSpeed = value;
				JumpSpeed = ((value < _normalSpeed) ? 0f : _jumpSpeed);
			}
		}

		private float TargetSpeed
		{
			get
			{
				if (_observersTracker.IsObserved)
				{
					return 0f;
				}
				if (!_breakneckSpeeds.IsActive)
				{
					return _normalSpeed;
				}
				return _fastSpeed;
			}
		}

		private float ServerSpeed
		{
			get
			{
				float targetSpeed = TargetSpeed;
				if (targetSpeed > 0f)
				{
					_lookStopwatch.Restart();
					return targetSpeed;
				}
				if (!(_lookStopwatch.Elapsed.TotalSeconds < 0.4000000059604645))
				{
					return 0f;
				}
				return _normalSpeed;
			}
		}

		private static int TpMask
		{
			get
			{
				if (_snapMask != 0)
				{
					return _snapMask;
				}
				int layer = global::UnityEngine.LayerMask.NameToLayer("Player");
				for (int i = 0; i < 32; i++)
				{
					if (!global::UnityEngine.Physics.GetIgnoreLayerCollision(layer, i))
					{
						_snapMask |= 1 << i;
					}
				}
				return _snapMask;
			}
		}

		private void Awake()
		{
			_normalSpeed = WalkSpeed;
			_fastSpeed = SprintSpeed;
			_jumpSpeed = JumpSpeed;
			_observerSpeed = SprintSpeed * 2f;
			_role = GetComponent<global::PlayerRoles.PlayableScps.Scp173.Scp173Role>();
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility>(out _breakneckSpeeds);
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observersTracker);
		}

		protected override void UpdateMovement()
		{
			MovementSpeed = (_role.IsLocalPlayer ? TargetSpeed : (global::Mirror.NetworkServer.active ? ServerSpeed : _observerSpeed));
			base.UpdateMovement();
			UpdateGlassBreaking();
		}

		private void UpdateGlassBreaking()
		{
			if (global::Mirror.NetworkServer.active && _breakneckSpeeds.IsActive)
			{
				global::UnityEngine.Vector3 moveDirection = base.Motor.MoveDirection;
				float maxDistance = base.CharController.radius + 0.3f;
				if (global::UnityEngine.Physics.Raycast(base.Position, moveDirection, out var hitInfo, maxDistance, 16384) && hitInfo.collider.TryGetComponent<BreakableWindow>(out var component))
				{
					component.Damage(component.health, _role.DamageHandler, global::UnityEngine.Vector3.zero);
				}
			}
		}

		public bool TryGetTeleportPos(float maxDis, out global::UnityEngine.Vector3 pos, out float usedDistance)
		{
			float radius = CharacterControllerSettings.Radius;
			float num = radius * 1.2f;
			global::UnityEngine.Vector3 position = base.Hub.PlayerCameraReference.position;
			global::UnityEngine.Vector3 forward = base.Hub.PlayerCameraReference.forward;
			if (!global::UnityEngine.Physics.SphereCast(position, 0.025f, forward, out var hitInfo, maxDis, TpMask))
			{
				hitInfo.point = position + forward * maxDis;
				hitInfo.normal = global::UnityEngine.Vector3.up;
				hitInfo.distance = maxDis;
			}
			usedDistance = hitInfo.distance;
			pos = hitInfo.point + hitInfo.normal * num;
			if (global::UnityEngine.Physics.CheckSphere(pos, radius, TpMask))
			{
				return false;
			}
			if (!global::UnityEngine.Physics.SphereCast(pos, radius, global::UnityEngine.Vector3.down, out var hitInfo2, 3.6f, TpMask))
			{
				return false;
			}
			if (!global::UnityEngine.Physics.SphereCast(new global::UnityEngine.Ray(pos, global::UnityEngine.Vector3.down), radius * 0.5f, hitInfo2.distance + 0.5f, TpMask))
			{
				return false;
			}
			if (global::UnityEngine.Vector3.Dot(global::UnityEngine.Vector3.up, hitInfo2.normal) < 0.15f)
			{
				return false;
			}
			if (!global::UnityEngine.Physics.SphereCast(pos, radius, global::UnityEngine.Vector3.up, out var hitInfo3, 7.2f, TpMask))
			{
				hitInfo3.point = pos + global::UnityEngine.Vector3.up * 7.2f;
			}
			if (global::UnityEngine.Mathf.Abs(hitInfo2.point.y - hitInfo3.point.y) < CharacterControllerSettings.Height)
			{
				return false;
			}
			if (hitInfo2.collider.TryGetComponent<CheckpointKiller>(out var _))
			{
				return false;
			}
			pos = hitInfo2.point + (hitInfo2.normal + global::UnityEngine.Vector3.down) * radius;
			return true;
		}

		public void ServerTeleportTo(global::UnityEngine.Vector3 pos)
		{
			if (global::Mirror.NetworkServer.active)
			{
				base.Position = pos;
				base.CharController.SimpleMove(global::UnityEngine.Vector3.zero);
				ServerOverridePosition(base.transform.position, global::UnityEngine.Vector3.zero);
			}
		}
	}
}
