namespace Utf8Json.Formatters
{
	internal class Lookup<TKey, TElement> : global::System.Linq.ILookup<TKey, TElement>, global::System.Collections.Generic.IEnumerable<global::System.Linq.IGrouping<TKey, TElement>>, global::System.Collections.IEnumerable
	{
		private readonly global::System.Collections.Generic.Dictionary<TKey, global::System.Linq.IGrouping<TKey, TElement>> groupings;

		public global::System.Collections.Generic.IEnumerable<TElement> this[TKey key] => groupings[key];

		public int Count => groupings.Count;

		public Lookup(global::System.Collections.Generic.Dictionary<TKey, global::System.Linq.IGrouping<TKey, TElement>> groupings)
		{
			this.groupings = groupings;
		}

		public bool Contains(TKey key)
		{
			return groupings.ContainsKey(key);
		}

		public global::System.Collections.Generic.IEnumerator<global::System.Linq.IGrouping<TKey, TElement>> GetEnumerator()
		{
			return groupings.Values.GetEnumerator();
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return groupings.Values.GetEnumerator();
		}
	}
}
