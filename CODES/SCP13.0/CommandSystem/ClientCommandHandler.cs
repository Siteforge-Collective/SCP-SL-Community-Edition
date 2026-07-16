using CommandSystem.Commands.Dot;
using CommandSystem.Commands.Dot.Overwatch;
using CommandSystem.Commands.Shared;

namespace CommandSystem
{
	public class ClientCommandHandler : CommandHandler
	{
		private ClientCommandHandler()
		{
		}

		public static ClientCommandHandler Create()
		{
			ClientCommandHandler clientCommandHandler = new ClientCommandHandler();
			clientCommandHandler.LoadGeneratedCommands();
			return clientCommandHandler;
		}

		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new ContactCommand());
			RegisterCommand(new GlobalTagCommand());
			RegisterCommand(new HelpCommand(this));
			RegisterCommand(new HideTagCommand());
			RegisterCommand(new ShowTagCommand());
			RegisterCommand(new SrvCfgCommand());
			RegisterCommand(new GroupsCommand());
			RegisterCommand(OverwatchCommand.Create());
		}
	}
}
