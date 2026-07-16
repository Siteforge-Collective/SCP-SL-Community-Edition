namespace InventorySystem.Items.Firearms.Attachments
{
	public struct AttachmentsChangeRequest : global::Mirror.NetworkMessage
	{
		public ushort WeaponSerial;

		public uint AttachmentsCode;

		public void Deserialize(global::Mirror.NetworkReader reader)
		{
			WeaponSerial = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			AttachmentsCode = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
		}

		public void Serialize(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, WeaponSerial);
			global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, AttachmentsCode);
		}
	}
}
