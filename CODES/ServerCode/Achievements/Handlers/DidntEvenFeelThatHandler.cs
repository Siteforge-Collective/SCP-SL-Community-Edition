namespace Achievements.Handlers
{
	public class DidntEvenFeelThatHandler : global::Achievements.AchievementHandlerBase
	{
		internal override void OnInitialize()
		{
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDamaged += AnyDamage;
		}

		private static void AnyDamage(ReferenceHub ply, global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			if (handler is global::PlayerStatsSystem.StandardDamageHandler standardDamageHandler && global::PlayerRoles.PlayerRolesUtils.IsHuman(ply))
			{
				global::PlayerStatsSystem.HealthStat module = ply.playerStats.GetModule<global::PlayerStatsSystem.HealthStat>();
				if (module.CurValue > 0f && module.CurValue - standardDamageHandler.AbsorbedAhpDamage <= 0f)
				{
					global::Achievements.AchievementHandlerBase.ServerAchieve(ply.networkIdentity.connectionToClient, global::Achievements.AchievementName.DidntEvenFeelThat);
				}
			}
		}
	}
}
