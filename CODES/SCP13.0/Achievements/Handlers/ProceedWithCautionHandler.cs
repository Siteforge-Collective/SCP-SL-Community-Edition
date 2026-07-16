namespace Achievements.Handlers
{
	public class ProceedWithCautionHandler : AchievementHandlerBase
	{
		private const float IgnoreTeslaDistance = 5f;

		private bool _wasInRange;

		internal override void OnInitialize()
		{
		}

		private void Update()
		{
		}

		private void OnBurstComplete(ReferenceHub hub, TeslaGate teslagate)
		{
		}
	}
}
