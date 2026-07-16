namespace InventorySystem.Disarming
{
	public static class DisarmedPlayersListMessageSerializers
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Disarming.DisarmedPlayersListMessage value)
		{
			writer.WriteByte((byte)value.Entries.Count);
			foreach (global::InventorySystem.Disarming.DisarmedPlayers.DisarmedEntry entry in value.Entries)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, entry.DisarmedPlayer);
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, entry.Disarmer);
			}
		}

		public static global::InventorySystem.Disarming.DisarmedPlayersListMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			global::System.Collections.Generic.List<global::InventorySystem.Disarming.DisarmedPlayers.DisarmedEntry> list = new global::System.Collections.Generic.List<global::InventorySystem.Disarming.DisarmedPlayers.DisarmedEntry>();
			int num = reader.ReadByte();
			for (int i = 0; i < num; i++)
			{
				uint disarmedPlayer = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
				uint disarmer = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
				list.Add(new global::InventorySystem.Disarming.DisarmedPlayers.DisarmedEntry(disarmedPlayer, disarmer));
			}
			return new global::InventorySystem.Disarming.DisarmedPlayersListMessage(list);
		}
	}
}
