namespace Utf8Json.Formatters
{
	public sealed class NullableUInt32Formatter : global::Utf8Json.IJsonFormatter<uint?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<uint?>
	{
		public static readonly global::Utf8Json.Formatters.NullableUInt32Formatter Default = new global::Utf8Json.Formatters.NullableUInt32Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, uint? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteUInt32(value.Value);
			}
		}

		public uint? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadUInt32();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, uint? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteUInt32(value.Value);
			writer.WriteQuotation();
		}

		public uint? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadUInt32(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
