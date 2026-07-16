using System;
using UnityEngine;

namespace CommandSystem.Commands.Console
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class MeasureCommand : ICommand
	{
		private static Vector3 _pos;

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
