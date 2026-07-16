namespace InventorySystem.Items.Usables.Scp330
{
	public class CandyGreen : ICandy
	{
		private const float RegenerationDuration = 80f;

		private const float RegenerationPerSecond = 1.5f;

		private const int VitalityDuration = 30;

		private const bool VitalityDurationStacking = true;

		public CandyKindID Kind => default(CandyKindID);

		public float SpawnChanceWeight => 0f;

		public void ServerApplyEffects(ReferenceHub hub)
		{
		}
	}
}
