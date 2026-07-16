namespace Utf8Json.Formatters
{
	public sealed class NullableInt32Formatter : global::Utf8Json.IJsonFormatter<int?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<int?>
	{
		public static readonly global::Utf8Json.Formatters.NullableInt32Formatter Default = new global::Utf8Json.Formatters.NullableInt32Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, int? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteInt32(value.Value);
			}
		}

		public int? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadInt32();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, int? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteInt32(value.Value);
			writer.WriteQuotation();
		}

		public int? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadInt32(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
