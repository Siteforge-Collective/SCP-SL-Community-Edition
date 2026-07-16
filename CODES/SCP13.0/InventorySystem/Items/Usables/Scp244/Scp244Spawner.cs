using System.Collections.Generic;
using MapGeneration;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244
{
	public static class Scp244Spawner
	{
		private static readonly List<RoomIdentifier> CompatibleRooms;

		private const int Amount = 1;

		private const float SpawnChance = 0.35f;

		private static readonly Dictionary<RoomName, Vector3> NameToPos;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void SpawnAllInstances()
		{
		}

		private static void SpawnScp244(ItemBase ib)
		{
		}
	}
}
