namespace Utf8Json.Formatters
{
	public sealed class UriFormatter : global::Utf8Json.IJsonFormatter<global::System.Uri>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Uri> Default = new global::Utf8Json.Formatters.UriFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Uri value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteString(value.ToString());
			}
		}

		public global::System.Uri Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return new global::System.Uri(reader.ReadString(), global::System.UriKind.RelativeOrAbsolute);
		}
	}
}
