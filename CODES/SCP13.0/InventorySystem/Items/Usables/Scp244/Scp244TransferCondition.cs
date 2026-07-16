using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244
{
	public class Scp244TransferCondition
	{
		private static readonly Vector3 DoorDetectionThickness;

		private const float MinimalDoorGapSqrt = 9f;

		private const float BorderDoorCheck = 1.05f;

		public readonly Bounds BoundsToEncapsulate;

		public readonly DoorVariant[] Doors;

		public readonly float ClosestPoint;

		private Scp244TransferCondition(Bounds b, DoorVariant[] dv, Scp244DeployablePickup scp)
		{
		}

		public static Scp244TransferCondition[] GenerateTransferConditions(Scp244DeployablePickup scp244)
		{
			return null;
		}

		private static Bounds GetBoundsOfEntireRoom(RoomIdentifier rid)
		{
			return default(Bounds);
		}

		private static void HandleRoomBounds(ref List<Bounds> extraBounds, RoomIdentifier rid)
		{
		}

		private static void AddNearbyRooms(ref List<Vector3Int> nearbyRooms, Vector3Int startCoords, RoomIdentifier startRoom)
		{
		}

		private static void AddDoorsFromRoom(ref HashSet<DoorVariant> doors, Bounds bounds, Vector3 room, List<Bounds> allBounds)
		{
		}

		private static Bounds GetRelativeBounds(Transform relativeTo, Bounds refBounds)
		{
			return default(Bounds);
		}
	}
}
