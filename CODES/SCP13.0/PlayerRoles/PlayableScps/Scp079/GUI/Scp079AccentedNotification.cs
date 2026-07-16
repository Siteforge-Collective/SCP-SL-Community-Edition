namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079AccentedNotification : Scp079SimpleNotification
	{
		public const char ToggleChar = '$';

		public const string AccentColor = "#00a2ff";

		private const string FormatStartColor = "<color={0}>";

		private const string FormatEndColor = "</color>";

		public Scp079AccentedNotification(string message, string color = "#00a2ff", char triggerChar = '$')
			: base(null)
		{
		}

		private static string ProcessText(string message, string color, char triggerChar)
		{
			return null;
		}
	}
}
