namespace Utf8Json
{
	public interface IJsonFormatterResolver
	{
		global::Utf8Json.IJsonFormatter<T> GetFormatter<T>();
	}
}
