namespace Utf8Json.Formatters
{
	public sealed class InterfaceEnumerableFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::Utf8Json.Internal.ArrayBuffer<T>, global::System.Collections.Generic.IEnumerable<T>>
	{
		protected override void Add(ref global::Utf8Json.Internal.ArrayBuffer<T> collection, int index, T value)
		{
			collection.Add(value);
		}

		protected override global::Utf8Json.Internal.ArrayBuffer<T> Create()
		{
			return new global::Utf8Json.Internal.ArrayBuffer<T>(4);
		}

		protected override global::System.Collections.Generic.IEnumerable<T> Complete(ref global::Utf8Json.Internal.ArrayBuffer<T> intermediateCollection)
		{
			return intermediateCollection.ToArray();
		}
	}
}
