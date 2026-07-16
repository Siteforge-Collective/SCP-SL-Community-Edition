using System;
using System.Collections.Generic;
using System.Text;
using Interactables.Interobjects;
using NorthwoodLib.Pools;

namespace CommandSystem.Commands.RemoteAdmin
{
	[CommandHandler(typeof(ElevatorCommand))]
	public class ElevatorSendCommand : ICommand
	{
		public string Command { get; } = "send";

		public string[] Aliases { get; } = new string[2] { "s", "snd" };

		public string Description { get; } = "Sends an elevator.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out response))
			{
				return false;
			}
			if (arguments.Count < 1 || arguments.Count > 2)
			{
				response = "Syntax error: elevator send <Elevator ID / all> [level]";
				return false;
			}
			string text = arguments.At(0);
			bool flag = text.Equals("all", StringComparison.OrdinalIgnoreCase) || text.Equals("*", StringComparison.OrdinalIgnoreCase);
			int result = -1;
			if (arguments.Count > 1 && (!int.TryParse(arguments.At(1), out result) || result < 0))
			{
				response = "Level must be a nonnegative integer.";
				return false;
			}
			if (!flag)
			{
				if (!ElevatorCommand.TryParseGroup(text, out var group))
				{
					response = "Elevator \"" + text + "\" not found.";
					return false;
				}
				return SendElevator(group, result, out response, sender);
			}
			StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
			bool result2 = true;
			try
			{
				foreach (KeyValuePair<ElevatorManager.ElevatorGroup, List<ElevatorDoor>> allElevatorDoor in ElevatorDoor.AllElevatorDoors)
				{
					if (!SendElevator(allElevatorDoor.Key, -1, out var response2, sender))
					{
						result2 = false;
					}
					stringBuilder.AppendFormat("{0}\n", response2);
				}
				response = stringBuilder.ToString();
				return result2;
			}
			finally
			{
				StringBuilderPool.Shared.Return(stringBuilder);
			}
		}

		private static bool SendElevator(ElevatorManager.ElevatorGroup group, int level, out string response, ICommandSender sender)
		{
			if (!ElevatorDoor.AllElevatorDoors.TryGetValue(group, out var value))
			{
				response = $"Elevator \"{group}\" could not be found in the Facility.";
				return false;
			}
			if (level == -1)
			{
				if (value.Count != 2)
				{
					response = "Elevator has more than 2 levels. Please specify a level manually.";
					return false;
				}
				if (!ElevatorManager.SpawnedChambers.TryGetValue(group, out var value2) || value2 == null)
				{
					response = "Chamber for elevator \"" + group.ToString() + "\" is not spawned.";
					return false;
				}
				level = ((value2.CurrentLevel == 0) ? 1 : 0);
			}
			else if (level >= value.Count)
			{
				response = $"Elevator \"{group}\" does not have a level {level}.";
				return false;
			}
			if (ElevatorManager.TrySetDestination(group, level, force: true))
			{
				ServerLogs.AddLog(ServerLogs.Modules.Administrative, $"{sender.LogName} sent elevator {group} to level {level}.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
				response = $"Elevator \"{group}\" has been sent to level {level}.";
				return true;
			}
			response = $"Could not send elevator \"{group}\" to level {level}.";
			return false;
		}
	}
}
