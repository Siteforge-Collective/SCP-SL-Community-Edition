namespace Utf8Json.Formatters
{
	public sealed class NullableDoubleFormatter : global::Utf8Json.IJsonFormatter<double?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<double?>
	{
		public static readonly global::Utf8Json.Formatters.NullableDoubleFormatter Default = new global::Utf8Json.Formatters.NullableDoubleFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, double? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteDouble(value.Value);
			}
		}

		public double? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadDouble();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, double? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteDouble(value.Value);
			writer.WriteQuotation();
		}

		public double? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadDouble(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
