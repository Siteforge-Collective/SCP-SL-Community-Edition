using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public static class HidStoppedReward
	{
		private const int Reward = 50;

		private const float MinReadiness = 0.75f;

		private const float TimeTolerance = 10f;

		private const float ScpMinProximitySqr = 600f;

		private static bool _available;

		private static double _microDamageCooldown;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void TryGrant(ReferenceHub ply)
		{
		}

		private static bool IsNearbyTeammate(Vector3 attackerPos, ReferenceHub teammate)
		{
			return false;
		}
	}
}
