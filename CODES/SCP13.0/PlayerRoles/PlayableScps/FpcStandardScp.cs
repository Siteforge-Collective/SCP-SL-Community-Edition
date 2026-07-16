using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Spawnpoints;
using UnityEngine;

namespace PlayerRoles.PlayableScps
{
	public class FpcStandardScp : FpcStandardRoleBase
	{
		private RoomRoleSpawnpoint _cachedSpawnpoint;

		[SerializeField]
		private RoleTypeId _roleTypeId;

		[SerializeField]
		private int _maxHealth;

		[SerializeField]
		private RoomRoleSpawnpoint _roomSpawnpoint;

		[SerializeField]
		private bool _disableSpawnpoint;

		public override RoleTypeId RoleTypeId => default(RoleTypeId);

		public override Team Team => default(Team);

		public override Color RoleColor => default(Color);

		public override float MaxHealth => 0f;

		public override ISpawnpointHandler SpawnpointHandler => null;

		public override float AmbientLight => 0f;

		public override bool TryGetViewmodelFov(out float fov)
		{
			fov = default(float);
			return false;
		}
	}
}
