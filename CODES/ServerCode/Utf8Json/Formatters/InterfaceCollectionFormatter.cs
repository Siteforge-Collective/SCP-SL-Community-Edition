namespace Utf8Json.Formatters
{
	public sealed class InterfaceCollectionFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::System.Collections.Generic.List<T>, global::System.Collections.Generic.ICollection<T>>
	{
		protected override void Add(ref global::System.Collections.Generic.List<T> collection, int index, T value)
		{
			collection.Add(value);
		}

		protected override global::System.Collections.Generic.List<T> Create()
		{
			return new global::System.Collections.Generic.List<T>();
		}

		protected override global::System.Collections.Generic.ICollection<T> Complete(ref global::System.Collections.Generic.List<T> intermediateCollection)
		{
			return intermediateCollection;
		}
	}
}
