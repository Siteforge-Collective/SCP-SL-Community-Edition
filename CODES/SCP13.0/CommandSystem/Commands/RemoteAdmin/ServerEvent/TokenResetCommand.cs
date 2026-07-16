using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.ServerEvent
{
	[CommandHandler(typeof(ServerEventCommand))]
	public class TokenResetCommand : ICommand
	{
		public string Command { get; } = "TOKEN_RESET";

		public string[] Aliases { get; }

		public string Description { get; } = "Forces the tokens to be reset to their starting values.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			RespawnManager.Singleton.NextKnownTeam = SpawnableTeamType.None;
			RespawnTokensManager.ResetTokens();
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced token reset.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			response = "Tokens have been reset.";
			return true;
		}
	}
}
