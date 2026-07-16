namespace PlayerRoles
{
	public static class PlayerRoleEnumsReadersWriters
	{
		public static void WriteRoleType(this global::Mirror.NetworkWriter writer, global::PlayerRoles.RoleTypeId role)
		{
			global::Mirror.NetworkWriterExtensions.WriteSByte(writer, (sbyte)role);
		}

		public static global::PlayerRoles.RoleTypeId ReadRoleType(this global::Mirror.NetworkReader reader)
		{
			return (global::PlayerRoles.RoleTypeId)global::Mirror.NetworkReaderExtensions.ReadSByte(reader);
		}
	}
}
