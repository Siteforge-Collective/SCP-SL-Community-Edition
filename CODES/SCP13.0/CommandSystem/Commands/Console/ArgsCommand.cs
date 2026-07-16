using System;
using System.Linq;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class ArgsCommand : ICommand
	{
		public string Command { get; } = "args";

		public string[] Aliases { get; }

		public string Description { get; } = "Prints all startup args";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = StartupArgs.Args.Aggregate("Startup args:\n", (string current, string arg) => current + "- " + arg + "\n");
			return true;
		}
	}
}
