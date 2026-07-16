namespace Utf8Json.Formatters
{
	public sealed class DateTimeFormatter : global::Utf8Json.IJsonFormatter<global::System.DateTime>, global::Utf8Json.IJsonFormatter
	{
		private readonly string formatString;

		public DateTimeFormatter()
		{
			formatString = null;
		}

		public DateTimeFormatter(string formatString)
		{
			this.formatString = formatString;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.DateTime value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteString(value.ToString(formatString));
		}

		public global::System.DateTime Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			string s = reader.ReadString();
			if (formatString == null)
			{
				return global::System.DateTime.Parse(s, global::System.Globalization.CultureInfo.InvariantCulture);
			}
			return global::System.DateTime.ParseExact(s, formatString, global::System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
