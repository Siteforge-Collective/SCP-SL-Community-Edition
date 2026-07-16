namespace Utf8Json.Formatters
{
	public sealed class ByteFormatter : global::Utf8Json.IJsonFormatter<byte>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<byte>
	{
		public static readonly global::Utf8Json.Formatters.ByteFormatter Default = new global::Utf8Json.Formatters.ByteFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, byte value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteByte(value);
		}

		public byte Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadByte();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, byte value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteByte(value);
			writer.WriteQuotation();
		}

		public byte DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadByte(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
