namespace Utf8Json.Formatters
{
	public sealed class SortedListFormatter<TKey, TValue> : global::Utf8Json.Formatters.DictionaryFormatterBase<TKey, TValue, global::System.Collections.Generic.SortedList<TKey, TValue>>
	{
		protected override void Add(ref global::System.Collections.Generic.SortedList<TKey, TValue> collection, int index, TKey key, TValue value)
		{
			collection.Add(key, value);
		}

		protected override global::System.Collections.Generic.SortedList<TKey, TValue> Create()
		{
			return new global::System.Collections.Generic.SortedList<TKey, TValue>();
		}
	}
}
