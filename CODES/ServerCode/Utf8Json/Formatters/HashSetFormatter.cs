namespace Utf8Json.Formatters
{
	public sealed class HashSetFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::System.Collections.Generic.HashSet<T>, global::System.Collections.Generic.HashSet<T>.Enumerator, global::System.Collections.Generic.HashSet<T>>
	{
		protected override void Add(ref global::System.Collections.Generic.HashSet<T> collection, int index, T value)
		{
			collection.Add(value);
		}

		protected override global::System.Collections.Generic.HashSet<T> Complete(ref global::System.Collections.Generic.HashSet<T> intermediateCollection)
		{
			return intermediateCollection;
		}

		protected override global::System.Collections.Generic.HashSet<T> Create()
		{
			return new global::System.Collections.Generic.HashSet<T>();
		}

		protected override global::System.Collections.Generic.HashSet<T>.Enumerator GetSourceEnumerator(global::System.Collections.Generic.HashSet<T> source)
		{
			return source.GetEnumerator();
		}
	}
}
