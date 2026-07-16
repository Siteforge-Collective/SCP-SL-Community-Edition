namespace MapGeneration
{
	public class RoomIdentifier : global::UnityEngine.MonoBehaviour
	{
		public global::MapGeneration.RoomShape Shape;

		public global::MapGeneration.RoomName Name;

		public global::MapGeneration.FacilityZone Zone;

		public global::UnityEngine.Vector3Int[] AdditionalZones;

		public global::UnityEngine.Bounds[] SubBounds;

		public global::UnityEngine.Sprite Icon;

		public static readonly global::System.Collections.Generic.HashSet<global::MapGeneration.RoomIdentifier> AllRoomIdentifiers = new global::System.Collections.Generic.HashSet<global::MapGeneration.RoomIdentifier>();

		public static readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.Vector3Int, global::MapGeneration.RoomIdentifier> RoomsByCoordinates = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Vector3Int, global::MapGeneration.RoomIdentifier>();

		public static readonly global::UnityEngine.Vector3 GridScale = new global::UnityEngine.Vector3(15f, 100f, 15f);

		public global::UnityEngine.Vector3Int[] OccupiedCoords { get; private set; }

		public global::PluginAPI.Core.Zones.FacilityRoom ApiRoom { get; private set; }

		public static event global::System.Action<global::MapGeneration.RoomIdentifier> OnAdded;

		public static event global::System.Action<global::MapGeneration.RoomIdentifier> OnRemoved;

		private void Awake()
		{
			AllRoomIdentifiers.Add(this);
			global::MapGeneration.RoomIdentifier.OnAdded?.Invoke(this);
			ApiRoom = global::PluginAPI.Core.Facility.GetRoom(this);
		}

		private void OnDestroy()
		{
			AllRoomIdentifiers.Remove(this);
			global::MapGeneration.RoomIdentifier.OnRemoved?.Invoke(this);
			if (OccupiedCoords != null)
			{
				global::UnityEngine.Vector3Int[] occupiedCoords = OccupiedCoords;
				foreach (global::UnityEngine.Vector3Int key in occupiedCoords)
				{
					RoomsByCoordinates.Remove(key);
				}
			}
		}

		public bool TryAssignId()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return false;
			}
			global::UnityEngine.Vector3Int vector3Int = global::MapGeneration.RoomIdUtils.PositionToCoords(base.transform.position);
			RoomsByCoordinates[vector3Int] = this;
			OccupiedCoords = new global::UnityEngine.Vector3Int[AdditionalZones.Length + 1];
			OccupiedCoords[0] = vector3Int;
			int num = 1;
			global::UnityEngine.Vector3Int[] additionalZones = AdditionalZones;
			for (int i = 0; i < additionalZones.Length; i++)
			{
				global::UnityEngine.Vector3 direction = global::UnityEngine.Vector3.Scale(additionalZones[i], GridScale);
				vector3Int = global::MapGeneration.RoomIdUtils.PositionToCoords(base.transform.position + base.transform.TransformDirection(direction));
				RoomsByCoordinates[vector3Int] = this;
				OccupiedCoords[num] = vector3Int;
				num++;
			}
			return true;
		}

		public bool TryGetMainCoords(out global::UnityEngine.Vector3Int coords)
		{
			if (OccupiedCoords == null || OccupiedCoords.Length == 0)
			{
				coords = global::UnityEngine.Vector3Int.zero;
				return false;
			}
			coords = OccupiedCoords[0];
			return true;
		}
	}
}
