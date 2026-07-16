using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.Tickets
{
	[CommandHandler(typeof(TokensCommand))]
	public class InfoCommand : ICommand
	{
		public string Command { get; } = "info";

		public string[] Aliases { get; } = new string[1] { "fetch" };

		public string Description { get; } = "Fetches the ticket information.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			response = RespawnManager.GetRemoteAdminInfoString() ?? "";
			return true;
		}
	}
}
