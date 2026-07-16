using System.Collections.Generic;
using MapGeneration;
using PlayerRoles;
using UnityEngine;

namespace Respawning
{
	public static class CivilianMilestoneTokens
	{
		private const float MinAliveTime = 10f;

		private static readonly Dictionary<FacilityZone, float> RewardsPerZone;

		private static readonly Dictionary<HumanRole, HashSet<FacilityZone>> Tracker;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void OnRoleChanged(ReferenceHub hub, PlayerRoleBase prev, PlayerRoleBase cur)
		{
		}

		private static void UpdatePlayer(ReferenceHub hub)
		{
		}
	}
}
