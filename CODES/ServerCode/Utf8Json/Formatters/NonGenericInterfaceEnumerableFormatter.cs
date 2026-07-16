namespace Utf8Json.Formatters
{
	public sealed class NonGenericInterfaceEnumerableFormatter : global::Utf8Json.IJsonFormatter<global::System.Collections.IEnumerable>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Collections.IEnumerable> Default = new global::Utf8Json.Formatters.NonGenericInterfaceEnumerableFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Collections.IEnumerable value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::Utf8Json.IJsonFormatter<object> formatterWithVerify = formatterResolver.GetFormatterWithVerify<object>();
			writer.WriteBeginArray();
			int num = 0;
			foreach (object item in value)
			{
				if (num != 0)
				{
					writer.WriteValueSeparator();
				}
				formatterWithVerify.Serialize(ref writer, item, formatterResolver);
			}
			writer.WriteEndArray();
		}

		public global::System.Collections.IEnumerable Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
