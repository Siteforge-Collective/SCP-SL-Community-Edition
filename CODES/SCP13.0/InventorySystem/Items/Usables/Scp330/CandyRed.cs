namespace InventorySystem.Items.Usables.Scp330
{
	public class CandyRed : ICandy
	{
		private const float RegenerationDuration = 5f;

		private const float RegenerationPerSecond = 9f;

		public CandyKindID Kind => default(CandyKindID);

		public float SpawnChanceWeight => 0f;

		public void ServerApplyEffects(ReferenceHub hub)
		{
		}
	}
}
