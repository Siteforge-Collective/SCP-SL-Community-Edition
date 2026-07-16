namespace Utf8Json.Formatters
{
	public sealed class NullableInt16Formatter : global::Utf8Json.IJsonFormatter<short?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<short?>
	{
		public static readonly global::Utf8Json.Formatters.NullableInt16Formatter Default = new global::Utf8Json.Formatters.NullableInt16Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, short? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteInt16(value.Value);
			}
		}

		public short? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadInt16();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, short? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteInt16(value.Value);
			writer.WriteQuotation();
		}

		public short? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadInt16(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
