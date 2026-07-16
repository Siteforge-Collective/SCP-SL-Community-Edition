using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace Respawning
{
	public static class ItemPickupTokens
	{
		private static bool _hidAlreadyPickedUp;

		private const float MicroPickupReward = 1f;

		private const float WeaponHeldReward = 0.4f;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static bool TryGetSpawnableTeam(ReferenceHub hub, out SpawnableTeamType stt)
		{
			stt = default(SpawnableTeamType);
			return false;
		}

		private static bool IsCivilian(ReferenceHub hub)
		{
			return false;
		}

		private static void OnItemAdded(ReferenceHub hub, ItemBase ib, ItemPickupBase ipb)
		{
		}

		private static void OnItemRemoved(ReferenceHub hub, ItemBase ib, ItemPickupBase ipb)
		{
		}
	}
}
