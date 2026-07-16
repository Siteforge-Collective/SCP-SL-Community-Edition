namespace Utf8Json.Formatters
{
	public sealed class LinkedListFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::System.Collections.Generic.LinkedList<T>, global::System.Collections.Generic.LinkedList<T>.Enumerator, global::System.Collections.Generic.LinkedList<T>>
	{
		private readonly global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour;

		protected override global::Utf8Json.CollectionDeserializeToBehaviour? SupportedOverwriteBehaviour => deserializeToBehaviour;

		public LinkedListFormatter()
			: this(global::Utf8Json.CollectionDeserializeToBehaviour.Add)
		{
		}

		public LinkedListFormatter(global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour)
		{
			this.deserializeToBehaviour = deserializeToBehaviour;
		}

		protected override void Add(ref global::System.Collections.Generic.LinkedList<T> collection, int index, T value)
		{
			collection.AddLast(value);
		}

		protected override global::System.Collections.Generic.LinkedList<T> Complete(ref global::System.Collections.Generic.LinkedList<T> intermediateCollection)
		{
			return intermediateCollection;
		}

		protected override global::System.Collections.Generic.LinkedList<T> Create()
		{
			return new global::System.Collections.Generic.LinkedList<T>();
		}

		protected override global::System.Collections.Generic.LinkedList<T>.Enumerator GetSourceEnumerator(global::System.Collections.Generic.LinkedList<T> source)
		{
			return source.GetEnumerator();
		}

		protected override void AddOnOverwriteDeserialize(ref global::System.Collections.Generic.LinkedList<T> collection, int index, T value)
		{
			collection.AddLast(value);
		}

		protected override void ClearOnOverwriteDeserialize(ref global::System.Collections.Generic.LinkedList<T> value)
		{
			value.Clear();
		}
	}
}
