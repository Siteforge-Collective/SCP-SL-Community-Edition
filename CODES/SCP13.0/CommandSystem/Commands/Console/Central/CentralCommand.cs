using System;

namespace CommandSystem.Commands.Console.Central
{
	[CommandHandler(typeof(GameConsoleCommandHandler))]
	public class CentralCommand : ParentCommand, IHiddenCommand
	{
		public override string Command { get; }

		public override string[] Aliases { get; }

		public override string Description { get; }

		public static CentralCommand Create()
		{
			return null;
		}

		protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			return false;
		}

		public override void LoadGeneratedCommands()
		{
		}
	}
}
