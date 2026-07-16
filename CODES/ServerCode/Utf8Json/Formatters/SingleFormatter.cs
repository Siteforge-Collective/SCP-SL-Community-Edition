namespace Utf8Json.Formatters
{
	public sealed class SingleFormatter : global::Utf8Json.IJsonFormatter<float>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<float>
	{
		public static readonly global::Utf8Json.Formatters.SingleFormatter Default = new global::Utf8Json.Formatters.SingleFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, float value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteSingle(value);
		}

		public float Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadSingle();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, float value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteSingle(value);
			writer.WriteQuotation();
		}

		public float DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadSingle(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
