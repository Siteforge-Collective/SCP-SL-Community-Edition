namespace InventorySystem.Disarming
{
	public static class DisarmMessageSerializers
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Disarming.DisarmMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, (!value.PlayerIsNull) ? value.PlayerToDisarm.networkIdentity.netId : 0u);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, value.Disarm);
		}

		public static global::InventorySystem.Disarming.DisarmMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			uint netId = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
			bool disarm = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			ReferenceHub hub;
			bool isNull = !ReferenceHub.TryGetHubNetID(netId, out hub);
			return new global::InventorySystem.Disarming.DisarmMessage(hub, disarm, isNull);
		}
	}
}
