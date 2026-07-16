namespace Utf8Json.Formatters
{
	public sealed class InterfaceListFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::System.Collections.Generic.List<T>, global::System.Collections.Generic.IList<T>>
	{
		protected override void Add(ref global::System.Collections.Generic.List<T> collection, int index, T value)
		{
			collection.Add(value);
		}

		protected override global::System.Collections.Generic.List<T> Create()
		{
			return new global::System.Collections.Generic.List<T>();
		}

		protected override global::System.Collections.Generic.IList<T> Complete(ref global::System.Collections.Generic.List<T> intermediateCollection)
		{
			return intermediateCollection;
		}
	}
}
