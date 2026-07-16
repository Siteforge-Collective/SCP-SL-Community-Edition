namespace Utf8Json.Formatters
{
	public sealed class TimeSpanFormatter : global::Utf8Json.IJsonFormatter<global::System.TimeSpan>, global::Utf8Json.IJsonFormatter
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.TimeSpan value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteString(value.ToString());
		}

		public global::System.TimeSpan Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return global::System.TimeSpan.Parse(reader.ReadString());
		}
	}
}
