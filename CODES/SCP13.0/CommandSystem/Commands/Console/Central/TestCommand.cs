using System;

namespace CommandSystem.Commands.Console.Central
{
	[CommandHandler(typeof(CentralCommand))]
	public class TestCommand : ICommand
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
