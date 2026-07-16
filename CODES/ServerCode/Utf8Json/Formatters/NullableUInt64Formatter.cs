namespace Utf8Json.Formatters
{
	public sealed class NullableUInt64Formatter : global::Utf8Json.IJsonFormatter<ulong?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<ulong?>
	{
		public static readonly global::Utf8Json.Formatters.NullableUInt64Formatter Default = new global::Utf8Json.Formatters.NullableUInt64Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, ulong? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteUInt64(value.Value);
			}
		}

		public ulong? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadUInt64();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, ulong? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteUInt64(value.Value);
			writer.WriteQuotation();
		}

		public ulong? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadUInt64(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
