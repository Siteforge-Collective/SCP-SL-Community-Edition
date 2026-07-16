public static class AmmoLimitSerializer
{
	public static void WriteAmmoLimit(this global::Mirror.NetworkWriter writer, ServerConfigSynchronizer.AmmoLimit value)
	{
		writer.WriteByte((byte)value.AmmoType);
		global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, value.Limit);
	}

	public static ServerConfigSynchronizer.AmmoLimit ReadAmmoLimit(this global::Mirror.NetworkReader reader)
	{
		return new ServerConfigSynchronizer.AmmoLimit
		{
			AmmoType = (ItemType)reader.ReadByte(),
			Limit = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader)
		};
	}
}
