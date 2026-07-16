namespace Utf8Json.Formatters
{
	public sealed class Int16Formatter : global::Utf8Json.IJsonFormatter<short>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<short>
	{
		public static readonly global::Utf8Json.Formatters.Int16Formatter Default = new global::Utf8Json.Formatters.Int16Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, short value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteInt16(value);
		}

		public short Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadInt16();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, short value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteInt16(value);
			writer.WriteQuotation();
		}

		public short DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadInt16(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
