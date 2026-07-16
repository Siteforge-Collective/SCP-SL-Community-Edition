namespace InventorySystem.Items
{
	[global::System.Serializable]
	public readonly struct ItemIdentifier : global::System.IEquatable<global::InventorySystem.Items.ItemIdentifier>
	{
		public static global::InventorySystem.Items.ItemIdentifier None = new global::InventorySystem.Items.ItemIdentifier(ItemType.None, 0);

		public readonly ItemType TypeId;

		public readonly ushort SerialNumber;

		public ItemIdentifier(ItemType t, ushort s)
		{
			TypeId = t;
			SerialNumber = s;
		}

		public override int GetHashCode()
		{
			return SerialNumber;
		}

		public static bool operator ==(global::InventorySystem.Items.ItemIdentifier left, global::InventorySystem.Items.ItemIdentifier right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(global::InventorySystem.Items.ItemIdentifier left, global::InventorySystem.Items.ItemIdentifier right)
		{
			return !left.Equals(right);
		}

		public bool Equals(global::InventorySystem.Items.ItemIdentifier other)
		{
			if (SerialNumber == other.SerialNumber)
			{
				return TypeId == other.TypeId;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is global::InventorySystem.Items.ItemIdentifier other)
			{
				return Equals(other);
			}
			return false;
		}
	}
}
