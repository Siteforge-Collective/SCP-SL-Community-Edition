using System;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class LocalhostCommand : ICommand
	{
		public string Command { get; } = "localhost";

        public string[] Aliases { get; } = new string[] { "host" };

		public string Description { get; } = "Starts a server on localhost.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			CustomNetworkManager.CreatePop.SetActive(true);
            response = "Started server on localhost.";
			return true;
        }
	}
}
