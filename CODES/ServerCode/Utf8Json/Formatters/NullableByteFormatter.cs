namespace Utf8Json.Formatters
{
	public sealed class NullableByteFormatter : global::Utf8Json.IJsonFormatter<byte?>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<byte?>
	{
		public static readonly global::Utf8Json.Formatters.NullableByteFormatter Default = new global::Utf8Json.Formatters.NullableByteFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, byte? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteByte(value.Value);
			}
		}

		public byte? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return reader.ReadByte();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, byte? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteQuotation();
			writer.WriteByte(value.Value);
			writer.WriteQuotation();
		}

		public byte? DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadByte(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
