namespace InventorySystem.Items.Usables
{
	public struct ItemCooldownMessage : global::Mirror.NetworkMessage
	{
		public ushort ItemSerial;

		public float RemainingTime;

		public ItemCooldownMessage(ushort serial, float remainingTime)
		{
			ItemSerial = serial;
			RemainingTime = remainingTime;
		}

		public void Serialize(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, ItemSerial);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, RemainingTime);
		}
	}
}
