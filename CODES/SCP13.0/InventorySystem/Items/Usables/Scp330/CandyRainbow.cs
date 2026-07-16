using PlayerStatsSystem;

namespace InventorySystem.Items.Usables.Scp330
{
	public class CandyRainbow : ICandy
	{
		private const int HealthInstant = 15;

		private const int InvigorationDuration = 5;

		private const bool InvigorationDurationStacking = true;

		private const int AhpInstant = 20;

		private const int AhpSustainDuration = 10;

		private const bool AhpSustainDurationStacking = false;

		private const int RainbowDuration = 10;

		private const bool RainbowDurationStacking = false;

		private const bool BodyshotReductionStacking = true;

		private AhpStat.AhpProcess _previousProcess;

		public CandyKindID Kind => default(CandyKindID);

		public float SpawnChanceWeight => 0f;

		public void ServerApplyEffects(ReferenceHub hub)
		{
		}
	}
}
