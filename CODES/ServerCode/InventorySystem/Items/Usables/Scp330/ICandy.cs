namespace InventorySystem.Items.Usables.Scp330
{
	public interface ICandy
	{
		global::InventorySystem.Items.Usables.Scp330.CandyKindID Kind { get; }

		float SpawnChanceWeight { get; }

		void ServerApplyEffects(ReferenceHub hub);
	}
}
