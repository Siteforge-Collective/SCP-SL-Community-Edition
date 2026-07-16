namespace InventorySystem.Searching
{
	public struct SearchInvalidation : global::InventorySystem.Searching.ISearchIdentifiable, global::Mirror.NetworkMessage, global::System.IEquatable<global::InventorySystem.Searching.SearchInvalidation>
	{
		public byte Id { get; private set; }

		public SearchInvalidation(byte id)
		{
			Id = id;
		}

		public void Deserialize(global::Mirror.NetworkReader reader)
		{
			Id = reader.ReadByte();
		}

		public void Serialize(global::Mirror.NetworkWriter writer)
		{
			writer.WriteByte(Id);
		}

		public bool Equals(global::InventorySystem.Searching.SearchInvalidation other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj is global::InventorySystem.Searching.SearchInvalidation other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
