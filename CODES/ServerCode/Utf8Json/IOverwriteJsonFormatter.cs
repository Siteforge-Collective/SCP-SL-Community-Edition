namespace Utf8Json
{
	public interface IOverwriteJsonFormatter<T> : global::Utf8Json.IJsonFormatter<T>, global::Utf8Json.IJsonFormatter
	{
		void DeserializeTo(ref T value, ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver);
	}
}
