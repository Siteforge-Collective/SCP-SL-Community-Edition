namespace InventorySystem.Items
{
	public interface IInteractionBlocker
	{
		global::InventorySystem.Items.BlockedInteraction BlockedInteractions { get; }

		bool CanBeCleared { get; }
	}
}
