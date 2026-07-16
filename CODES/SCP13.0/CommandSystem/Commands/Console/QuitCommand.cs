using System;
using ServerOutput;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class QuitCommand : ICommand
	{
		public string Command { get; } = "quit";

		public string[] Aliases { get; } = new string[2] { "exit", "stop" };

		public string Description { get; } = "Quit the game.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			IdleMode.SetIdleMode(state: false);
			ServerConsole.AddOutputEntry(default(ExitActionShutdownEntry));
			Shutdown.Quit();
			response = "<size=50>GOODBYE!</size>";
			return true;
		}
	}
}
