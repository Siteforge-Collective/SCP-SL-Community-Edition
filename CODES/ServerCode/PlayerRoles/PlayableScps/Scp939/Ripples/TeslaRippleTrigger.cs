namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class TeslaRippleTrigger : global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleTriggerBase
	{
		private const float CooldownDuration = 0.7f;

		private const float IdleRangeSqr = 120f;

		private const float BurstRangeSqr = 2400f;

		private static readonly global::UnityEngine.Vector3 PosOffset = global::UnityEngine.Vector3.up * 1.35f;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		public override void SpawnObject()
		{
			base.SpawnObject();
			TeslaGate.OnBursted += OnTeslaBursted;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_cooldown.Clear();
			TeslaGate.OnBursted -= OnTeslaBursted;
		}

		private void OnTeslaBursted(TeslaGate tg)
		{
			if (base.IsLocalOrSpectated)
			{
				PlayInRange(tg.transform.position + PosOffset, 2400f, global::UnityEngine.Color.red);
			}
		}

		private void Update()
		{
			if (!base.IsLocalOrSpectated || !_cooldown.IsReady)
			{
				return;
			}
			_cooldown.Trigger(0.7f);
			foreach (TeslaGate teslaGate in TeslaGateController.Singleton.TeslaGates)
			{
				if (teslaGate.isIdling)
				{
					PlayInRange(teslaGate.transform.position + PosOffset, 120f, global::UnityEngine.Color.red);
				}
			}
		}
	}
}
