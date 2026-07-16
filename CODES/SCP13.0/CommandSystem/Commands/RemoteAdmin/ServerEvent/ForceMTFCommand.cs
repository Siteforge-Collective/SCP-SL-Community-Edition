using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.ServerEvent
{
	[CommandHandler(typeof(ServerEventCommand))]
	public class ForceMTFCommand : ICommand
	{
		public string Command { get; } = "FORCE_MTF";

		public string[] Aliases { get; }

		public string Description { get; } = "Forces the next team to respawn to be Mobile Task Forces.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			RespawnManager.Singleton.NextKnownTeam = SpawnableTeamType.NineTailedFox;
			RespawnTokensManager.ForceTeamDominance(SpawnableTeamType.NineTailedFox, 1f);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced MTF respawn next.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			response = "Mobile Task Forces will be respawning next.";
			return true;
		}
	}
}
