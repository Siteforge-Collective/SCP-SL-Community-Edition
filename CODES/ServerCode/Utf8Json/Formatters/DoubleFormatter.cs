namespace Utf8Json.Formatters
{
	public sealed class DoubleFormatter : global::Utf8Json.IJsonFormatter<double>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<double>
	{
		public static readonly global::Utf8Json.Formatters.DoubleFormatter Default = new global::Utf8Json.Formatters.DoubleFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, double value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteDouble(value);
		}

		public double Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadDouble();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, double value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteDouble(value);
			writer.WriteQuotation();
		}

		public double DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadDouble(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
