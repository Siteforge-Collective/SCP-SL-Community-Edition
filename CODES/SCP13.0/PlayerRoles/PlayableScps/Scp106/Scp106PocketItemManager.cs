using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using Mirror;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public static class Scp106PocketItemManager
	{
		private class PocketItem
		{
			public double TriggerTime;

			public bool Remove;

			public bool WarningSent;

			public RelativePosition DropPosition;
		}

		public struct WarningMessage : NetworkMessage
		{
			public RelativePosition Position;
		}

		private const float WarningTime = 3f;

		private const float RaycastRange = 30f;

		private const float SoundRange = 12f;

		private const float SpawnOffset = 0.3f;

		private const float RandomEscapeVelocity = 0.2f;

		private const int MaxValidPositions = 64;

		private static readonly Vector3[] ValidPositionsNonAlloc;

		private static readonly HashSet<ItemPickupBase> ToRemove;

		private static readonly Dictionary<ItemPickupBase, PocketItem> TrackedItems;

		private static readonly Vector2 HeightLimit;

		private static readonly Vector2 TimerRage;

		private static readonly float[] RecycleChances;

		private static float RandomVel => 0f;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void Update()
		{
		}

		private static void OnAdded(ItemPickupBase ipb)
		{
		}

		private static void OnRemoved(ItemPickupBase ipb)
		{
		}

		private static bool ValidateHeight(ItemPickupBase ipb)
		{
			return false;
		}

		private static int GetRarity(ItemBase ib)
		{
			return 0;
		}

		private static RelativePosition GetRandomValidSpawnPosition()
		{
			return default(RelativePosition);
		}

		private static bool TryGetRoofPosition(Vector3 point, out Vector3 result)
		{
			result = default(Vector3);
			return false;
		}

		private static bool HasFlagFast(ItemBase ib, ItemTierFlags flag)
		{
			return false;
		}
	}
}
