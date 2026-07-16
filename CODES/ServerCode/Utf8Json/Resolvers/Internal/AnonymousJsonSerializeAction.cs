namespace Utf8Json.Resolvers.Internal
{
	internal delegate void AnonymousJsonSerializeAction<T>(byte[][] stringByteKeysField, object[] customFormatters, ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver resolver);
}
