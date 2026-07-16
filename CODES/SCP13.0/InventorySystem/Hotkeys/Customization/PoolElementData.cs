using System;
using Mirror;
using UnityEngine;

namespace InventorySystem.Hotkeys.Customization
{
    [Serializable]
    public struct PoolElementData : IEquatable<PoolElementData>
    {
        public enum ElementType
        {
            SpecificItem = 0,
            Group = 1,
            Order = 2
        }

        [SerializeField]
        private short _value;

        public ElementType Type;

        public ItemType SpecificItem
        {
            get
            {
                EnsureType(ElementType.SpecificItem);
                return (ItemType)_value;
            }
        }

        public HotkeyItemGroup Group
        {
            get
            {
                EnsureType(ElementType.Group);
                return (HotkeyItemGroup)_value;
            }
        }

        public int Order
        {
            get
            {
                EnsureType(ElementType.Order);
                return _value;
            }
        }

        private void EnsureType(ElementType expected)
        {
            if (expected == Type)
                return;

            string message = string.Format(
                "Invalid PoolElementData access: expected type {0} but was {1}.",
                expected, Type);
            throw new InvalidCastException(message);
        }

        public bool Equals(PoolElementData other)
        {
            if (Type != other.Type)
                return false;

            return _value == other._value;
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.WriteByte((byte)Type);
            writer.WriteShort(_value);
        }

        public PoolElementData(NetworkReader reader)
        {
            Type = (ElementType)reader.ReadByte();
            _value = reader.ReadShort();
        }

        public PoolElementData(HotkeyItemGroup group)
        {
            Type = ElementType.Group;
            _value = (short)group;
        }

        public PoolElementData(ItemType item)
        {
            Type = ElementType.SpecificItem;
            _value = (short)item;
        }

        public PoolElementData(int order)
        {
            Type = ElementType.Order;
            _value = (short)order;
        }
    }
}