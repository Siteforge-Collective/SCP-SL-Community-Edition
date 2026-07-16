namespace Utf8Json.Formatters
{
	public sealed class GuidFormatter : global::Utf8Json.IJsonFormatter<global::System.Guid>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<global::System.Guid>
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Guid> Default = new global::Utf8Json.Formatters.GuidFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Guid value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.EnsureCapacity(38);
			writer.WriteRawUnsafe(34);
			global::System.ArraySegment<byte> buffer = writer.GetBuffer();
			new global::Utf8Json.Internal.GuidBits(ref value).Write(buffer.Array, writer.CurrentOffset);
			writer.AdvanceOffset(36);
			writer.WriteRawUnsafe(34);
		}

		public global::System.Guid Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> utf8string = reader.ReadStringSegmentUnsafe();
			return new global::Utf8Json.Internal.GuidBits(ref utf8string).Value;
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, global::System.Guid value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			Serialize(ref writer, value, formatterResolver);
		}

		public global::System.Guid DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return Deserialize(ref reader, formatterResolver);
		}
	}
}
