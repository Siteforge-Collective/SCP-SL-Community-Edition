namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public static class TerminationRewards
	{
		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied += delegate(ReferenceHub ply, global::PlayerStatsSystem.DamageHandlerBase handler)
			{
				if ((handler is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler && attackerDamageHandler.Attacker.Role.GetTeam() == global::PlayerRoles.Team.SCPs) || (handler is global::PlayerStatsSystem.UniversalDamageHandler universalDamageHandler && universalDamageHandler.TranslationId == global::PlayerStatsSystem.DeathTranslations.Tesla.Id))
				{
					OnHumanTerminated(ply);
				}
			};
			global::PlayerRoles.PlayableScps.Scp106.Scp106Attack.OnPlayerTeleported += OnHumanTerminated;
		}

		private static void OnHumanTerminated(ReferenceHub ply)
		{
			if (!global::Mirror.NetworkServer.active || !(ply.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole) || !TryGetReward(humanRole.RoleTypeId, out var gainReason, out var amount))
			{
				return;
			}
			global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(humanRole.FpcModule.Position);
			if (roomIdentifier == null || global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager.GrantExpForRoom(roomIdentifier, amount, gainReason))
			{
				return;
			}
			foreach (global::PlayerRoles.PlayableScps.Scp079.Scp079Role activeInstance in global::PlayerRoles.PlayableScps.Scp079.Scp079Role.ActiveInstances)
			{
				if (!(activeInstance.CurrentCamera.Room != roomIdentifier))
				{
					global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager.GrantExp(activeInstance, amount / 2, global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainWitnessingTermination);
				}
			}
		}

		public static bool TryGetReward(global::PlayerRoles.RoleTypeId rt, out global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation gainReason, out int amount)
		{
			switch (rt.GetTeam())
			{
			case global::PlayerRoles.Team.ChaosInsurgency:
				gainReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationChaos;
				amount = 30;
				return true;
			case global::PlayerRoles.Team.ClassD:
				gainReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationClassD;
				amount = 40;
				return true;
			case global::PlayerRoles.Team.Scientists:
				gainReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationScientist;
				amount = 50;
				return true;
			case global::PlayerRoles.Team.OtherAlive:
				gainReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationOther;
				amount = 30;
				return true;
			case global::PlayerRoles.Team.FoundationForces:
				if (rt == global::PlayerRoles.RoleTypeId.FacilityGuard)
				{
					amount = 25;
					gainReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationGuard;
				}
				else
				{
					amount = 30;
					gainReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationNtf;
				}
				return true;
			default:
				amount = 0;
				gainReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
				return false;
			}
		}
	}
}
