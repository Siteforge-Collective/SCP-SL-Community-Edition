namespace Utf8Json.Formatters
{
	public sealed class SortedDictionaryFormatter<TKey, TValue> : global::Utf8Json.Formatters.DictionaryFormatterBase<TKey, TValue, global::System.Collections.Generic.SortedDictionary<TKey, TValue>, global::System.Collections.Generic.SortedDictionary<TKey, TValue>.Enumerator, global::System.Collections.Generic.SortedDictionary<TKey, TValue>>
	{
		protected override void Add(ref global::System.Collections.Generic.SortedDictionary<TKey, TValue> collection, int index, TKey key, TValue value)
		{
			collection.Add(key, value);
		}

		protected override global::System.Collections.Generic.SortedDictionary<TKey, TValue> Complete(ref global::System.Collections.Generic.SortedDictionary<TKey, TValue> intermediateCollection)
		{
			return intermediateCollection;
		}

		protected override global::System.Collections.Generic.SortedDictionary<TKey, TValue> Create()
		{
			return new global::System.Collections.Generic.SortedDictionary<TKey, TValue>();
		}

		protected override global::System.Collections.Generic.SortedDictionary<TKey, TValue>.Enumerator GetSourceEnumerator(global::System.Collections.Generic.SortedDictionary<TKey, TValue> source)
		{
			return source.GetEnumerator();
		}
	}
}
