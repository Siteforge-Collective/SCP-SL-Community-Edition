namespace CommandSystem
{
	public class ClientCommandHandler : global::CommandSystem.CommandHandler
	{
		private ClientCommandHandler()
		{
		}

		public static global::CommandSystem.ClientCommandHandler Create()
		{
			global::CommandSystem.ClientCommandHandler clientCommandHandler = new global::CommandSystem.ClientCommandHandler();
			clientCommandHandler.LoadGeneratedCommands();
			return clientCommandHandler;
		}

		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new global::CommandSystem.Commands.Shared.HelpCommand(this));
		}
	}
}
