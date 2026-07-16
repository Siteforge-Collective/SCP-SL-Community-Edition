namespace Utf8Json.Formatters
{
	public sealed class NonGenericInterfaceCollectionFormatter : global::Utf8Json.IJsonFormatter<global::System.Collections.ICollection>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Collections.ICollection> Default = new global::Utf8Json.Formatters.NonGenericInterfaceCollectionFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Collections.ICollection value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::Utf8Json.IJsonFormatter<object> formatterWithVerify = formatterResolver.GetFormatterWithVerify<object>();
			writer.WriteBeginArray();
			global::System.Collections.IEnumerator enumerator = value.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					formatterWithVerify.Serialize(ref writer, enumerator.Current, formatterResolver);
					while (enumerator.MoveNext())
					{
						writer.WriteValueSeparator();
						formatterWithVerify.Serialize(ref writer, enumerator.Current, formatterResolver);
					}
				}
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			writer.WriteEndArray();
		}

		public global::System.Collections.ICollection Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
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
