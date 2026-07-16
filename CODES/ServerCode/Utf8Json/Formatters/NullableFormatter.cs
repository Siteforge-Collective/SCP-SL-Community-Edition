namespace Utf8Json.Formatters
{
	public sealed class NullableFormatter<T> : global::Utf8Json.IJsonFormatter<T?>, global::Utf8Json.IJsonFormatter where T : struct
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, T? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				formatterResolver.GetFormatterWithVerify<T>().Serialize(ref writer, value.Value, formatterResolver);
			}
		}

		public T? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return formatterResolver.GetFormatterWithVerify<T>().Deserialize(ref reader, formatterResolver);
		}
	}
}
