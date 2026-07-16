namespace Utf8Json.Formatters
{
	public sealed class NullableBooleanFormatter : global::Utf8Json.IJsonFormatter<bool?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<bool?>
	{
		public static readonly global::Utf8Json.Formatters.NullableBooleanFormatter Default = new global::Utf8Json.Formatters.NullableBooleanFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, bool? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteBoolean(value.Value);
			}
		}

		public bool? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadBoolean();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, bool? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteBoolean(value.Value);
			writer.WriteQuotation();
		}

		public bool? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadBoolean(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
