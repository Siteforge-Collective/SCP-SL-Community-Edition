namespace Utf8Json
{
	public static class JsonFormatterExtensions
	{
		public static string ToJsonString<T>(this global::Utf8Json.IJsonFormatter<T> formatter, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::Utf8Json.JsonWriter writer = default(global::Utf8Json.JsonWriter);
			formatter.Serialize(ref writer, value, formatterResolver);
			return writer.ToString();
		}
	}
}
