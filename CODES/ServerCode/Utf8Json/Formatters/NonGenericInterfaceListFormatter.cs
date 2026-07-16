namespace Utf8Json.Formatters
{
	public sealed class NonGenericInterfaceListFormatter : global::Utf8Json.IJsonFormatter<global::System.Collections.IList>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Collections.IList> Default = new global::Utf8Json.Formatters.NonGenericInterfaceListFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Collections.IList value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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

		public global::System.Collections.IList Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			int count = 0;
			global::Utf8Json.IJsonFormatter<object> formatterWithVerify = formatterResolver.GetFormatterWithVerify<object>();
			global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>();
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				list.Add(formatterWithVerify.Deserialize(ref reader, formatterResolver));
			}
			return list;
		}
	}
}
