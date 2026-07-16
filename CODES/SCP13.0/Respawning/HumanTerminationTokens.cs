using System.Collections.Generic;
using PlayerRoles;
using PlayerStatsSystem;
using UnityEngine;

namespace Respawning
{
	public static class HumanTerminationTokens
	{
		private const float DClassKilledMilitantReward = 2f;

		private const float KilledMilitantReward = 1.5f;

		private const float MilitantDiedPenalty = 1.2f;

		private const float KilledScientistReward = 1.4f;

		private const float GuardKilledArmedDClassReward = 0.5f;

		private static readonly Dictionary<SpawnableTeamType, int> NumberOfRespawns;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void OnPlayerDied(ReferenceHub ply, DamageHandlerBase handler)
		{
		}

		private static void HandleHomocide(ReferenceHub deadPly, AttackerDamageHandler adh)
		{
		}

		private static void HandleOtherMilitantDeath(ReferenceHub deadPly)
		{
		}

		private static void HandleFoundationForcesHomocide(Team attackerTeam, ReferenceHub deadPly)
		{
		}

		private static bool IsArmedCategory(ItemCategory category)
		{
			return false;
		}
	}
}
