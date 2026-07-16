using System.Collections.Generic;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.Usables;
using PlayerRoles;
using UnityEngine;

namespace Respawning
{
	public static class ScpItemsTokens
	{
		private const float HighReward = 1f;

		private const float MidReward = 0.7f;

		private const float LowReward = 0.4f;

		private const float CandyReward = 0.1f;

		private static readonly Dictionary<ItemType, float> ItemUseRewards;

		private static readonly HashSet<ushort> AlreadyUsedItems;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ServerOnUsingCompleted(ReferenceHub ply, UsableItem ui)
		{
		}

		private static void OnThrown(ThrownProjectile projectile)
		{
		}

		private static void GrantTokens(ushort serial, ItemType itemType, RoleTypeId role)
		{
		}
	}
}
