namespace Utf8Json.Formatters
{
	public sealed class GenericDictionaryFormatter<TKey, TValue, TDictionary> : global::Utf8Json.Formatters.DictionaryFormatterBase<TKey, TValue, TDictionary> where TDictionary : class, global::System.Collections.Generic.IDictionary<TKey, TValue>, new()
	{
		protected override void Add(ref TDictionary collection, int index, TKey key, TValue value)
		{
			collection.Add(key, value);
		}

		protected override TDictionary Create()
		{
			return new TDictionary();
		}
	}
}
