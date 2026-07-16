using System;
using InventorySystem.Items.Firearms.Modules;

namespace CommandSystem.Commands.RemoteAdmin
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class GunDebugCommand : ICommand
	{
		public string Command { get; } = "gundebug";

		public string[] Aliases { get; } = new string[3] { "gunsdebug", "weaponsdebug", "weapondebug" };

		public string Description { get; } = "Toggles debug mode for firearms.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(new PlayerPermissions[1] { PlayerPermissions.FacilityManagement }, out response))
			{
				return false;
			}
			StandardHitregBase.DebugMode = !StandardHitregBase.DebugMode;
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " toggled firearms debug mode.", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
			return true;
		}
	}
}
