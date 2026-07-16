using System.Collections.Generic;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.HumeShield;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieShieldController : DynamicHumeShieldController
	{
		public const float MaxShield = 100f;

		public const float MaxActivateDistanceSqr = 100f;

		private static readonly HashSet<Scp049CallAbility> CallSubroutines;

		private FirstPersonMovementModule _fpc;

		public override float HsMax => 0f;

		public override float HsRegeneration => 0f;

		public override void SpawnObject()
		{
		}

		private bool CheckDistanceTo(Scp049Role role)
		{
			return false;
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static bool TryGetCallSubroutine(PlayerRoleBase prb, out Scp049CallAbility sr)
		{
			sr = null;
			return false;
		}
	}
}
