namespace PlayerRoles.Spectating
{
	public class SpectatorRole : global::PlayerRoles.PlayerRoleBase, global::PlayerRoles.SpawnData.IPrivateSpawnDataWriter, global::PlayerRoles.IHealthbarRole, global::PlayerRoles.SpawnData.ISpawnDataReader, global::PlayerRoles.IAdvancedCameraController, global::PlayerRoles.ICameraController, global::GameObjectPools.IPoolSpawnable, global::PlayerRoles.Voice.IVoiceRole, IViewmodelRole, global::PlayerRoles.IAmbientLightRole
	{
		public global::PlayerRoles.Spectating.SpectatorTargetTracker TrackerPrefab;

		public const float SpawnHeight = 6000f;

		private global::UnityEngine.Transform _transformToRestore;

		private global::PlayerStatsSystem.DamageHandlerBase _damageHandler;

		public override global::PlayerRoles.RoleTypeId RoleTypeId => global::PlayerRoles.RoleTypeId.Spectator;

		public override global::PlayerRoles.Team Team => global::PlayerRoles.Team.Dead;

		public override global::UnityEngine.Color RoleColor => global::UnityEngine.Color.white;

		public float VerticalRotation => global::PlayerRoles.Spectating.SpectatorTargetTracker.CurrentOffset.rotation.x;

		public float HorizontalRotation => global::PlayerRoles.Spectating.SpectatorTargetTracker.CurrentOffset.rotation.y;

		public float RollRotation => global::PlayerRoles.Spectating.SpectatorTargetTracker.CurrentOffset.rotation.z;

		public global::UnityEngine.Vector3 CameraPosition => global::PlayerRoles.Spectating.SpectatorTargetTracker.CurrentOffset.position;

		public virtual bool ReadyToRespawn
		{
			get
			{
				if (TryGetOwner(out var hub))
				{
					return hub.characterClassManager.InstanceMode != ClientInstanceMode.DedicatedServer;
				}
				return false;
			}
		}

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.Voice.VoiceModuleBase VoiceModule { get; private set; }

		public global::RelativePositioning.RelativePosition DeathPosition { get; private set; }

		public uint SyncedSpectatedNetId { get; internal set; }

		public float MaxHealth => 0f;

		public global::PlayerStatsSystem.PlayerStats TargetStats
		{
			get
			{
				if (!global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub))
				{
					return null;
				}
				if (!(hub.roleManager.CurrentRole is global::PlayerRoles.IHealthbarRole healthbarRole))
				{
					return null;
				}
				return healthbarRole.TargetStats;
			}
		}

		public global::UnityEngine.Color AmbientLight
		{
			get
			{
				if (!global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) || !(hub.roleManager.CurrentRole is global::PlayerRoles.IAmbientLightRole ambientLightRole))
				{
					return global::CustomPlayerEffects.InsufficientLighting.DefaultColor;
				}
				return ambientLightRole.AmbientLight;
			}
		}

		public bool InsufficientLight => false;

		public override void DisableRole(global::PlayerRoles.RoleTypeId newRole)
		{
			base.DisableRole(newRole);
			_damageHandler = null;
			if (!(_transformToRestore == null))
			{
				_transformToRestore.position = DeathPosition.Position;
				_transformToRestore = null;
			}
		}

		public void SpawnObject()
		{
			if (!TryGetOwner(out var hub))
			{
				throw new global::System.InvalidOperationException("Spectator role failed to spawn - owner is null");
			}
			global::UnityEngine.Transform transform = hub.transform;
			DeathPosition = new global::RelativePositioning.RelativePosition(transform.position);
			transform.position = global::UnityEngine.Vector3.up * 6000f;
			SyncedSpectatedNetId = 0u;
			if (global::Mirror.NetworkServer.active || hub.isLocalPlayer)
			{
				_transformToRestore = transform;
			}
		}

		public void WritePrivateSpawnData(global::Mirror.NetworkWriter writer)
		{
			if (_damageHandler == null)
			{
				writer.WriteSpawnReason(global::PlayerRoles.Spectating.SpectatorSpawnReason.None);
			}
			else
			{
				_damageHandler.WriteDeathScreen(writer);
			}
			_damageHandler = null;
		}

		public void ReadSpawnData(global::Mirror.NetworkReader reader)
		{
			_ = base.IsLocalPlayer;
		}

		public void ServerSetData(global::PlayerStatsSystem.DamageHandlerBase dhb)
		{
			_damageHandler = dhb;
		}
	}
}
