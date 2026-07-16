namespace Utf8Json.Formatters
{
	internal class Grouping<TKey, TElement> : global::System.Linq.IGrouping<TKey, TElement>, global::System.Collections.Generic.IEnumerable<TElement>, global::System.Collections.IEnumerable
	{
		private readonly TKey key;

		private readonly global::System.Collections.Generic.IEnumerable<TElement> elements;

		public TKey Key => key;

		public Grouping(TKey key, global::System.Collections.Generic.IEnumerable<TElement> elements)
		{
			this.key = key;
			this.elements = elements;
		}

		public global::System.Collections.Generic.IEnumerator<TElement> GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
