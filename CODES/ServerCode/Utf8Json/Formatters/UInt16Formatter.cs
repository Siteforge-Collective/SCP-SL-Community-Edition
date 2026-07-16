namespace Utf8Json.Formatters
{
	public sealed class UInt16Formatter : global::Utf8Json.IJsonFormatter<ushort>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<ushort>
	{
		public static readonly global::Utf8Json.Formatters.UInt16Formatter Default = new global::Utf8Json.Formatters.UInt16Formatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, ushort value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteUInt16(value);
		}

		public ushort Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadUInt16();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, ushort value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteQuotation();
			writer.WriteUInt16(value);
			writer.WriteQuotation();
		}

		public ushort DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentRaw();
			int readCount;
			return global::Utf8Json.Internal.NumberConverter.ReadUInt16(arraySegment.Array, arraySegment.Offset, out readCount);
		}
	}
}
