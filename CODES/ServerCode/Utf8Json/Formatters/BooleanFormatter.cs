namespace Utf8Json.Formatters
{
	public sealed class BooleanFormatter : global::Utf8Json.IJsonFormatter<bool>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<bool>
	{
		public static readonly global::Utf8Json.Formatters.BooleanFormatter Default = new global::Utf8Json.Formatters.BooleanFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, bool value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteBoolean(value);
		}

		public bool Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadBoolean();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, bool value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteBoolean(value);
			writer.WriteQuotation();
		}

		public bool DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadBoolean(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
