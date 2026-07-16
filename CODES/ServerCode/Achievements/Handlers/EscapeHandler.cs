namespace Achievements.Handlers
{
	public class EscapeHandler : global::Achievements.AchievementHandlerBase
	{
		internal override void OnInitialize()
		{
			Escape.OnServerPlayerEscape += OnEscaped;
			global::PlayerRoles.PlayerRoleManager.OnServerRoleSet += OnRoleSet;
		}

		private static void OnRoleSet(ReferenceHub userHub, global::PlayerRoles.RoleTypeId newId, global::PlayerRoles.RoleChangeReason reason)
		{
			if (global::Mirror.NetworkServer.active && reason == global::PlayerRoles.RoleChangeReason.Escaped && global::GameCore.RoundStart.RoundStartTimer.Elapsed.TotalSeconds <= 180.0)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(userHub.networkIdentity.connectionToClient, global::Achievements.AchievementName.EscapeArtist);
			}
		}

		private static void OnEscaped(ReferenceHub userHub)
		{
			if (global::InventorySystem.Disarming.DisarmedPlayers.IsDisarmed(userHub.inventory))
			{
				return;
			}
			global::Mirror.NetworkConnectionToClient connectionToClient = userHub.networkIdentity.connectionToClient;
			global::PlayerRoles.PlayerRoleBase currentRole = userHub.roleManager.CurrentRole;
			if (userHub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Scp207>().IsEnabled)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(connectionToClient, global::Achievements.AchievementName.Escape207);
			}
			if (currentRole.RoleTypeId == global::PlayerRoles.RoleTypeId.ClassD)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(connectionToClient, global::Achievements.AchievementName.ItsAlwaysLeft);
				int num = 0;
				foreach (global::InventorySystem.Items.ItemBase value in userHub.inventory.UserInventory.Items.Values)
				{
					if (value.Category == ItemCategory.SCPItem)
					{
						num++;
					}
				}
				if (num >= 2)
				{
					global::Achievements.AchievementHandlerBase.ServerAchieve(connectionToClient, global::Achievements.AchievementName.PropertyOfChaos);
				}
			}
			else if (currentRole.RoleTypeId == global::PlayerRoles.RoleTypeId.Scientist)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(connectionToClient, global::Achievements.AchievementName.ForScience);
			}
		}
	}
}
