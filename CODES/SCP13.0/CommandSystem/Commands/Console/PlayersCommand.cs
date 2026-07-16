using System;
using System.Collections.Generic;
using System.Text;
using CentralAuth;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class PlayersCommand : ICommand
	{
		public string Command { get; } = "players";

		public string[] Aliases { get; } = new string[2] { "pl", "list" };

		public string Description { get; } = "Displays a list of all players.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (ReferenceHub.LocalHub == null)
			{
				response = "You must join a server to execute this command.";
				return false;
			}
			HashSet<ReferenceHub> allHubs = ReferenceHub.AllHubs;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"<color=cyan>List of players ({(ServerStatic.IsDedicated ? (allHubs.Count - 1) : allHubs.Count)}):</color>");
			foreach (ReferenceHub item in allHubs)
			{
				if (item.Mode != ClientInstanceMode.DedicatedServer)
				{
					stringBuilder.AppendLine(string.Format("- {0}: {1} [{2}]", item.nicknameSync.CombinedName ?? "(no nickname)", item.authManager.UserId ?? "(no User ID)", item.PlayerId));
				}
			}
			response = stringBuilder.ToString();
			return true;
		}
	}
}
