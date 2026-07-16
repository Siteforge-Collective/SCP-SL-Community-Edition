namespace InventorySystem.Items
{
	[global::System.Flags]
	public enum BlockedInteraction : byte
	{
		GeneralInteractions = 1,
		OpenInventory = 2,
		BeDisarmed = 4,
		GrabItems = 8,
		All = 0xF
	}
}
