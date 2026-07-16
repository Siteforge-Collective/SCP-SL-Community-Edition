namespace PlayerRoles.RoleAssign
{
	public class OneRoleHumanSpawner : global::PlayerRoles.RoleAssign.IHumanSpawnHandler
	{
		public global::PlayerRoles.RoleTypeId NextRole { get; }

		public OneRoleHumanSpawner(global::PlayerRoles.RoleTypeId targetRole)
		{
			NextRole = targetRole;
		}
	}
}
