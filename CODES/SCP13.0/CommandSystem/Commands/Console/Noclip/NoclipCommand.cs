using System;

namespace CommandSystem.Commands.Console.Noclip
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class NoclipCommand : ICommand
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
