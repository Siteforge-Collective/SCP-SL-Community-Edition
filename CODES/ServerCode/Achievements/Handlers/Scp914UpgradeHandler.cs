namespace Achievements.Handlers
{
	public class Scp914UpgradeHandler : global::Achievements.AchievementHandlerBase
	{
		private static bool AnyDClassIn914
		{
			get
			{
				foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
				{
					if (global::PlayerRoles.PlayerRolesUtils.GetRoleId(allHub) == global::PlayerRoles.RoleTypeId.ClassD)
					{
						global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPosition(allHub.transform.position);
						if (roomIdentifier != null && roomIdentifier.Name == global::MapGeneration.RoomName.Lcz914)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		internal override void OnInitialize()
		{
			global::Scp914.Scp914Upgrader.OnInventoryItemUpgraded = (global::System.Action<global::InventorySystem.Items.ItemBase, global::Scp914.Scp914KnobSetting>)global::System.Delegate.Combine(global::Scp914.Scp914Upgrader.OnInventoryItemUpgraded, new global::System.Action<global::InventorySystem.Items.ItemBase, global::Scp914.Scp914KnobSetting>(ItemUpgraded));
			global::Scp914.Scp914Upgrader.OnPickupUpgraded = (global::System.Action<global::InventorySystem.Items.Pickups.ItemPickupBase, global::Scp914.Scp914KnobSetting>)global::System.Delegate.Combine(global::Scp914.Scp914Upgrader.OnPickupUpgraded, new global::System.Action<global::InventorySystem.Items.Pickups.ItemPickupBase, global::Scp914.Scp914KnobSetting>(PickupUpgraded));
		}

		private static void ItemUpgraded(global::InventorySystem.Items.ItemBase item, global::Scp914.Scp914KnobSetting sett)
		{
			if (global::PlayerRoles.PlayerRolesUtils.GetRoleId(item.Owner) == global::PlayerRoles.RoleTypeId.Scientist && item is global::InventorySystem.Items.Keycards.KeycardItem && AnyDClassIn914)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(item.OwnerInventory.connectionToClient, global::Achievements.AchievementName.Friendship);
			}
		}

		private static void PickupUpgraded(global::InventorySystem.Items.Pickups.ItemPickupBase ipb, global::Scp914.Scp914KnobSetting sett)
		{
			if (ipb is global::InventorySystem.Items.Keycards.KeycardPickup && ipb.PreviousOwner.Role == global::PlayerRoles.RoleTypeId.Scientist && AnyDClassIn914 && ipb.PreviousOwner.Hub != null)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(ipb.PreviousOwner.Hub.networkIdentity.connectionToClient, global::Achievements.AchievementName.Friendship);
			}
		}
	}
}
