using System.Collections.Generic;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public static class Scp3114Spawner
	{
		private const float SpawnChance = 0f;

		private const int MinHumans = 2;

		private static readonly List<ReferenceHub> SpawnCandidates;

		private static bool _ragdollsSpawned;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void OnServerRoleSet(ReferenceHub userHub, RoleTypeId newRole, RoleChangeReason reason)
		{
		}

		private static void OnPlayersSpawned()
		{
		}

		private static void SpawnRagdolls(string ragdollDisplayNickname)
		{
		}

		private static void SpawnRagdoll(RoleTypeId role, Vector3 pos, Quaternion rot, string nickname)
		{
		}
	}
}
