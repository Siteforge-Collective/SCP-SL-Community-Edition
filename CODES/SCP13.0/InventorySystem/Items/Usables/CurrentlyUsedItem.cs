namespace InventorySystem.Items.Usables
{
	public readonly struct CurrentlyUsedItem
	{
		public static CurrentlyUsedItem None;

		public readonly UsableItem Item;

		public readonly ushort ItemSerial;

		public readonly float StartTime;

		public CurrentlyUsedItem(UsableItem item, ushort serial, float startTime)
		{
			Item = null;
			ItemSerial = 0;
			StartTime = 0f;
		}
	}
}
