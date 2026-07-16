namespace InventorySystem.Items.Firearms.Attachments.Components
{
	[global::System.Serializable]
	public struct ReflexSightSyncMessage : global::Mirror.NetworkMessage
	{
		public readonly ushort WeaponSerial;

		public readonly int AttachmentId;

		public readonly int TextureId;

		public readonly int ColorId;

		public readonly int SizeId;

		public ReflexSightSyncMessage(global::Mirror.NetworkReader reader)
		{
			WeaponSerial = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			ushort num = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			AttachmentId = (num & 0xF000) >> 12;
			TextureId = (num & 0xF00) >> 8;
			ColorId = (num & 0xF0) >> 4;
			SizeId = num & 0xF;
		}

		public void Write(global::Mirror.NetworkWriter writer)
		{
			int num = AttachmentId << 12;
			int num2 = TextureId << 8;
			int num3 = ColorId << 4;
			int sizeId = SizeId;
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, WeaponSerial);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, (ushort)(num + num2 + num3 + sizeId));
		}

		public override string ToString()
		{
			return $"Texture={TextureId} Color={ColorId} Size={SizeId}";
		}
	}
}
