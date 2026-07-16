namespace InventorySystem
{
	public class InventoryInfo
	{
		public global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.ItemBase> Items;

		public global::System.Collections.Generic.Dictionary<ItemType, ushort> ReserveAmmo;

		public InventoryInfo()
		{
			Items = new global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.ItemBase>();
			ReserveAmmo = new global::System.Collections.Generic.Dictionary<ItemType, ushort>();
		}
	}
}
