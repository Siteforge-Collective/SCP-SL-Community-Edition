using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.ServerEvent
{
	[CommandHandler(typeof(ServerEventCommand))]
	public class PlayEffectMTFCommand : ICommand
	{
		public string Command { get; } = "PLAY_EFFECT_MTF";

		public string[] Aliases { get; }

		public string Description { get; } = "Forces the MTF Helicopter to appear.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " forced MTF Helicopter to spawn.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			response = "MTF Helicopter spawned.";
			return true;
		}
	}
}
