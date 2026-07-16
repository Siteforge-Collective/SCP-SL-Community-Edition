namespace Utf8Json.Formatters
{
	public sealed class StackFormatter<T> : global::Utf8Json.Formatters.CollectionFormatterBase<T, global::Utf8Json.Internal.ArrayBuffer<T>, global::System.Collections.Generic.Stack<T>.Enumerator, global::System.Collections.Generic.Stack<T>>
	{
		protected override void Add(ref global::Utf8Json.Internal.ArrayBuffer<T> collection, int index, T value)
		{
			collection.Add(value);
		}

		protected override global::Utf8Json.Internal.ArrayBuffer<T> Create()
		{
			return new global::Utf8Json.Internal.ArrayBuffer<T>(4);
		}

		protected override global::System.Collections.Generic.Stack<T>.Enumerator GetSourceEnumerator(global::System.Collections.Generic.Stack<T> source)
		{
			return source.GetEnumerator();
		}

		protected override global::System.Collections.Generic.Stack<T> Complete(ref global::Utf8Json.Internal.ArrayBuffer<T> intermediateCollection)
		{
			T[] buffer = intermediateCollection.Buffer;
			global::System.Collections.Generic.Stack<T> stack = new global::System.Collections.Generic.Stack<T>(intermediateCollection.Size);
			for (int num = intermediateCollection.Size - 1; num >= 0; num--)
			{
				stack.Push(buffer[num]);
			}
			return stack;
		}
	}
}
