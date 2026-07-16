namespace PlayerRoles.PlayableScps.Subroutines
{
	public static class SubroutineMessageReaderWriter
	{
		public static void WriteSubroutineMessage(this global::Mirror.NetworkWriter writer, global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessage msg)
		{
			msg.Write(writer);
		}

		public static global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessage ReadSubroutineMessage(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessage(reader);
		}
	}
}
