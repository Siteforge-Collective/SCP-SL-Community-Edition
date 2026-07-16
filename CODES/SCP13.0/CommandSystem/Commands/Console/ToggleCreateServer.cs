using System;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class ToggleCreateServer : ICommand
	{
		public string Command { get; }

		public string[] Aliases { get; }

		public string Description { get; }

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			return false;
		}
	}
}
