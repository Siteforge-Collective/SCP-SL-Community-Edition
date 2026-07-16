using System;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class SteamDebugCommand : ICommand
	{
		public string Command { get; } = "steamdebug";

		public string[] Aliases { get; }

		public string Description { get; } = "Toggles steam debug";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			PlayerPrefsSl.Set("steam_debug", !PlayerPrefsSl.Get("steam_debug", defaultValue: false));
			response = string.Format("Steam debug: {0}", PlayerPrefsSl.Get("steam_debug", defaultValue: false));
			return true;
		}
	}
}
