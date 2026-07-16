namespace CommandSystem
{
	public class GameConsoleCommandHandler : global::CommandSystem.CommandHandler
	{
		private GameConsoleCommandHandler()
		{
		}

		public static global::CommandSystem.GameConsoleCommandHandler Create()
		{
			global::CommandSystem.GameConsoleCommandHandler gameConsoleCommandHandler = new global::CommandSystem.GameConsoleCommandHandler();
			gameConsoleCommandHandler.LoadGeneratedCommands();
			return gameConsoleCommandHandler;
		}

		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new global::CommandSystem.Commands.Shared.BuildInfoCommand());
			RegisterCommand(global::CommandSystem.Commands.Shared.ConfigCommand.Create());
			RegisterCommand(new global::CommandSystem.Commands.Shared.HelloCommand());
			RegisterCommand(new global::CommandSystem.Commands.Shared.HelpCommand(this));
			RegisterCommand(new global::CommandSystem.Commands.Shared.RefreshCommandsCommand(this));
			RegisterCommand(new global::CommandSystem.Commands.Shared.RestartNextRoundCommand());
			RegisterCommand(new global::CommandSystem.Commands.Shared.RidListCommand());
			RegisterCommand(new global::CommandSystem.Commands.Shared.SoftRestartCommand());
			RegisterCommand(new global::CommandSystem.Commands.Shared.StopNextRoundCommand());
			RegisterCommand(new global::CommandSystem.Commands.Shared.UptimeRoundsCommand());
			RegisterCommand(new global::CommandSystem.Commands.RemoteAdmin.RedirectCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.ArgsCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.BanCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.ContactCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.GroupsCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.IdCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.IpCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.ItemListCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.KeyCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.LennyCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.PlayersCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.QuitCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.ReloadTranslationsCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.RoleListCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.SeedCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.SrvCfgCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.SteamDebugCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.PocketDimensionCommand());
			RegisterCommand(new global::CommandSystem.Commands.Console.RoundRestartCommand());
		}
	}
}
