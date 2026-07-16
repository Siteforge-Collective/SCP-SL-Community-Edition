namespace Utf8Json.Formatters
{
	public sealed class NullableStringFormatter : global::Utf8Json.IJsonFormatter<string>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IObjectPropertyNameFormatter<string>
	{
		public static readonly global::Utf8Json.IJsonFormatter<string> Default = new global::Utf8Json.Formatters.NullableStringFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, string value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteString(value);
		}

		public string Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadString();
		}

		public void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, string value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteString(value);
		}

		public string DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			return reader.ReadString();
		}
	}
}
