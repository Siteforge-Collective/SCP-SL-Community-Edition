namespace InventorySystem.Items.Pickups
{
	public static class PickupSyncInfoSerializer
	{
		public static void WritePickupSyncInfo(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Pickups.PickupSyncInfo value)
		{
			global::Mirror.NetworkWriterExtensions.WriteSByte(writer, (sbyte)value.ItemId);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, value.Serial);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, value.Weight);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, value.RelativePosition);
			writer.WriteLowPrecisionQuaternion(value.RelativeRotation);
			writer.WriteByte(value.SyncedFlags);
		}

		public static global::InventorySystem.Items.Pickups.PickupSyncInfo ReadPickupSyncInfo(this global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Pickups.PickupSyncInfo
			{
				ItemId = (ItemType)global::Mirror.NetworkReaderExtensions.ReadSByte(reader),
				Serial = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader),
				Weight = global::Mirror.NetworkReaderExtensions.ReadSingle(reader),
				RelativePosition = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader),
				RelativeRotation = reader.ReadLowPrecisionQuaternion(),
				SyncedFlags = reader.ReadByte()
			};
		}
	}
}
