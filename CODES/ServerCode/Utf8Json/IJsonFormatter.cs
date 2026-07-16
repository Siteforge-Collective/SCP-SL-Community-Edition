namespace Utf8Json
{
	public interface IJsonFormatter
	{
	}
	public interface IJsonFormatter<T> : global::Utf8Json.IJsonFormatter
	{
		void Serialize(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver);

		T Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver);
	}
}
