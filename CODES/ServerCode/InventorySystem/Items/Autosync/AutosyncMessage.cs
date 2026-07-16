namespace InventorySystem.Items.Autosync
{
	public readonly struct AutosyncMessage : global::Mirror.NetworkMessage
	{
		private static readonly byte[] Buffer = new byte[65535];

		private static readonly global::Mirror.NetworkReader Reader = new global::Mirror.NetworkReader(Buffer);

		private readonly int _bytesWritten;

		private readonly ushort _serial;

		private readonly ItemType _itemType;

		public AutosyncMessage(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.ItemBase instance)
		{
			_serial = instance.ItemSerial;
			_itemType = instance.ItemTypeId;
			_bytesWritten = global::UnityEngine.Mathf.Min(writer.Length, 255);
			global::System.Array.Copy(writer.buffer, Buffer, _bytesWritten);
		}

		internal AutosyncMessage(global::Mirror.NetworkReader reader)
		{
			_serial = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			_itemType = (ItemType)reader.ReadByte();
			_bytesWritten = reader.ReadByte();
			reader.ReadBytes(Buffer, _bytesWritten);
		}

		internal void Serialize(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, _serial);
			writer.WriteByte((byte)_itemType);
			writer.WriteByte((byte)_bytesWritten);
			writer.WriteBytes(Buffer, 0, _bytesWritten);
		}

		internal void ProcessCmd(ReferenceHub sender)
		{
			if (sender.inventory.UserInventory.Items.TryGetValue(_serial, out var value) && value is global::InventorySystem.Items.Autosync.AutosyncItem autosyncItem && autosyncItem.ItemTypeId == _itemType)
			{
				ResetReader();
				autosyncItem.ServerProcessCmd(Reader);
			}
		}

		internal void ProcessRpc()
		{
			if (global::InventorySystem.InventoryItemLoader.TryGetItem<global::InventorySystem.Items.Autosync.AutosyncItem>(_itemType, out var result))
			{
				ResetReader();
				result.ClientProcessRpcTemplate(Reader, _serial);
			}
			if (ReferenceHub.TryGetLocalHub(out var hub) && hub.inventory.UserInventory.Items.TryGetValue(_serial, out var value) && value is global::InventorySystem.Items.Autosync.AutosyncItem autosyncItem && autosyncItem.ItemTypeId == _itemType)
			{
				ResetReader();
				autosyncItem.ClientProcessRpcLocally(Reader);
			}
		}

		private void ResetReader()
		{
			Reader.buffer = new global::System.ArraySegment<byte>(Buffer, 0, _bytesWritten);
			Reader.Position = 0;
		}
	}
}
