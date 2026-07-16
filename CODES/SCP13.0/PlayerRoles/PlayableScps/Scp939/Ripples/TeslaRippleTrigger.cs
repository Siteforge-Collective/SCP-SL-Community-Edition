using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class TeslaRippleTrigger : RippleTriggerBase
	{
		private const float CooldownDuration = 0.7f;

		private const float IdleRangeSqr = 120f;

		private const float BurstRangeSqr = 2400f;

		private static readonly Vector3 PosOffset;

		private readonly AbilityCooldown _cooldown;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void OnTeslaBursted(TeslaGate tg)
		{
		}

		private void Update()
		{
		}
	}
}
