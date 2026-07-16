namespace InventorySystem.Items.Usables.Scp330
{
	public class CandyYellow : ICandy
	{
		private const int InstantStamina = 25;

		private const int InvigorationDuration = 8;

		private const bool InvigorationDurationStacking = true;

		private const int BoostDuration = 8;

		private const bool BoostDurationStacking = true;

		private const int BoostIntensity = 10;

		private const bool BoostIntensityStacking = true;

		public CandyKindID Kind => default(CandyKindID);

		public float SpawnChanceWeight => 0f;

		public void ServerApplyEffects(ReferenceHub hub)
		{
		}
	}
}
