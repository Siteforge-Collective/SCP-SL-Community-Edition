namespace Utf8Json
{
	public delegate void JsonSerializeAction<T>(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver resolver);
}
