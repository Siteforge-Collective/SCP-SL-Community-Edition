namespace MapGeneration
{
	public static class RoomIdUtils
	{
		public static global::UnityEngine.Vector3Int PositionToCoords(global::UnityEngine.Vector3 position)
		{
			global::UnityEngine.Vector3Int zero = global::UnityEngine.Vector3Int.zero;
			for (int i = 0; i < 3; i++)
			{
				zero[i] = global::UnityEngine.Mathf.RoundToInt(position[i] / global::MapGeneration.RoomIdentifier.GridScale[i]);
			}
			return zero;
		}

		public static bool TryFindRoom(global::MapGeneration.RoomName name, global::MapGeneration.FacilityZone zone, global::MapGeneration.RoomShape shape, out global::MapGeneration.RoomIdentifier foundRoom)
		{
			foreach (global::MapGeneration.RoomIdentifier allRoomIdentifier in global::MapGeneration.RoomIdentifier.AllRoomIdentifiers)
			{
				if ((name == global::MapGeneration.RoomName.Unnamed || name == allRoomIdentifier.Name) && (zone == global::MapGeneration.FacilityZone.None || zone == allRoomIdentifier.Zone) && (shape == global::MapGeneration.RoomShape.Undefined || shape == allRoomIdentifier.Shape))
				{
					foundRoom = allRoomIdentifier;
					return true;
				}
			}
			foundRoom = null;
			return false;
		}

		public static global::System.Collections.Generic.HashSet<global::MapGeneration.RoomIdentifier> FindRooms(global::MapGeneration.RoomName name, global::MapGeneration.FacilityZone zone, global::MapGeneration.RoomShape shape)
		{
			global::System.Collections.Generic.HashSet<global::MapGeneration.RoomIdentifier> hashSet = new global::System.Collections.Generic.HashSet<global::MapGeneration.RoomIdentifier>();
			foreach (global::MapGeneration.RoomIdentifier allRoomIdentifier in global::MapGeneration.RoomIdentifier.AllRoomIdentifiers)
			{
				if ((name == global::MapGeneration.RoomName.Unnamed || name == allRoomIdentifier.Name) && (zone == global::MapGeneration.FacilityZone.None || zone == allRoomIdentifier.Zone) && (shape == global::MapGeneration.RoomShape.Undefined || shape == allRoomIdentifier.Shape))
				{
					hashSet.Add(allRoomIdentifier);
				}
			}
			return hashSet;
		}

		public static bool IsTheSameRoom(global::UnityEngine.Vector3 pos1, global::UnityEngine.Vector3 pos2)
		{
			return RoomAtPosition(pos1) == RoomAtPosition(pos2);
		}

		public static bool IsWithinRoomBoundaries(global::MapGeneration.RoomIdentifier room, global::UnityEngine.Vector3 pos, float extension = 0f, bool accurateMode = false)
		{
			if (accurateMode)
			{
				if (RoomAtPositionRaycasts(pos) == room)
				{
					return true;
				}
				if (extension == 0f)
				{
					return false;
				}
				pos += (room.transform.position - pos).normalized * extension;
				if (RoomAtPositionRaycasts(pos) == room)
				{
					return true;
				}
			}
			if (extension != 0f)
			{
				pos += (room.transform.position - pos).normalized * extension;
			}
			return RoomAtPosition(pos) == room;
		}

		public static global::MapGeneration.RoomIdentifier RoomAtPosition(global::UnityEngine.Vector3 position)
		{
			if (global::MapGeneration.RoomIdentifier.RoomsByCoordinates.TryGetValue(PositionToCoords(position), out var value))
			{
				return value;
			}
			return null;
		}

		public static global::MapGeneration.RoomIdentifier RoomAtPositionRaycasts(global::UnityEngine.Vector3 position, bool prioritizeRaycast = true)
		{
			if (!prioritizeRaycast && global::MapGeneration.RoomIdentifier.RoomsByCoordinates.TryGetValue(PositionToCoords(position), out var value) && value != null)
			{
				return value;
			}
			if (TryCastRayToFindRoom(new global::UnityEngine.Ray(position, global::UnityEngine.Vector3.up), 8f, out value) || TryCastRayToFindRoom(new global::UnityEngine.Ray(position, global::UnityEngine.Vector3.down), 8f, out value))
			{
				return value;
			}
			if (prioritizeRaycast)
			{
				return RoomAtPosition(position);
			}
			return null;
		}

		private static bool TryCastRayToFindRoom(global::UnityEngine.Ray ray, float distance, out global::MapGeneration.RoomIdentifier room)
		{
			room = null;
			try
			{
				if (global::UnityEngine.Physics.Raycast(ray, out var hitInfo, distance, 1))
				{
					room = hitInfo.collider.GetComponentInParent<global::MapGeneration.RoomIdentifier>();
				}
				return room != null;
			}
			catch
			{
				return false;
			}
		}
	}
}
