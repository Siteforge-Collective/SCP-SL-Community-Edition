namespace Utf8Json.Formatters
{
	public abstract class DictionaryFormatterBase<TKey, TValue, TIntermediate, TEnumerator, TDictionary> : global::Utf8Json.IJsonFormatter<TDictionary>, global::Utf8Json.IJsonFormatter where TEnumerator : global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<TKey, TValue>> where TDictionary : class, global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<TKey, TValue>>
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, TDictionary value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::Utf8Json.IObjectPropertyNameFormatter<TKey> objectPropertyNameFormatter = formatterResolver.GetFormatterWithVerify<TKey>() as global::Utf8Json.IObjectPropertyNameFormatter<TKey>;
			global::Utf8Json.IJsonFormatter<TValue> formatterWithVerify = formatterResolver.GetFormatterWithVerify<TValue>();
			writer.WriteBeginObject();
			TEnumerator sourceEnumerator = GetSourceEnumerator(value);
			try
			{
				if (objectPropertyNameFormatter != null)
				{
					if (sourceEnumerator.MoveNext())
					{
						global::System.Collections.Generic.KeyValuePair<TKey, TValue> current = sourceEnumerator.Current;
						objectPropertyNameFormatter.SerializeToPropertyName(ref writer, current.Key, formatterResolver);
						writer.WriteNameSeparator();
						formatterWithVerify.Serialize(ref writer, current.Value, formatterResolver);
						while (sourceEnumerator.MoveNext())
						{
							writer.WriteValueSeparator();
							global::System.Collections.Generic.KeyValuePair<TKey, TValue> current2 = sourceEnumerator.Current;
							objectPropertyNameFormatter.SerializeToPropertyName(ref writer, current2.Key, formatterResolver);
							writer.WriteNameSeparator();
							formatterWithVerify.Serialize(ref writer, current2.Value, formatterResolver);
						}
					}
				}
				else if (sourceEnumerator.MoveNext())
				{
					global::System.Collections.Generic.KeyValuePair<TKey, TValue> current3 = sourceEnumerator.Current;
					writer.WriteString(current3.Key.ToString());
					writer.WriteNameSeparator();
					formatterWithVerify.Serialize(ref writer, current3.Value, formatterResolver);
					while (sourceEnumerator.MoveNext())
					{
						writer.WriteValueSeparator();
						global::System.Collections.Generic.KeyValuePair<TKey, TValue> current4 = sourceEnumerator.Current;
						writer.WriteString(current4.Key.ToString());
						writer.WriteNameSeparator();
						formatterWithVerify.Serialize(ref writer, current4.Value, formatterResolver);
					}
				}
			}
			finally
			{
				sourceEnumerator.Dispose();
			}
			writer.WriteEndObject();
		}

		public TDictionary Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			if (!(formatterResolver.GetFormatterWithVerify<TKey>() is global::Utf8Json.IObjectPropertyNameFormatter<TKey> objectPropertyNameFormatter))
			{
				throw new global::System.InvalidOperationException(string.Concat(typeof(TKey), " does not support dictionary key deserialize."));
			}
			global::Utf8Json.IJsonFormatter<TValue> formatterWithVerify = formatterResolver.GetFormatterWithVerify<TValue>();
			reader.ReadIsBeginObjectWithVerify();
			TIntermediate collection = Create();
			int count = 0;
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				TKey key = objectPropertyNameFormatter.DeserializeFromPropertyName(ref reader, formatterResolver);
				reader.ReadIsNameSeparatorWithVerify();
				TValue value = formatterWithVerify.Deserialize(ref reader, formatterResolver);
				Add(ref collection, count - 1, key, value);
			}
			return Complete(ref collection);
		}

		protected abstract TEnumerator GetSourceEnumerator(TDictionary source);

		protected abstract TIntermediate Create();

		protected abstract void Add(ref TIntermediate collection, int index, TKey key, TValue value);

		protected abstract TDictionary Complete(ref TIntermediate intermediateCollection);
	}
	public abstract class DictionaryFormatterBase<TKey, TValue, TIntermediate, TDictionary> : global::Utf8Json.Formatters.DictionaryFormatterBase<TKey, TValue, TIntermediate, global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<TKey, TValue>>, TDictionary> where TDictionary : class, global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<TKey, TValue>>
	{
		protected override global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<TKey, TValue>> GetSourceEnumerator(TDictionary source)
		{
			return source.GetEnumerator();
		}
	}
	public abstract class DictionaryFormatterBase<TKey, TValue, TDictionary> : global::Utf8Json.Formatters.DictionaryFormatterBase<TKey, TValue, TDictionary, TDictionary> where TDictionary : class, global::System.Collections.Generic.IDictionary<TKey, TValue>
	{
		protected override TDictionary Complete(ref TDictionary intermediateCollection)
		{
			return intermediateCollection;
		}
	}
}
