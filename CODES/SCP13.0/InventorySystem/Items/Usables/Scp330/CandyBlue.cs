namespace InventorySystem.Items.Usables.Scp330
{
	public class CandyBlue : ICandy
	{
		private const int AhpInstant = 30;

		private const float AhpDecay = 0f;

		public CandyKindID Kind => default(CandyKindID);

		public float SpawnChanceWeight => 0f;

		public void ServerApplyEffects(ReferenceHub hub)
		{
		}
	}
}
