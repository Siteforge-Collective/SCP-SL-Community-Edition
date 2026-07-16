namespace Utf8Json.Formatters
{
	public sealed class UnixTimestampDateTimeFormatter : global::Utf8Json.IJsonFormatter<global::System.DateTime>, global::Utf8Json.IJsonFormatter
	{
		private static readonly global::System.DateTime UnixEpoch = new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc);

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.DateTime value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			long value2 = (long)(value.ToUniversalTime() - UnixEpoch).TotalSeconds;
			writer.WriteQuotation();
			writer.WriteInt64(value2);
			writer.WriteQuotation();
		}

		public global::System.DateTime Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentUnsafe();
			int readCount;
			ulong num = global::Utf8Json.Internal.NumberConverter.ReadUInt64(arraySegment.Array, arraySegment.Offset, out readCount);
			return UnixEpoch.AddSeconds(num);
		}
	}
}
