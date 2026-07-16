namespace Utf8Json
{
	public interface IObjectPropertyNameFormatter<T>
	{
		void SerializeToPropertyName(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver);

		T DeserializeFromPropertyName(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver);
	}
}
