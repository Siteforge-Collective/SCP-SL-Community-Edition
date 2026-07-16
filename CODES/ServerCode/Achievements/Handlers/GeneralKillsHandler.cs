namespace Achievements.Handlers
{
	public class GeneralKillsHandler : global::Achievements.AchievementHandlerBase
	{
		private const int WowReallyAchievementTime = 60;

		internal override void OnInitialize()
		{
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied += HandleDeath;
		}

		private static void HandleDeath(ReferenceHub deadPlayer, global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			global::PlayerRoles.RoleTypeId roleId = global::PlayerRoles.PlayerRolesUtils.GetRoleId(deadPlayer);
			if (global::GameCore.RoundStart.RoundStartTimer.Elapsed.TotalSeconds <= 60.0)
			{
				Send(deadPlayer, global::Achievements.AchievementName.WowReally);
			}
			if (handler is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler && attackerDamageHandler.Attacker.Hub != null)
			{
				ReferenceHub hub = attackerDamageHandler.Attacker.Hub;
				if (global::PlayerRoles.PlayerRolesUtils.IsSCP(hub) && deadPlayer.inventory.CurInstance is global::InventorySystem.Items.MicroHID.MicroHIDItem microHIDItem && (microHIDItem.State == global::InventorySystem.Items.MicroHID.HidState.PoweringUp || microHIDItem.State == global::InventorySystem.Items.MicroHID.HidState.Firing))
				{
					Send(hub, global::Achievements.AchievementName.IllPassThanks);
				}
				if (attackerDamageHandler.Attacker.Role == global::PlayerRoles.RoleTypeId.ClassD && roleId == global::PlayerRoles.RoleTypeId.Scientist && deadPlayer.inventory.CurInstance is global::InventorySystem.Items.Keycards.KeycardItem)
				{
					Send(hub, global::Achievements.AchievementName.Betrayal);
				}
				else if (attackerDamageHandler.Attacker.Role == global::PlayerRoles.RoleTypeId.Scientist)
				{
					if (roleId == global::PlayerRoles.RoleTypeId.ClassD)
					{
						Send(hub, global::Achievements.AchievementName.JustResources);
					}
					else if (global::PlayerRoles.PlayerRolesUtils.IsSCP(deadPlayer))
					{
						Send(hub, global::Achievements.AchievementName.SomethingDoneRight);
					}
				}
			}
			if (global::PlayerRoles.PlayerRolesUtils.GetRoleId(deadPlayer) == global::PlayerRoles.RoleTypeId.Scp096 && global::PlayerRoles.PlayableScps.Scp096.Scp096StateExtensions.IsRageState(deadPlayer.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp096.Scp096Role, global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Distressed))
			{
				Send(deadPlayer, global::Achievements.AchievementName.InvoluntaryRageQuit);
			}
			else if (handler is global::PlayerStatsSystem.UniversalDamageHandler universalDamageHandler)
			{
				if (universalDamageHandler.TranslationId == global::PlayerStatsSystem.DeathTranslations.PocketDecay.Id)
				{
					Send(deadPlayer, global::Achievements.AchievementName.Newb);
				}
				if (universalDamageHandler.TranslationId == global::PlayerStatsSystem.DeathTranslations.Falldown.Id)
				{
					Send(deadPlayer, global::Achievements.AchievementName.Gravity);
				}
				if (universalDamageHandler.TranslationId == global::PlayerStatsSystem.DeathTranslations.Tesla.Id)
				{
					if (deadPlayer.inventory.CurInstance is global::InventorySystem.Items.MicroHID.MicroHIDItem)
					{
						Send(deadPlayer, global::Achievements.AchievementName.Overcurrent);
					}
					Send(deadPlayer, global::Achievements.AchievementName.DeepFried);
				}
			}
			else if (handler is global::PlayerStatsSystem.ScpDamageHandler scpDamageHandler)
			{
				if (scpDamageHandler.Attacker.Role == global::PlayerRoles.RoleTypeId.Scp173)
				{
					Send(deadPlayer, global::Achievements.AchievementName.FirstTime);
				}
			}
			else if (handler is global::PlayerStatsSystem.MicroHidDamageHandler microHidDamageHandler)
			{
				if (global::PlayerRoles.PlayerRolesUtils.IsSCP(deadPlayer))
				{
					Send(microHidDamageHandler.Attacker.Hub, global::Achievements.AchievementName.MicrowaveMeal);
				}
			}
			else if (handler is global::PlayerStatsSystem.RecontainmentDamageHandler recontainmentDamageHandler)
			{
				if (roleId == global::PlayerRoles.RoleTypeId.Scp106)
				{
					Send(recontainmentDamageHandler.Attacker.Hub, global::Achievements.AchievementName.SecureContainProtect);
				}
			}
			else if (handler is global::PlayerStatsSystem.ExplosionDamageHandler explosionDamageHandler && explosionDamageHandler.Attacker.Hub == deadPlayer)
			{
				Send(deadPlayer, global::Achievements.AchievementName.Rocket);
			}
		}

		private static void Send(ReferenceHub hub, global::Achievements.AchievementName name)
		{
			if (hub != null)
			{
				global::Achievements.AchievementHandlerBase.ServerAchieve(hub.networkIdentity.connectionToClient, name);
			}
		}
	}
}
