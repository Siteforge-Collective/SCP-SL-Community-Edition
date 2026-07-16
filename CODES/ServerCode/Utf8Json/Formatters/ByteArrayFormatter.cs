namespace Utf8Json.Formatters
{
	public sealed class ByteArrayFormatter : global::Utf8Json.IJsonFormatter<byte[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<byte[]> Default = new global::Utf8Json.Formatters.ByteArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, byte[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteString(global::System.Convert.ToBase64String(value, global::System.Base64FormattingOptions.None));
			}
		}

		public byte[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return global::System.Convert.FromBase64String(reader.ReadString());
		}
	}
}
