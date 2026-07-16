namespace Utf8Json.Formatters
{
	public sealed class NonGenericListFormatter<T> : global::Utf8Json.IJsonFormatter<T>, global::Utf8Json.IJsonFormatter where T : class, global::System.Collections.IList, new()
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::Utf8Json.IJsonFormatter<object> formatterWithVerify = formatterResolver.GetFormatterWithVerify<object>();
			writer.WriteBeginArray();
			if (value.Count != 0)
			{
				formatterWithVerify.Serialize(ref writer, value[0], formatterResolver);
			}
			for (int i = 1; i < value.Count; i++)
			{
				writer.WriteValueSeparator();
				formatterWithVerify.Serialize(ref writer, value[i], formatterResolver);
			}
			writer.WriteEndArray();
		}

		public T Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			int count = 0;
			global::Utf8Json.IJsonFormatter<object> formatterWithVerify = formatterResolver.GetFormatterWithVerify<object>();
			T val = new T();
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				val.Add(formatterWithVerify.Deserialize(ref reader, formatterResolver));
			}
			return val;
		}
	}
}
