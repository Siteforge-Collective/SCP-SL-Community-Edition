using System;

namespace CommandSystem.Commands.Console.Prefs
{
	[CommandHandler(typeof(PrefsCommand))]
	public class SetCommand : ICommand, IUsageProvider
	{
		public string Command { get; }

		public string[] Aliases { get; }

		public string Description { get; }

		public string[] Usage { get; }

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			return false;
		}
	}
}
