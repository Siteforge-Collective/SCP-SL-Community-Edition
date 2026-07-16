using System.Collections.Generic;
using InventorySystem.Items;

namespace InventorySystem
{
	public class InventoryInfo
	{
		public Dictionary<ushort, ItemBase> Items = new();

		public Dictionary<ItemType, ushort> ReserveAmmo = new();
	}
}
