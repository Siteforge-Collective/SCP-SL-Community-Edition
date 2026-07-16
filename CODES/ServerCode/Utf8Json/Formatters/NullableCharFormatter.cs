namespace Utf8Json.Formatters
{
	public sealed class NullableCharFormatter : global::Utf8Json.IJsonFormatter<char?>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.NullableCharFormatter Default = new global::Utf8Json.Formatters.NullableCharFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, char? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				global::Utf8Json.Formatters.CharFormatter.Default.Serialize(ref writer, value.Value, formatterResolver);
			}
		}

		public char? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return global::Utf8Json.Formatters.CharFormatter.Default.Deserialize(ref reader, formatterResolver);
		}
	}
}
