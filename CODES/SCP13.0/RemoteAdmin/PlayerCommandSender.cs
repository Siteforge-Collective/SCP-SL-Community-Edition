namespace RemoteAdmin
{
	public class PlayerCommandSender : CommandSender
	{
		public readonly ReferenceHub ReferenceHub;

		public override string SenderId => null;

		public int PlayerId => 0;

		public override string Nickname => null;

		public override ulong Permissions => 0uL;

		public override byte KickPower => 0;

		public override bool FullPermissions => false;

		public override string LogName => null;

		public PlayerCommandSender(ReferenceHub hub)
		{
		}

		public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
		{
		}

		public override void Print(string text)
		{
		}
	}
}
