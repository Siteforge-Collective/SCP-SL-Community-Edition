using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.ServerEvent
{
	[CommandHandler(typeof(ServerEventCommand))]
	public class RespawnCICommand : ICommand
	{
		public string Command { get; } = "RESPAWN_CI";

		public string[] Aliases { get; }

		public string Description { get; } = "Forces the CI to respawn.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.ChaosInsurgency);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced CI respawn.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			response = "CI respawn forced.";
			return true;
		}
	}
}
