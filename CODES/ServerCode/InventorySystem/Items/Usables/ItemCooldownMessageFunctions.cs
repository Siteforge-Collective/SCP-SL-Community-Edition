namespace InventorySystem.Items.Usables
{
	public static class ItemCooldownMessageFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Usables.ItemCooldownMessage value)
		{
			value.Serialize(writer);
		}

		public static global::InventorySystem.Items.Usables.ItemCooldownMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Usables.ItemCooldownMessage(global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), global::Mirror.NetworkReaderExtensions.ReadSingle(reader));
		}
	}
}
