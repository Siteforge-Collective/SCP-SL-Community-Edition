using System;

namespace CommandSystem.Commands.RemoteAdmin
{
	[CommandHandler(typeof(ElevatorCommand))]
	public class ElevatorUnlockCommand : ICommand
	{
		public string Command { get; } = "unlock";

		public string[] Aliases { get; } = new string[3] { "u", "ul", "ulck" };

		public string Description { get; } = "Unlocks an elevator.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out response))
			{
				return false;
			}
			if (arguments.Count == 1)
			{
				return ElevatorLockCommand.TrySetLock(arguments.At(0), locked: false, out response, sender);
			}
			response = "Syntax error: elevator unlock <Elevator ID / all>";
			return false;
		}
	}
}
