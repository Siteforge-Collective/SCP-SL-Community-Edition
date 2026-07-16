namespace PlayerRoles.PlayableScps
{
	public class FpcStandardScp : global::PlayerRoles.FirstPersonControl.FpcStandardRoleBase
	{
		private static readonly global::UnityEngine.Color DarkenedAmbientColor = new global::UnityEngine.Color(0.25f, 0.25f, 0.25f);

		private global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint _cachedSpawnpoint;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.RoleTypeId _roleTypeId;

		[global::UnityEngine.SerializeField]
		private int _maxHealth;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint _roomSpawnpoint;

		[global::UnityEngine.SerializeField]
		private bool _disableSpawnpoint;

		public override global::PlayerRoles.RoleTypeId RoleTypeId => _roleTypeId;

		public override global::PlayerRoles.Team Team => global::PlayerRoles.Team.SCPs;

		public override global::UnityEngine.Color RoleColor => global::UnityEngine.Color.red;

		public override float MaxHealth => _maxHealth;

		public override global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler SpawnpointHandler
		{
			get
			{
				if (_disableSpawnpoint)
				{
					return null;
				}
				if (_cachedSpawnpoint == null)
				{
					_cachedSpawnpoint = new global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint(_roomSpawnpoint);
				}
				return _cachedSpawnpoint;
			}
		}

		public override global::UnityEngine.Color AmbientLight
		{
			get
			{
				if (!InsufficientLight)
				{
					return base.AmbientLight;
				}
				return DarkenedAmbientColor;
			}
		}
	}
}
