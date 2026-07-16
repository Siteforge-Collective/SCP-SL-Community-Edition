namespace PlayerRoles.FirstPersonControl
{
	public abstract class FpcStandardRoleBase : global::PlayerRoles.PlayerRoleBase, global::PlayerRoles.SpawnData.ISpawnDataReader, global::PlayerRoles.SpawnData.IPrivateSpawnDataWriter, global::PlayerRoles.SpawnData.IPublicSpawnDataWriter, global::PlayerRoles.IHealthbarRole, global::PlayerRoles.IRagdollRole, global::PlayerRoles.FirstPersonControl.IFpcRole, global::GameObjectPools.IPoolSpawnable, global::PlayerRoles.ICameraController, global::PlayerRoles.IAvatarRole, global::PlayerRoles.Spectating.ISpectatableRole, global::PlayerRoles.Voice.IVoiceRole, global::PlayerRoles.Visibility.ICustomVisibilityRole, IViewmodelRole, global::PlayerRoles.IAmbientLightRole, global::PlayerRoles.IAFKRole
	{
		private global::UnityEngine.Vector3 _lastPos;

		private const float PositionalTolerance = 1.25f;

		private global::UnityEngine.Transform _hubTransform;

		private global::UnityEngine.Transform _cameraTransform;

		private global::CustomPlayerEffects.InsufficientLighting _noLightFx;

		public virtual global::UnityEngine.Vector3 CameraPosition => _cameraTransform.position;

		public virtual float VerticalRotation => _cameraTransform.eulerAngles.x;

		public virtual float HorizontalRotation => _cameraTransform.eulerAngles.y;

		public virtual global::PlayerStatsSystem.PlayerStats TargetStats
		{
			get
			{
				if (!TryGetOwner(out var hub))
				{
					return null;
				}
				return hub.playerStats;
			}
		}

		public abstract float MaxHealth { get; }

		public abstract global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler SpawnpointHandler { get; }

		public virtual global::UnityEngine.Color AmbientLight
		{
			get
			{
				if (!InsufficientLight && !InDarkness)
				{
					return VeryHighPerformance.TargetColor;
				}
				return global::UnityEngine.Color.black;
			}
		}

		public virtual bool InsufficientLight
		{
			get
			{
				if (!global::Mirror.NetworkServer.active)
				{
					return _noLightFx.IsEnabled;
				}
				if (InDarkness)
				{
					return !HasFlashlight;
				}
				return false;
			}
		}

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule FpcModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.Spectating.SpectatableModuleBase SpectatorModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.Voice.VoiceModuleBase VoiceModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.Visibility.VisibilityController VisibilityController { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public BasicRagdoll Ragdoll { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Texture RoleAvatar { get; private set; }

		private bool InDarkness => FlickerableLightController.IsInDarkenedRoom(FpcModule.Position);

		private bool HasFlashlight
		{
			get
			{
				if (!TryGetOwner(out var hub))
				{
					return false;
				}
				if (hub.inventory.CurInstance is global::InventorySystem.Items.ILightEmittingItem lightEmittingItem && lightEmittingItem != null)
				{
					return lightEmittingItem.IsEmittingLight;
				}
				return false;
			}
		}

		public bool IsAFK
		{
			get
			{
				if (_lastPos == global::UnityEngine.Vector3.zero)
				{
					_lastPos = _hubTransform.position;
					return true;
				}
				if ((_lastPos - _hubTransform.position).sqrMagnitude < 1.25f)
				{
					return true;
				}
				_lastPos = _hubTransform.position;
				return false;
			}
		}

		protected virtual void ShowStartScreen()
		{
			if (base.IsLocalPlayer)
			{
				StartScreen.Show(this);
			}
		}

		internal override void Init(ReferenceHub hub, global::PlayerRoles.RoleChangeReason spawnReason, global::PlayerRoles.RoleSpawnFlags spawnFlags)
		{
			base.Init(hub, spawnReason, spawnFlags);
			_noLightFx = hub.playerEffectsController.GetEffect<global::CustomPlayerEffects.InsufficientLighting>();
			_lastPos = global::UnityEngine.Vector3.zero;
		}

		public virtual void ReadSpawnData(global::Mirror.NetworkReader reader)
		{
			global::RelativePositioning.RelativePosition receivedPosition = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
			FpcModule.MouseLook.ApplySyncValues(global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), 32767);
			if (receivedPosition.WaypointId == 0)
			{
				return;
			}
			if (!base.IsLocalPlayer)
			{
				global::PlayerRoles.FirstPersonControl.FpcMotor motor = FpcModule.Motor;
				if (motor.ReceivedPosition.WaypointId != 0)
				{
					return;
				}
				motor.ReceivedPosition = receivedPosition;
			}
			FpcModule.Position = receivedPosition.Position;
		}

		public virtual void WritePublicSpawnData(global::Mirror.NetworkWriter writer)
		{
			FpcModule.MouseLook.GetSyncValues(0, out var syncH, out var _);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(_hubTransform.position));
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, syncH);
		}

		public virtual void WritePrivateSpawnData(global::Mirror.NetworkWriter writer)
		{
		}

		public virtual void SpawnObject()
		{
			if (TryGetOwner(out var hub))
			{
				_hubTransform = hub.transform;
				_cameraTransform = hub.PlayerCameraReference;
				ShowStartScreen();
			}
		}
	}
}
