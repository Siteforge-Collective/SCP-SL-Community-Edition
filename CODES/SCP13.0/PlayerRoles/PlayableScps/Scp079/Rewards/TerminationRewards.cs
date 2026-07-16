using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public static class TerminationRewards
	{
		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void OnPlayerTeleported(ReferenceHub scp106, ReferenceHub hub)
		{
		}

		private static void OnHumanTerminated(ReferenceHub ply, DamageHandlerBase damageHandler)
		{
		}

		private static void GainReward(Scp079Role scp079, ReferenceHub deadPly, DamageHandlerBase damageHandler)
		{
		}

		private static Scp079HudTranslation EvaluateGainReason(Scp079Role scp079, ReferenceHub deadPlayer, DamageHandlerBase damageHandler)
		{
			return default(Scp079HudTranslation);
		}

		private static bool CheckDirectTermination(Scp079Role scp079, DamageHandlerBase damageHandler)
		{
			return false;
		}

		public static bool TryGetBaseReward(RoleTypeId rt, out int amount)
		{
			amount = default(int);
			return false;
		}
	}
}
