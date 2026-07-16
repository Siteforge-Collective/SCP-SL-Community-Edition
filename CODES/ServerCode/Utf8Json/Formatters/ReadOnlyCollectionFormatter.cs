namespace Utf8Json.Formatters
{
	public sealed class ReadOnlyCollectionFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::Utf8Json.Internal.ArrayBuffer<T>, global::System.Collections.ObjectModel.ReadOnlyCollection<T>>
	{
		protected override void Add(ref global::Utf8Json.Internal.ArrayBuffer<T> collection, int index, T value)
		{
			collection.Add(value);
		}

		protected override global::System.Collections.ObjectModel.ReadOnlyCollection<T> Complete(ref global::Utf8Json.Internal.ArrayBuffer<T> intermediateCollection)
		{
			return new global::System.Collections.ObjectModel.ReadOnlyCollection<T>(intermediateCollection.ToArray());
		}

		protected override global::Utf8Json.Internal.ArrayBuffer<T> Create()
		{
			return new global::Utf8Json.Internal.ArrayBuffer<T>(4);
		}
	}
}
