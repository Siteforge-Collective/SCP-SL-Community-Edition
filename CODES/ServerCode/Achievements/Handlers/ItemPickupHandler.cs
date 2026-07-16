namespace Achievements.Handlers
{
	public class ItemPickupHandler : global::Achievements.AchievementHandlerBase
	{
		internal override void OnInitialize()
		{
			global::InventorySystem.InventoryExtensions.OnItemAdded += OnItemAdded;
		}

		private static void OnItemAdded(ReferenceHub ply, global::InventorySystem.Items.ItemBase ib, global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (ib.ItemTypeId == ItemType.KeycardO5)
				{
					global::Achievements.AchievementHandlerBase.ServerAchieve(ply.connectionToClient, global::Achievements.AchievementName.Power);
				}
				else if (global::PlayerRoles.PlayerRolesUtils.GetRoleId(ply) == global::PlayerRoles.RoleTypeId.ClassD && ib is global::InventorySystem.Items.Firearms.Firearm)
				{
					global::Achievements.AchievementHandlerBase.ServerAchieve(ply.connectionToClient, global::Achievements.AchievementName.ThatCanBeUseful);
				}
			}
		}
	}
}
