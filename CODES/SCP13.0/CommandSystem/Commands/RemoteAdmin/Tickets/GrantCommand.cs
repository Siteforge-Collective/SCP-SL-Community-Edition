using System;
using Respawning;

namespace CommandSystem.Commands.RemoteAdmin.Tickets
{
	[CommandHandler(typeof(TokensCommand))]
	public class GrantCommand : ICommand
	{
		public string Command { get; } = "grant";

		public string[] Aliases { get; }

		public string Description { get; } = "Grants a team a specific amount of tokens.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.RespawnEvents, out response))
			{
				return false;
			}
			response = "You must specify a valid team and token amount.";
			if (arguments.Count <= 1)
			{
				return false;
			}
			string text = arguments.At(0).ToUpper();
			if (!Enum.TryParse<SpawnableTeamType>(text, ignoreCase: true, out var result))
			{
				switch (text)
				{
				case "CI":
				case "CHI":
				case "CHAOS":
					result = SpawnableTeamType.ChaosInsurgency;
					break;
				case "NTF":
				case "MTF":
				case "MOBILE":
					result = SpawnableTeamType.NineTailedFox;
					break;
				default:
					return false;
				}
			}
			if (!float.TryParse(arguments.At(1), out var result2))
			{
				return false;
			}
			float teamDominance = RespawnTokensManager.GetTeamDominance(result);
			RespawnTokensManager.ForceTeamDominance(result, result2 + teamDominance);
			float teamDominance2 = RespawnTokensManager.GetTeamDominance(result);
			response = $"Set the <color=yellow>{result}'s</color> tokens to <color=orange>{teamDominance2}</color>. (Previous value: <color=orange>{teamDominance}</color>)</color>";
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, $"{sender.LogName} set the {result}'s tokens to {teamDominance2}. Previous value: {teamDominance}.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			return true;
		}
	}
}
