namespace InventorySystem
{
	public readonly struct InventoryRoleInfo
	{
		public readonly ItemType[] Items;

		public readonly global::System.Collections.Generic.Dictionary<ItemType, ushort> Ammo;

		public InventoryRoleInfo(ItemType[] items, global::System.Collections.Generic.Dictionary<ItemType, ushort> ammo)
		{
			Items = items;
			Ammo = ammo;
		}
	}
}
