namespace MapGeneration.Distributors
{
	[global::UnityEngine.CreateAssetMenu(fileName = "New Spawner Settings Preset", menuName = "ScriptableObject/Map Generation/Spawnable Elements Settings")]
	public class SpawnablesDistributorSettings : global::UnityEngine.ScriptableObject
	{
		[global::UnityEngine.Range(0.05f, 5f)]
		public float SpawnerDelay;

		[global::UnityEngine.Range(0.05f, 5f)]
		public float UnfreezeDelay;

		public global::MapGeneration.Distributors.SpawnableItem[] SpawnableItems;

		public global::MapGeneration.Distributors.SpawnableStructure[] SpawnableStructures;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientStarted += RegisterSpawnables;
		}

		private static void RegisterSpawnables()
		{
			global::MapGeneration.Distributors.SpawnablesDistributorSettings[] array = global::UnityEngine.Resources.LoadAll<global::MapGeneration.Distributors.SpawnablesDistributorSettings>(string.Empty);
			global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.SpawnableStructure> hashSet = global::NorthwoodLib.Pools.HashSetPool<global::MapGeneration.Distributors.SpawnableStructure>.Shared.Rent();
			global::MapGeneration.Distributors.SpawnablesDistributorSettings[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				global::MapGeneration.Distributors.SpawnableStructure[] spawnableStructures = array2[i].SpawnableStructures;
				foreach (global::MapGeneration.Distributors.SpawnableStructure spawnableStructure in spawnableStructures)
				{
					if (hashSet.Add(spawnableStructure))
					{
						global::Mirror.NetworkClient.RegisterPrefab(spawnableStructure.gameObject);
					}
				}
			}
			global::NorthwoodLib.Pools.HashSetPool<global::MapGeneration.Distributors.SpawnableStructure>.Shared.Return(hashSet);
		}
	}
}
