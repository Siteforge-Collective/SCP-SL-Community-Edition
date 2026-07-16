namespace Utf8Json.Formatters
{
	public sealed class NullableDateTimeOffsetFormatter : global::Utf8Json.IJsonFormatter<global::System.DateTimeOffset?>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Formatters.DateTimeOffsetFormatter innerFormatter;

		public NullableDateTimeOffsetFormatter()
		{
			innerFormatter = new global::Utf8Json.Formatters.DateTimeOffsetFormatter();
		}

		public NullableDateTimeOffsetFormatter(string formatString)
		{
			innerFormatter = new global::Utf8Json.Formatters.DateTimeOffsetFormatter(formatString);
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.DateTimeOffset? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

		public global::System.DateTimeOffset? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return innerFormatter.Deserialize(ref reader, formatterResolver);
		}
	}
}
