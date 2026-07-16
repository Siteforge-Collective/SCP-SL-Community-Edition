namespace Utf8Json.Formatters
{
	public sealed class NonGenericInterfaceDictionaryFormatter : global::Utf8Json.IJsonFormatter<global::System.Collections.IDictionary>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Collections.IDictionary> Default = new global::Utf8Json.Formatters.NonGenericInterfaceDictionaryFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Collections.IDictionary value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::Utf8Json.IJsonFormatter<object> formatterWithVerify = formatterResolver.GetFormatterWithVerify<object>();
			writer.WriteBeginObject();
			global::System.Collections.IDictionaryEnumerator dictionaryEnumerator = value.GetEnumerator();
			try
			{
				if (dictionaryEnumerator.MoveNext())
				{
					global::System.Collections.DictionaryEntry dictionaryEntry = (global::System.Collections.DictionaryEntry)dictionaryEnumerator.Current;
					writer.WritePropertyName(dictionaryEntry.Key.ToString());
					formatterWithVerify.Serialize(ref writer, dictionaryEntry.Value, formatterResolver);
					while (dictionaryEnumerator.MoveNext())
					{
						writer.WriteValueSeparator();
						global::System.Collections.DictionaryEntry dictionaryEntry2 = (global::System.Collections.DictionaryEntry)dictionaryEnumerator.Current;
						writer.WritePropertyName(dictionaryEntry2.Key.ToString());
						formatterWithVerify.Serialize(ref writer, dictionaryEntry2.Value, formatterResolver);
					}
				}
			}
			finally
			{
				global::System.IDisposable disposable = dictionaryEnumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			writer.WriteEndObject();
		}

		public global::System.Collections.IDictionary Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::Utf8Json.IJsonFormatter<object> formatterWithVerify = formatterResolver.GetFormatterWithVerify<object>();
			reader.ReadIsBeginObjectWithVerify();
			global::System.Collections.Generic.Dictionary<object, object> dictionary = new global::System.Collections.Generic.Dictionary<object, object>();
			int count = 0;
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				string key = reader.ReadPropertyName();
				object value = formatterWithVerify.Deserialize(ref reader, formatterResolver);
				dictionary.Add(key, value);
			}
			return dictionary;
		}
	}
}
