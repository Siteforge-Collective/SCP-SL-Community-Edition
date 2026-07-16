namespace Utf8Json.Formatters
{
	public sealed class DateTimeOffsetFormatter : global::Utf8Json.IJsonFormatter<global::System.DateTimeOffset>, global::Utf8Json.IJsonFormatter
	{
		private readonly string formatString;

		public DateTimeOffsetFormatter()
		{
			formatString = null;
		}

		public DateTimeOffsetFormatter(string formatString)
		{
			this.formatString = formatString;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.DateTimeOffset value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteString(value.ToString(formatString));
		}

		public global::System.DateTimeOffset Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			string input = reader.ReadString();
			if (formatString == null)
			{
				return global::System.DateTimeOffset.Parse(input, global::System.Globalization.CultureInfo.InvariantCulture);
			}
			return global::System.DateTimeOffset.ParseExact(input, formatString, global::System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
