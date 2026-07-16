namespace Utf8Json.Formatters
{
	public sealed class NullableDateTimeFormatter : global::Utf8Json.IJsonFormatter<global::System.DateTime?>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Formatters.DateTimeFormatter innerFormatter;

		public NullableDateTimeFormatter()
		{
			innerFormatter = new global::Utf8Json.Formatters.DateTimeFormatter();
		}

		public NullableDateTimeFormatter(string formatString)
		{
			innerFormatter = new global::Utf8Json.Formatters.DateTimeFormatter(formatString);
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.DateTime? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				innerFormatter.Serialize(ref writer, value.Value, formatterResolver);
			}
		}

		public global::System.DateTime? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return innerFormatter.Deserialize(ref reader, formatterResolver);
		}
	}
}
