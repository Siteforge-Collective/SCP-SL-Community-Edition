namespace Utf8Json.Formatters
{
	public sealed class IgnoreFormatter<T> : global::Utf8Json.IJsonFormatter<T>, global::Utf8Json.IJsonFormatter
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteNull();
		}

		public T Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			reader.ReadNextBlock();
			return default(T);
		}
	}
}
