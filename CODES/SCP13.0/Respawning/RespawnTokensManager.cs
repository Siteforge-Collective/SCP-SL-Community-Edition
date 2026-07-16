using System;
using System.Collections.Generic;
using PlayerRoles;
using UnityEngine;

namespace Respawning
{
	public static class RespawnTokensManager
	{
		private class TokenCounter
		{
			private float _amount;

			public SpawnableTeamType Team;

			public SpawnableTeamHandlerBase Handler;

			public float Amount
			{
				get
				{
					return 0f;
				}
				set
				{
				}
			}

			public TokenCounter(SpawnableTeamType team, SpawnableTeamHandlerBase handler)
			{
			}
		}

		public static readonly HashSet<SpawnableTeamType> SupportedTeams;

		private static readonly bool DebugMode;

		private static readonly List<TokenCounter> Counters;

		private static int _teamsCount;

		private const float TotalTokens = 100f;

		private const float AccuracyTolerance = 0.5f;

		private const float OverallMultiplier = 3.5f;

		private static float TotalAssigned => 0f;

		public static SpawnableTeamType DominatingTeam => default(SpawnableTeamType);

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ForEachCounter(Action<TokenCounter> action)
		{
		}

		private static void UpdateAccuracy(bool force = false)
		{
		}

		public static void ResetTokens()
		{
		}

		public static void ModifyTokens(SpawnableTeamType team, float amount)
		{
		}

		public static void GrantTokens(SpawnableTeamType team, float tokens)
		{
		}

		public static void RemoveTokens(SpawnableTeamType team, float tokens)
		{
		}

		public static float GetTeamDominance(SpawnableTeamType team)
		{
			return 0f;
		}

		public static void ForceTeamDominance(SpawnableTeamType team, float val)
		{
		}

		public static bool TryGetAssignedSpawnableTeam(this Faction faction, out SpawnableTeamType stt)
		{
			stt = default(SpawnableTeamType);
			return false;
		}

		public static bool TryGetAssignedSpawnableTeam(this RoleTypeId role, out SpawnableTeamType stt)
		{
			stt = default(SpawnableTeamType);
			return false;
		}

		public static bool TryGetAssignedSpawnableTeam(this ReferenceHub ply, out SpawnableTeamType stt)
		{
			stt = default(SpawnableTeamType);
			return false;
		}
	}
}
