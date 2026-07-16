namespace Utils.Networking
{
	public static class NullableBoolReaderWriter
	{
		private enum NullableBoolValue : byte
		{
			Null = 0,
			True = 1,
			False = 2
		}

		public static void WriteNullableBool(this global::Mirror.NetworkWriter writer, bool? val)
		{
			global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue value = global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue.Null;
			if (val.HasValue)
			{
				value = (val.Value ? global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue.True : global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue.False);
			}
			writer.WriteByte((byte)value);
		}

		public static bool? ReadNullableBool(this global::Mirror.NetworkReader reader)
		{
			global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue nullableBoolValue = (global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue)reader.ReadByte();
			if (nullableBoolValue != global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue.Null)
			{
				return nullableBoolValue == global::Utils.Networking.NullableBoolReaderWriter.NullableBoolValue.True;
			}
			return null;
		}
	}
}
