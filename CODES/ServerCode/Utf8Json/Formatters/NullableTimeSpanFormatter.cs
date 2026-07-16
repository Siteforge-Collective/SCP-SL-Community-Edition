namespace Utf8Json.Formatters
{
	public sealed class NullableTimeSpanFormatter : global::Utf8Json.IJsonFormatter<global::System.TimeSpan?>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Formatters.TimeSpanFormatter innerFormatter;

		public NullableTimeSpanFormatter()
		{
			innerFormatter = new global::Utf8Json.Formatters.TimeSpanFormatter();
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.TimeSpan? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

		public global::System.TimeSpan? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return innerFormatter.Deserialize(ref reader, formatterResolver);
		}
	}
}
