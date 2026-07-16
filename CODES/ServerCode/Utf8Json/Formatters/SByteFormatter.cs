namespace Utf8Json.Formatters
{
	public sealed class SByteFormatter : global::Utf8Json.IJsonFormatter<sbyte>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<sbyte>
	{
		public static readonly global::Utf8Json.Formatters.SByteFormatter Default = new global::Utf8Json.Formatters.SByteFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, sbyte value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteSByte(value);
		}

		public sbyte Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadSByte();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, sbyte value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteSByte(value);
			writer.WriteQuotation();
		}

		public sbyte DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadSByte(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
