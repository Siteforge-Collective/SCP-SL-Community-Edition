namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class SpawnableRipplesTrigger : global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleTriggerBase
	{
		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.PlayableScps.Scp939.Ripples.SpawnableRipple.OnSpawned += OnSpawned;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::PlayerRoles.PlayableScps.Scp939.Ripples.SpawnableRipple.OnSpawned -= OnSpawned;
		}

		private void OnSpawned(global::PlayerRoles.PlayableScps.Scp939.Ripples.SpawnableRipple sr)
		{
			if (base.IsLocalOrSpectated)
			{
				PlayInRange(sr.transform.position, sr.Range, global::UnityEngine.Color.red);
			}
		}
	}
}
