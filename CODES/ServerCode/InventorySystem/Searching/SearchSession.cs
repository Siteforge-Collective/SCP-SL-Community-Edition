namespace InventorySystem.Searching
{
	public struct SearchSession : global::InventorySystem.Searching.ISearchSession, global::Mirror.NetworkMessage, global::System.IEquatable<global::InventorySystem.Searching.SearchSession>
	{
		public global::InventorySystem.Items.Pickups.ItemPickupBase Target { get; set; }

		public double InitialTime { get; set; }

		public double FinishTime { get; set; }

		public double Duration => FinishTime - InitialTime;

		public double Progress => global::Utils.MoreMath.InverseLerp(InitialTime, FinishTime, global::Mirror.NetworkTime.time);

		public SearchSession(double initialTime, double finishTime, global::InventorySystem.Items.Pickups.ItemPickupBase target)
		{
			Target = target;
			InitialTime = initialTime;
			FinishTime = finishTime;
		}

		public void Deserialize(global::Mirror.NetworkReader reader)
		{
			global::Mirror.NetworkIdentity networkIdentity = global::Mirror.NetworkReaderExtensions.ReadNetworkIdentity(reader);
			Target = ((networkIdentity == null) ? null : networkIdentity.GetComponent<global::InventorySystem.Items.Pickups.ItemPickupBase>());
			InitialTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
			FinishTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		}

		public void Serialize(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteNetworkIdentity(writer, (Target == null) ? null : Target.netIdentity);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, InitialTime);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, FinishTime);
		}

		public bool Equals(global::InventorySystem.Searching.SearchSession other)
		{
			if (object.Equals(Target, other.Target) && InitialTime.Equals(other.InitialTime))
			{
				return FinishTime.Equals(other.FinishTime);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is global::InventorySystem.Searching.SearchSession other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((((Target != null) ? Target.GetHashCode() : 0) * 397) ^ InitialTime.GetHashCode()) * 397) ^ FinishTime.GetHashCode();
		}
	}
}
