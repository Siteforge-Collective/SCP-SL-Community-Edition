namespace PlayerRoles
{
	public interface IObfuscatedRole
	{
		global::PlayerRoles.RoleTypeId GetRoleForUser(ReferenceHub receiver);
	}
}
