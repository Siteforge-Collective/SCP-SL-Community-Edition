namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079AccentedNotification : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079SimpleNotification
	{
		private const string FormatStartColor = "<color={0}>";

		private const string FormatEndColor = "</color>";

		private const char ToggleChar = '$';

		private const string AccentColor = "#00a2ff";

		public Scp079AccentedNotification(string message, string color = "#00a2ff", char triggerChar = '$')
			: base(ProcessText(message, color, triggerChar))
		{
		}

		private static string ProcessText(string message, string color, char triggerChar)
		{
			global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
			bool flag = false;
			string text = $"<color={color}>";
			foreach (char c in message)
			{
				if (c != triggerChar)
				{
					stringBuilder.Append(c);
					continue;
				}
				stringBuilder.Append(flag ? "</color>" : text);
				flag = !flag;
			}
			if (flag)
			{
				stringBuilder.Append("</color>");
			}
			return global::NorthwoodLib.Pools.StringBuilderPool.Shared.ToStringReturn(stringBuilder);
		}
	}
}
