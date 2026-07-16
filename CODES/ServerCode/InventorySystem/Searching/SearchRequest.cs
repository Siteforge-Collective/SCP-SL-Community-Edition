namespace InventorySystem.Searching
{
	public struct SearchRequest : global::InventorySystem.Searching.ISearchSession, global::Mirror.NetworkMessage, global::System.IEquatable<global::InventorySystem.Searching.SearchRequest>
	{
		private global::InventorySystem.Searching.SearchSession _body;

		public byte Id { get; private set; }

		public global::InventorySystem.Searching.SearchSession Body => _body;

		public global::InventorySystem.Items.Pickups.ItemPickupBase Target
		{
			get
			{
				return _body.Target;
			}
			set
			{
				_body.Target = value;
			}
		}

		public double InitialTime
		{
			get
			{
				return _body.InitialTime;
			}
			set
			{
				_body.InitialTime = value;
			}
		}

		public double FinishTime
		{
			get
			{
				return _body.FinishTime;
			}
			set
			{
				_body.FinishTime = value;
			}
		}

		public double Progress => _body.Progress;

		public void Deserialize(global::Mirror.NetworkReader reader)
		{
			Id = reader.ReadByte();
			_body.Deserialize(reader);
		}

		public void Serialize(global::Mirror.NetworkWriter writer)
		{
			writer.WriteByte(Id);
			_body.Serialize(writer);
		}

		public bool Equals(global::InventorySystem.Searching.SearchRequest other)
		{
			if (Body.Equals(other.Body))
			{
				return Id == other.Id;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is global::InventorySystem.Searching.SearchRequest other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (Body.GetHashCode() * 397) ^ Id.GetHashCode();
		}
	}
}
