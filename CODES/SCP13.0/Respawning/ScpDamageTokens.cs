using System.Collections.Generic;
using PlayerRoles;
using PlayerStatsSystem;
using UnityEngine;

namespace Respawning
{
	public static class ScpDamageTokens
	{
		private static readonly Dictionary<uint, Dictionary<Faction, float>> DamageContributions;

		private const float HsPointDamageReward = 0.0005f;

		private const float FullHealthbarDamageReward = 4f;

		private const float TerminationReward = 3f;

		private const float MicroKillBonus = 1.5f;

		private const Faction DefaultRewardReceiver = Faction.FoundationStaff;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void OnAnyPlayerDied(ReferenceHub hub, DamageHandlerBase dhb)
		{
		}

		private static void OnAnyPlayerDamaged(ReferenceHub hub, DamageHandlerBase dhb)
		{
		}

		private static void RewardFaction(Faction faction, float totalTokens)
		{
		}

		private static void HandleDamageTickets(ReferenceHub scp, AttackerDamageHandler adh)
		{
		}

		private static void RegisterContribution(ReferenceHub scp, Faction faction, StandardDamageHandler handler)
		{
		}
	}
}
