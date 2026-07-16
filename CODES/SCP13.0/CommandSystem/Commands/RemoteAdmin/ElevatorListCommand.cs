using System;
using System.Linq;
using System.Text;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using NorthwoodLib.Pools;
using UnityEngine;

namespace CommandSystem.Commands.RemoteAdmin
{
	[CommandHandler(typeof(ElevatorCommand))]
	public class ElevatorListCommand : ICommand
	{
		public string Command { get; } = "list";

		public string[] Aliases { get; } = new string[6] { "ls", "lst", "elevators", "lifts", "els", "elevs" };

		public string Description { get; } = "Lists all elevators.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out response))
			{
				return false;
			}
			bool getLevels = arguments.Count > 0 && (arguments.At(0).Equals("detailed", StringComparison.OrdinalIgnoreCase) || arguments.At(0).Equals("d", StringComparison.OrdinalIgnoreCase) || arguments.At(0).Equals("det", StringComparison.OrdinalIgnoreCase));
			StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
			try
			{
				bool result = true;
				stringBuilder.Append("Detected the following elevators:");
				foreach (ElevatorManager.ElevatorGroup key in ElevatorDoor.AllElevatorDoors.Keys)
				{
					stringBuilder.Append("\n- ");
					if (!GetElevatorData(key, getLevels, stringBuilder))
					{
						result = false;
					}
				}
				response = stringBuilder.ToString();
				return result;
			}
			finally
			{
				StringBuilderPool.Shared.Return(stringBuilder);
			}
		}

		private static bool GetElevatorData(ElevatorManager.ElevatorGroup group, bool getLevels, StringBuilder sb)
		{
			if (!ElevatorDoor.AllElevatorDoors.TryGetValue(group, out var value))
			{
				sb.AppendFormat("Elevator \"{0}\" could not be found in the Facility.", group);
				return false;
			}
			sb.AppendFormat("Elevator \"{0}\" detected with {1} levels. Currently ", group, value.Count);
			ElevatorDoor elevatorDoor = value.FirstOrDefault((ElevatorDoor x) => x.TargetState);
			if (elevatorDoor == null)
			{
				sb.Append("in transit");
			}
			else
			{
				sb.AppendFormat("at level {0}", value.IndexOf(elevatorDoor));
			}
			ElevatorDoor elevatorDoor2 = value.FirstOrDefault();
			if (elevatorDoor2 != null)
			{
				if (elevatorDoor2.TargetPanel.AssignedChamber.ActiveLocks.HasFlagFast(DoorLockReason.AdminCommand))
				{
					sb.Append(" and administratively locked");
				}
				else if (elevatorDoor2.TargetPanel.AssignedChamber.ActiveLocks != DoorLockReason.None)
				{
					sb.Append(" and locked");
				}
			}
			else
			{
				sb.Append(" (lock status unknown)");
			}
			sb.Append(".");
			if (!getLevels)
			{
				return true;
			}
			for (int num = 0; num < value.Count; num++)
			{
				Vector3 position = value[num].transform.position;
				sb.AppendFormat("\n-   Level {0} at height {1}", num, Mathf.Round(position.y));
				if (RoomIdentifier.RoomsByCoordinates.TryGetValue(RoomIdUtils.PositionToCoords(position), out var value2))
				{
					sb.AppendFormat(" (room: \"{0}\")", value2.Name);
				}
			}
			sb.Append('\n');
			return true;
		}
	}
}
