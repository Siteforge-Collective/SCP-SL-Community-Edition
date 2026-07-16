using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.ServerEvent
{
	[CommandHandler(typeof(ServerEventCommand))]
	public class ForceCICommand : ICommand
	{
		public string Command { get; } = "FORCE_CI";

		public string[] Aliases { get; }

		public string Description { get; } = "Forces the next team to respawn to be Chaos Insurgency.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			RespawnManager.Singleton.NextKnownTeam = SpawnableTeamType.ChaosInsurgency;
			RespawnTokensManager.ForceTeamDominance(SpawnableTeamType.ChaosInsurgency, 1f);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced CI respawn next.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			response = "Chaos Insurgency will be respawning next.";
			return true;
		}
	}
}
