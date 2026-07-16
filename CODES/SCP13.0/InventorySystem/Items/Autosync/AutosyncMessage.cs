using System;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Autosync
{
    public readonly struct AutosyncMessage : NetworkMessage
    {
        private static readonly byte[] Buffer = new byte[65535];
        private static readonly NetworkReader PooledReader = new NetworkReader(new ArraySegment<byte>(Buffer));

        private readonly int _bytesWritten;
        private readonly ushort _serial;
        private readonly ItemType _itemType;

        public AutosyncMessage(NetworkWriter writer, ItemBase instance)
        {
            _serial = instance.ItemSerial;
            _itemType = instance.ItemTypeId;
            _bytesWritten = Mathf.Min(writer.Position, 255);

            ArraySegment<byte> segment = writer.ToArraySegment();
            if (segment.Array != null)
            {
                System.Buffer.BlockCopy(segment.Array, segment.Offset, Buffer, 0, _bytesWritten);
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.WriteUShort(_serial);
            writer.WriteByte((byte)_itemType);
            writer.WriteByte((byte)_bytesWritten);
            writer.WriteBytes(Buffer, 0, _bytesWritten);
        }

        internal AutosyncMessage(NetworkReader reader)
        {
            _serial = reader.ReadUShort();
            _itemType = (ItemType)reader.ReadByte();
            _bytesWritten = reader.ReadByte();
            reader.ReadBytes(Buffer, _bytesWritten);
        }

        internal void ProcessCmd(ReferenceHub sender)
        {
            if (sender.inventory.UserInventory.Items.TryGetValue(_serial, out var value) && value is AutosyncItem autosyncItem && autosyncItem.ItemTypeId == _itemType)
            {
                ResetReader();
                autosyncItem.ServerProcessCmd(PooledReader);
            }
        }

        internal void ProcessRpc()
        {
            if (InventoryItemLoader.TryGetItem<AutosyncItem>(_itemType, out var result))
            {
                ResetReader();
                result.ClientProcessRpcTemplate(PooledReader, _serial);
            }

            if (ReferenceHub.TryGetLocalHub(out var hub) && hub.inventory.UserInventory.Items.TryGetValue(_serial, out var value) && value is AutosyncItem autosyncItem && autosyncItem.ItemTypeId == _itemType)
            {
                ResetReader();
                autosyncItem.ClientProcessRpcLocally(PooledReader);
            }
        }

        private void ResetReader()
        {
            PooledReader.SetBuffer(new ArraySegment<byte>(Buffer, 0, _bytesWritten));
        }
    }
}