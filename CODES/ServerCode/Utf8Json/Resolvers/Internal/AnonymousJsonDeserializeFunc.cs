namespace Utf8Json.Resolvers.Internal
{
	internal delegate T AnonymousJsonDeserializeFunc<T>(object[] customFormatters, ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver resolver);
}
