namespace Utf8Json.Formatters
{
	public sealed class StringBuilderFormatter : global::Utf8Json.IJsonFormatter<global::System.Text.StringBuilder>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Text.StringBuilder> Default = new global::Utf8Json.Formatters.StringBuilderFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Text.StringBuilder value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

		public global::System.Text.StringBuilder Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return new global::System.Text.StringBuilder(reader.ReadString());
		}
	}
}
