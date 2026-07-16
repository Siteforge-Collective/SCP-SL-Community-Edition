namespace Achievements.Handlers
{
	public class TurnThemAllHandler : global::Achievements.AchievementHandlerBase
	{
		private const int TargetCured = 10;

		private static int _healedPlayers;

		private static bool _alreadyAchieved;

		internal override void OnInitialize()
		{
			global::PlayerRoles.PlayerRoleManager.OnServerRoleSet += OnRoleChanged;
		}

		internal override void OnRoundStarted()
		{
			_healedPlayers = 0;
			_alreadyAchieved = false;
		}

		private static void OnRoleChanged(ReferenceHub userHub, global::PlayerRoles.RoleTypeId newClass, global::PlayerRoles.RoleChangeReason reason)
		{
			if (!global::Mirror.NetworkServer.active || _alreadyAchieved || newClass != global::PlayerRoles.RoleTypeId.Scp0492 || reason != global::PlayerRoles.RoleChangeReason.Revived)
			{
				return;
			}
			_healedPlayers++;
			if (_healedPlayers < 10)
			{
				return;
			}
			global::Mirror.NetworkConnection conn = null;
			bool flag = false;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (global::PlayerRoles.PlayerRolesUtils.GetRoleId(allHub) == global::PlayerRoles.RoleTypeId.Scp049)
				{
					if (flag)
					{
						return;
					}
					conn = allHub.networkIdentity.connectionToClient;
					flag = true;
				}
			}
			if (flag)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(conn, global::Achievements.AchievementName.TurnThemAll);
			}
		}
	}
}
