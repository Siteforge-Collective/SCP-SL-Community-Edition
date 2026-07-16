using System;
using System.Collections.Generic;
using System.Text;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using NorthwoodLib.Pools;

namespace CommandSystem.Commands.RemoteAdmin
{
	[CommandHandler(typeof(ElevatorCommand))]
	public class ElevatorLockCommand : ICommand
	{
		public string Command { get; } = "lock";

		public string[] Aliases { get; } = new string[2] { "l", "lck" };

		public string Description { get; } = "Locks an elevator.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out response))
			{
				return false;
			}
			if (arguments.Count == 1)
			{
				return TrySetLock(arguments.At(0), locked: true, out response, sender);
			}
			response = "Syntax error: elevator lock <Elevator ID / all>";
			return false;
		}

		internal static bool TrySetLock(string elevatorId, bool locked, out string response, ICommandSender sender)
		{
			if (!elevatorId.Equals("all", StringComparison.OrdinalIgnoreCase) && !elevatorId.Equals("*", StringComparison.OrdinalIgnoreCase))
			{
				if (!ElevatorCommand.TryParseGroup(elevatorId, out var group))
				{
					response = "Elevator \"" + elevatorId + "\" not found.";
					return false;
				}
				if (!ElevatorDoor.AllElevatorDoors.TryGetValue(group, out var value))
				{
					response = $"Elevator \"{group}\" could not be found in the Facility.";
					return false;
				}
				if (!SetLock(value, locked, sender, group.ToString()))
				{
					response = $"Could not update lock status for elevator \"{group}\".";
					return false;
				}
				response = string.Format("Elevator \"{0}\" has been {1}.", group, locked ? "locked" : "unlocked");
				return true;
			}
			StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
			bool result = true;
			try
			{
				foreach (KeyValuePair<ElevatorManager.ElevatorGroup, List<ElevatorDoor>> allElevatorDoor in ElevatorDoor.AllElevatorDoors)
				{
					if (SetLock(allElevatorDoor.Value, locked, sender, allElevatorDoor.Key.ToString()))
					{
						stringBuilder.AppendFormat("Elevator \"{0}\" has been {1}.\n", allElevatorDoor.Key, locked ? "locked" : "unlocked");
						continue;
					}
					result = false;
					stringBuilder.AppendFormat("Could not update lock status for elevator \"{0}\".\n", allElevatorDoor.Key);
				}
				response = stringBuilder.ToString();
				return result;
			}
			finally
			{
				StringBuilderPool.Shared.Return(stringBuilder);
			}
		}

		private static bool SetLock(List<ElevatorDoor> list, bool locked, ICommandSender sender, string elevatorName)
		{
			if (list.Count == 0 || list[0].TargetPanel == null)
			{
				return false;
			}
			if (list[0].TargetPanel.AssignedChamber.ActiveLocks.HasFlagFast(DoorLockReason.AdminCommand) == locked)
			{
				return true;
			}
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, sender.LogName + " " + (locked ? "locked" : "unlocked") + " elevator " + elevatorName + ".", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			foreach (ElevatorDoor item in list)
			{
				if (locked)
				{
					item.ActiveLocks = (ushort)(item.ActiveLocks | 8);
				}
				else
				{
					item.ActiveLocks = (ushort)(item.ActiveLocks & 0xFFF7);
				}
			}
			return true;
		}
	}
}
