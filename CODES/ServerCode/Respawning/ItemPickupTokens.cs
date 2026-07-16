namespace Respawning
{
	public static class ItemPickupTokens
	{
		private static bool _hidAlreadyPickedUp;

		private const float MicroPickupReward = 1f;

		private const float WeaponHeldReward = 0.4f;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.InventoryExtensions.OnItemAdded += OnItemAdded;
			global::InventorySystem.InventoryExtensions.OnItemRemoved += OnItemRemoved;
			CustomNetworkManager.OnClientReady += delegate
			{
				_hidAlreadyPickedUp = false;
			};
		}

		private static bool TryGetSpawnableTeam(ReferenceHub hub, out global::Respawning.SpawnableTeamType stt)
		{
			if (global::Mirror.NetworkServer.active && hub.TryGetAssignedSpawnableTeam(out stt))
			{
				return true;
			}
			stt = global::Respawning.SpawnableTeamType.None;
			return false;
		}

		private static bool IsCivilian(ReferenceHub hub)
		{
			if (hub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole)
			{
				return humanRole.AssignedSpawnableTeam == global::Respawning.SpawnableTeamType.None;
			}
			return false;
		}

		private static void OnItemAdded(ReferenceHub hub, global::InventorySystem.Items.ItemBase ib, global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			if (!TryGetSpawnableTeam(hub, out var stt))
			{
				return;
			}
			switch ((byte)ib.Category)
			{
			case 7:
				if (!_hidAlreadyPickedUp)
				{
					_hidAlreadyPickedUp = true;
					global::Respawning.RespawnTokensManager.GrantTokens(stt, 1f);
				}
				break;
			case 4:
				if (IsCivilian(hub))
				{
					global::Respawning.RespawnTokensManager.GrantTokens(stt, 0.4f);
				}
				break;
			}
		}

		private static void OnItemRemoved(ReferenceHub hub, global::InventorySystem.Items.ItemBase ib, global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			if (ib.Category == ItemCategory.Firearm && IsCivilian(hub) && TryGetSpawnableTeam(hub, out var stt))
			{
				global::Respawning.RespawnTokensManager.RemoveTokens(stt, 0.4f);
			}
		}
	}
}
