namespace Utf8Json.Formatters
{
	public sealed class Int64Formatter : global::Utf8Json.IJsonFormatter<long>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<long>
	{
		public static readonly global::Utf8Json.Formatters.Int64Formatter Default = new global::Utf8Json.Formatters.Int64Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, long value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteInt64(value);
		}

		public long Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadInt64();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, long value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteInt64(value);
			writer.WriteQuotation();
		}

		public long DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadInt64(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
