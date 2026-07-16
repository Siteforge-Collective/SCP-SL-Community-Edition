namespace Utf8Json.Formatters
{
	public sealed class InterfaceDictionaryFormatter<TKey, TValue> : global::Utf8Json.Formatters.DictionaryFormatterBase<TKey, TValue, global::System.Collections.Generic.Dictionary<TKey, TValue>, global::System.Collections.Generic.IDictionary<TKey, TValue>>
	{
		protected override void Add(ref global::System.Collections.Generic.Dictionary<TKey, TValue> collection, int index, TKey key, TValue value)
		{
			collection.Add(key, value);
		}

		protected override global::System.Collections.Generic.Dictionary<TKey, TValue> Create()
		{
			return new global::System.Collections.Generic.Dictionary<TKey, TValue>();
		}

		protected override global::System.Collections.Generic.IDictionary<TKey, TValue> Complete(ref global::System.Collections.Generic.Dictionary<TKey, TValue> intermediateCollection)
		{
			return intermediateCollection;
		}
	}
}
