namespace Utf8Json.Formatters
{
	public sealed class ByteArraySegmentFormatter : global::Utf8Json.IJsonFormatter<global::System.ArraySegment<byte>>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.ArraySegment<byte>> Default = new global::Utf8Json.Formatters.ByteArraySegmentFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.ArraySegment<byte> value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value.Array == null)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteString(global::System.Convert.ToBase64String(value.Array, value.Offset, value.Count, global::System.Base64FormattingOptions.None));
			}
		}

		public global::System.ArraySegment<byte> Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return default(global::System.ArraySegment<byte>);
			}
			byte[] array = global::System.Convert.FromBase64String(reader.ReadString());
			return new global::System.ArraySegment<byte>(array, 0, array.Length);
		}
	}
}
