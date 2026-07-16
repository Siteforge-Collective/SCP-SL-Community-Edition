using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.ServerEvent
{
	[CommandHandler(typeof(ServerEventCommand))]
	public class PlayEffectCICommand : ICommand
	{
		public string Command { get; } = "PLAY_EFFECT_CI";

		public string[] Aliases { get; }

		public string Description { get; } = "Forces the CI Van to appear.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced CI Van to spawn.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			response = "CI Van spawned.";
			return true;
		}
	}
}
