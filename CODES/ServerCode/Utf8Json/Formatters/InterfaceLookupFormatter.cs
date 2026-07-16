namespace Utf8Json.Formatters
{
	public sealed class InterfaceLookupFormatter<TKey, TElement> : global::Utf8Json.IJsonFormatter<global::System.Linq.ILookup<TKey, TElement>>, global::Utf8Json.IJsonFormatter
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Linq.ILookup<TKey, TElement> value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else
			{
				formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::System.Linq.IGrouping<TKey, TElement>>>().Serialize(ref writer, global::System.Linq.Enumerable.AsEnumerable(value), formatterResolver);
			}
		}

		public global::System.Linq.ILookup<TKey, TElement> Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			if (reader.ReadIsNull())
			{
				return null;
			}
			int count = 0;
			global::Utf8Json.IJsonFormatter<global::System.Linq.IGrouping<TKey, TElement>> formatterWithVerify = formatterResolver.GetFormatterWithVerify<global::System.Linq.IGrouping<TKey, TElement>>();
			global::System.Collections.Generic.Dictionary<TKey, global::System.Linq.IGrouping<TKey, TElement>> dictionary = new global::System.Collections.Generic.Dictionary<TKey, global::System.Linq.IGrouping<TKey, TElement>>();
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				global::System.Linq.IGrouping<TKey, TElement> grouping = formatterWithVerify.Deserialize(ref reader, formatterResolver);
				dictionary.Add(grouping.Key, grouping);
			}
			return new global::Utf8Json.Formatters.Lookup<TKey, TElement>(dictionary);
		}
	}
}
