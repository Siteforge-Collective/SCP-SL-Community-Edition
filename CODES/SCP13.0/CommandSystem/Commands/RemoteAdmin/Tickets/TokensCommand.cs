using System;
using Respawning;
using UnityEngine;

namespace CommandSystem.Commands.RemoteAdmin.Tickets
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class TokensCommand : ParentCommand, IUsageProvider
	{
		public override string Command { get; } = "tickets";

		public override string[] Aliases { get; } = new string[1] { "tix" };

		public override string Description { get; } = "Reads or sets the amount of NTF/CI respawn tokens.";

		public string[] Usage { get; } = new string[2] { "NTF/CI/Fetch/Info", "Value (Optional)" };

		public static TokensCommand Create()
		{
			TokensCommand tokensCommand = new TokensCommand();
			tokensCommand.LoadGeneratedCommands();
			return tokensCommand;
		}

		protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			float num = Mathf.Round(RespawnTokensManager.GetTeamDominance(SpawnableTeamType.NineTailedFox) * 100000f) * 0.001f;
			response = $"Domination: <color=#4179D6>MTF: {num}%</color> - <color=#3DB735>CI: {100f - num}%</color>";
			return false;
		}

		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new GrantCommand());
			RegisterCommand(new InfoCommand());
		}
	}
}
