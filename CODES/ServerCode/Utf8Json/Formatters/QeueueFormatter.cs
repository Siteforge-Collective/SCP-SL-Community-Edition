namespace Utf8Json.Formatters
{
	public sealed class QeueueFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::System.Collections.Generic.Queue<T>, global::System.Collections.Generic.Queue<T>.Enumerator, global::System.Collections.Generic.Queue<T>>
	{
		private readonly global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour;

		protected override global::Utf8Json.CollectionDeserializeToBehaviour? SupportedOverwriteBehaviour => deserializeToBehaviour;

		public QeueueFormatter()
			: this(global::Utf8Json.CollectionDeserializeToBehaviour.Add)
		{
		}

		public QeueueFormatter(global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour)
		{
			this.deserializeToBehaviour = deserializeToBehaviour;
		}

		protected override void Add(ref global::System.Collections.Generic.Queue<T> collection, int index, T value)
		{
			collection.Enqueue(value);
		}

		protected override global::System.Collections.Generic.Queue<T> Create()
		{
			return new global::System.Collections.Generic.Queue<T>();
		}

		protected override global::System.Collections.Generic.Queue<T>.Enumerator GetSourceEnumerator(global::System.Collections.Generic.Queue<T> source)
		{
			return source.GetEnumerator();
		}

		protected override global::System.Collections.Generic.Queue<T> Complete(ref global::System.Collections.Generic.Queue<T> intermediateCollection)
		{
			return intermediateCollection;
		}

		protected override void AddOnOverwriteDeserialize(ref global::System.Collections.Generic.Queue<T> collection, int index, T value)
		{
			collection.Enqueue(value);
		}

		protected override void ClearOnOverwriteDeserialize(ref global::System.Collections.Generic.Queue<T> value)
		{
			value.Clear();
		}
	}
}
