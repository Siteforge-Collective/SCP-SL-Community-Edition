namespace MapGeneration.Distributors
{
	public class StructureDistributor : global::MapGeneration.Distributors.SpawnablesDistributorBase
	{
		private readonly global::System.Collections.Generic.Queue<int> _queuedStructures = new global::System.Collections.Generic.Queue<int>();

		private readonly global::System.Collections.Generic.HashSet<int> _missingSpawnpoints = new global::System.Collections.Generic.HashSet<int>();

		protected override void PlaceSpawnables()
		{
			PrepareQueueForStructures();
			int structureId;
			global::MapGeneration.Distributors.StructureSpawnpoint spawnpoint;
			while (TryGetNextStructure(out structureId, out spawnpoint))
			{
				if (!(spawnpoint == null))
				{
					SpawnStructure(Settings.SpawnableStructures[structureId], spawnpoint.transform, spawnpoint.TriggerDoorName);
				}
			}
		}

		private void PrepareQueueForStructures()
		{
			global::System.Collections.Generic.List<int> list = global::NorthwoodLib.Pools.ListPool<int>.Shared.Rent();
			for (int i = 0; i < Settings.SpawnableStructures.Length; i++)
			{
				int num = global::UnityEngine.Random.Range(Settings.SpawnableStructures[i].MinAmount, Settings.SpawnableStructures[i].MaxAmount + 1);
				for (int j = 0; j < num; j++)
				{
					if (j < Settings.SpawnableStructures[i].MinAmount)
					{
						_queuedStructures.Enqueue(i);
					}
					else
					{
						list.Add(i);
					}
				}
			}
			while (list.Count > 0)
			{
				int index = global::UnityEngine.Random.Range(0, list.Count);
				_queuedStructures.Enqueue(list[index]);
				list.RemoveAt(index);
			}
			global::NorthwoodLib.Pools.ListPool<int>.Shared.Return(list);
		}

		private bool TryGetNextStructure(out int structureId, out global::MapGeneration.Distributors.StructureSpawnpoint spawnpoint)
		{
			spawnpoint = null;
			if (!CollectionExtensions.TryDequeue(_queuedStructures, out structureId))
			{
				return false;
			}
			if (_missingSpawnpoints.Contains(structureId))
			{
				return true;
			}
			global::System.Collections.Generic.List<global::MapGeneration.Distributors.StructureSpawnpoint> list = global::NorthwoodLib.Pools.ListPool<global::MapGeneration.Distributors.StructureSpawnpoint>.Shared.Rent();
			foreach (global::MapGeneration.Distributors.StructureSpawnpoint availableInstance in global::MapGeneration.Distributors.StructureSpawnpoint.AvailableInstances)
			{
				if (!(availableInstance == null) && availableInstance.CompatibleStructures.Contains(Settings.SpawnableStructures[structureId].StructureType))
				{
					list.Add(availableInstance);
				}
			}
			if (list.Count > 0)
			{
				global::MapGeneration.Distributors.StructureSpawnpoint structureSpawnpoint = list[global::UnityEngine.Random.Range(0, list.Count)];
				global::MapGeneration.Distributors.StructureSpawnpoint.AvailableInstances.Remove(structureSpawnpoint);
				spawnpoint = structureSpawnpoint;
			}
			else
			{
				_missingSpawnpoints.Add(structureId);
			}
			return true;
		}

		private void SpawnStructure(global::MapGeneration.Distributors.SpawnableStructure structure, global::UnityEngine.Transform tr, string doorName)
		{
			global::MapGeneration.Distributors.SpawnableStructure spawnableStructure = global::UnityEngine.Object.Instantiate(structure, tr.position, tr.rotation);
			spawnableStructure.transform.SetParent(tr);
			if (string.IsNullOrEmpty(doorName) || !global::Interactables.Interobjects.DoorUtils.DoorNametagExtension.NamedDoors.TryGetValue(doorName, out var value))
			{
				SpawnObject(spawnableStructure.gameObject);
			}
			else
			{
				RegisterUnspawnedObject(value.TargetDoor, spawnableStructure.gameObject);
			}
		}
	}
}
