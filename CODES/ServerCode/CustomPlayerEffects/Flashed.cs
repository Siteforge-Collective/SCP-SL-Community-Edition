namespace CustomPlayerEffects
{
	public class Flashed : global::CustomPlayerEffects.StatusEffectBase
	{
		protected override void IntensityChanged(byte prevState, byte newState)
		{
			float timeLeft = (float)(int)newState * 0.1f;
			if (global::Mirror.NetworkServer.active)
			{
				base.TimeLeft = timeLeft;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active && base.Duration == 0f)
			{
				base.TimeLeft = 1f;
			}
		}
	}
}
