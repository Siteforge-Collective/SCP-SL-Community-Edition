using CommandSystem.Commands.Console;
using CommandSystem.Commands.RemoteAdmin;
using CommandSystem.Commands.RemoteAdmin.Stripdown;
using CommandSystem.Commands.Shared;
using _Scripts.CommandSystem.Commands.Console;

namespace CommandSystem
{
	public class GameConsoleCommandHandler : CommandHandler
	{
		private GameConsoleCommandHandler()
		{
        }

		public static GameConsoleCommandHandler Create()
		{
			GameConsoleCommandHandler gameConsoleCommandHandler = new GameConsoleCommandHandler();
			gameConsoleCommandHandler.LoadGeneratedCommands();
			return gameConsoleCommandHandler;
		}

		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new LocalhostCommand());
			RegisterCommand(new BufferSizeCommand());
			RegisterCommand(new PrrCommand());
			RegisterCommand(new BuildInfoCommand());
			RegisterCommand(ConfigCommand.Create());
			RegisterCommand(new ForceStartCommand());
			RegisterCommand(new HelloCommand());
			RegisterCommand(new HelpCommand(this));
			RegisterCommand(new NextRoundCommand());
			RegisterCommand(new RefreshCommandsCommand(this));
			RegisterCommand(new RestartNextRoundCommand());
			RegisterCommand(new RidListCommand());
			RegisterCommand(new SoftRestartCommand());
			RegisterCommand(new StopNextRoundCommand());
			RegisterCommand(new UptimeRoundsCommand());
			RegisterCommand(new RedirectCommand());
			RegisterCommand(StripdownCommand.Create());
			RegisterCommand(new ArgsCommand());
			RegisterCommand(new CommandSystem.Commands.Console.BanCommand());
			RegisterCommand(new IdCommand());
			RegisterCommand(new IdleCommand());
			RegisterCommand(new IpCommand());
			RegisterCommand(new ItemListCommand());
			RegisterCommand(new LennyCommand());
			RegisterCommand(new PlayersCommand());
			RegisterCommand(new QuitCommand());
			RegisterCommand(new ReloadTranslationsCommand());
			RegisterCommand(new RoleListCommand());
			RegisterCommand(new SeedCommand());
			RegisterCommand(new CommandSystem.Commands.Console.SrvCfgCommand());
			RegisterCommand(new SteamDebugCommand());
			RegisterCommand(new PocketDimensionCommand());
			RegisterCommand(new RoundRestartCommand());
		}
	}
}
