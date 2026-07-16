namespace InventorySystem.Items.Usables.Scp330
{
	public class CandyPink : ICandy
	{
		public CandyKindID Kind => default(CandyKindID);

		public float SpawnChanceWeight => 0f;

		public void ServerApplyEffects(ReferenceHub hub)
		{
		}
	}
}
