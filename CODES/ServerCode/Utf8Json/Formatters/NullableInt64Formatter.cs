namespace Utf8Json.Formatters
{
	public sealed class NullableInt64Formatter : global::Utf8Json.IJsonFormatter<long?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<long?>
	{
		public static readonly global::Utf8Json.Formatters.NullableInt64Formatter Default = new global::Utf8Json.Formatters.NullableInt64Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, long? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteInt64(value.Value);
			}
		}

		public long? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadInt64();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, long? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteInt64(value.Value);
			writer.WriteQuotation();
		}

		public long? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadInt64(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
