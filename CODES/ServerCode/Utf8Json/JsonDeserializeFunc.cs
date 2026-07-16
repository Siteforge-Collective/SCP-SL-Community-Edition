namespace Utf8Json
{
	public delegate T JsonDeserializeFunc<T>(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver resolver);
}
