namespace InventorySystem.Items.Usables.Scp330
{
	public class CandyPurple : ICandy
	{
		private const int ReductionDuration = 15;

		private const int ReductionIntensity = 40;

		private const bool ReductionStacking = true;

		private const float RegenerationDuration = 10f;

		private const float RegenerationPerSecond = 1.5f;

		public CandyKindID Kind => default(CandyKindID);

		public float SpawnChanceWeight => 0f;

		public void ServerApplyEffects(ReferenceHub hub)
		{
		}
	}
}
