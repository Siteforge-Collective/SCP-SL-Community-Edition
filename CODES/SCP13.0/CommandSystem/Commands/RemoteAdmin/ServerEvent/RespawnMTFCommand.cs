using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.ServerEvent
{
	[CommandHandler(typeof(ServerEventCommand))]
	public class RespawnMTFCommand : ICommand
	{
		public string Command { get; } = "RESPAWN_MTF";

		public string[] Aliases { get; }

		public string Description { get; } = "Forces the MTF to respawn.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced MTF respawn.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			response = "MTF respawn forced.";
			return true;
		}
	}
}
