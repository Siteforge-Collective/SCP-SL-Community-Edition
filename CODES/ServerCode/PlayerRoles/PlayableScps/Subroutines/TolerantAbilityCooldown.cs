namespace PlayerRoles.PlayableScps.Subroutines
{
	public class TolerantAbilityCooldown : global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown
	{
		private readonly double _tolerance;

		public bool TolerantIsReady => global::Mirror.NetworkTime.time >= base.NextUse - _tolerance;

		public TolerantAbilityCooldown(float tolerance = 0.2f)
		{
			_tolerance = tolerance;
		}

		public override void Trigger(float cooldown)
		{
			if (IsReady)
			{
				base.Trigger(cooldown);
			}
			else
			{
				base.NextUse += cooldown;
			}
		}
	}
}
