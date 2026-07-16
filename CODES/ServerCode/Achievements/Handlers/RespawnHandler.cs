namespace Achievements.Handlers
{
	public class RespawnHandler : global::Achievements.AchievementHandlerBase
	{
		internal override void OnInitialize()
		{
			global::PlayerRoles.PlayerRoleManager.OnServerRoleSet += OnRespawned;
		}

		private static void OnRespawned(ReferenceHub userHub, global::PlayerRoles.RoleTypeId newRole, global::PlayerRoles.RoleChangeReason reason)
		{
			if (reason == global::PlayerRoles.RoleChangeReason.Respawn && global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayerRoleBase>(newRole, out var result))
			{
				global::Mirror.NetworkConnection connectionToClient = userHub.connectionToClient;
				switch (result.Team)
				{
				case global::PlayerRoles.Team.FoundationForces:
					global::Achievements.AchievementHandlerBase.ServerAchieve(connectionToClient, global::Achievements.AchievementName.GearUp);
					break;
				case global::PlayerRoles.Team.ChaosInsurgency:
					global::Achievements.AchievementHandlerBase.ServerAchieve(connectionToClient, global::Achievements.AchievementName.Chaos);
					break;
				}
			}
		}
	}
}
