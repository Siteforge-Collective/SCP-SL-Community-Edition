namespace MapGeneration
{
	public class DoorSpawnpoint : global::UnityEngine.MonoBehaviour
	{
		private const float MinimumDistance = 2.5f;

		private static readonly global::System.Collections.Generic.Queue<global::MapGeneration.DoorSpawnpoint> AllInstances = new global::System.Collections.Generic.Queue<global::MapGeneration.DoorSpawnpoint>();

		public global::Interactables.Interobjects.DoorUtils.DoorVariant TargetPrefab;

		public string DesiredNametag;

		private void Start()
		{
			AllInstances.Enqueue(this);
		}

		public static void SetupAllDoors()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			global::System.Collections.Generic.HashSet<global::MapGeneration.DoorSpawnpoint> hashSet = new global::System.Collections.Generic.HashSet<global::MapGeneration.DoorSpawnpoint>();
			while (AllInstances.Count > 0)
			{
				global::MapGeneration.DoorSpawnpoint doorSpawnpoint = AllInstances.Dequeue();
				if (doorSpawnpoint == null || !doorSpawnpoint.gameObject.activeInHierarchy)
				{
					continue;
				}
				global::UnityEngine.Vector3 position = doorSpawnpoint.transform.position;
				bool flag = false;
				foreach (global::MapGeneration.DoorSpawnpoint item in hashSet)
				{
					global::UnityEngine.Vector3 position2 = item.transform.position;
					if (!(global::UnityEngine.Mathf.Abs(position2.y - position.y) > 2.5f) && !(global::UnityEngine.Mathf.Abs(position2.x - position.x) > 2.5f) && !(global::UnityEngine.Mathf.Abs(position2.z - position.z) > 2.5f))
					{
						flag = true;
						item.transform.position = global::UnityEngine.Vector3.Lerp(position2, position, 0.5f);
						if (!string.IsNullOrEmpty(doorSpawnpoint.DesiredNametag))
						{
							item.DesiredNametag = doorSpawnpoint.DesiredNametag;
						}
						break;
					}
				}
				if (!flag)
				{
					hashSet.Add(doorSpawnpoint);
				}
			}
			foreach (global::MapGeneration.DoorSpawnpoint item2 in hashSet)
			{
				global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant = global::UnityEngine.Object.Instantiate(item2.TargetPrefab, item2.transform.position, item2.transform.rotation);
				global::PluginAPI.Core.Facility.RegisterDoor(item2, doorVariant);
				if (!string.IsNullOrEmpty(item2.DesiredNametag))
				{
					doorVariant.gameObject.AddComponent<global::Interactables.Interobjects.DoorUtils.DoorNametagExtension>().UpdateName(item2.DesiredNametag);
				}
				global::Mirror.NetworkServer.Spawn(doorVariant.gameObject);
			}
		}
	}
}
