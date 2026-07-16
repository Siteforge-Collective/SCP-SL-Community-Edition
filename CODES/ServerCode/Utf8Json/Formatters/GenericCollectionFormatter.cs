namespace Utf8Json.Formatters
{
	public sealed class GenericCollectionFormatter<TElement, TCollection> : global::Utf8Json.Formatters.CollectionFormatterBase<TElement, TCollection> where TCollection : class, global::System.Collections.Generic.ICollection<TElement>, new()
	{
		protected override TCollection Create()
		{
			return new TCollection();
		}

		protected override void Add(ref TCollection collection, int index, TElement value)
		{
			collection.Add(value);
		}
	}
}
