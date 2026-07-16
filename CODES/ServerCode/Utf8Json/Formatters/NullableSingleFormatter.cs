namespace Utf8Json.Formatters
{
	public sealed class NullableSingleFormatter : global::Utf8Json.IJsonFormatter<float?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<float?>
	{
		public static readonly global::Utf8Json.Formatters.NullableSingleFormatter Default = new global::Utf8Json.Formatters.NullableSingleFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, float? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteSingle(value.Value);
			}
		}

		public float? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadSingle();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, float? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteSingle(value.Value);
			writer.WriteQuotation();
		}

		public float? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadSingle(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
