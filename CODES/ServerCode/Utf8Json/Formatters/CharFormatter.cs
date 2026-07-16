namespace Utf8Json.Formatters
{
	public sealed class CharFormatter : global::Utf8Json.IJsonFormatter<char>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.CharFormatter Default = new global::Utf8Json.Formatters.CharFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, char value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteString(value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
		}

		public char Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadString()[0];
		}
	}
}
