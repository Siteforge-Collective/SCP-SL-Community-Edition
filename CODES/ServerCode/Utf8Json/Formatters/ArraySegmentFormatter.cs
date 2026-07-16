namespace Utf8Json.Formatters
{
	public class ArraySegmentFormatter<T> : global::Utf8Json.IJsonFormatter<global::System.ArraySegment<T>>, global::Utf8Json.IJsonFormatter
	{
		private static readonly global::Utf8Json.Internal.ArrayPool<T> arrayPool = new global::Utf8Json.Internal.ArrayPool<T>(99);

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.ArraySegment<T> value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value.Array == null)
			{
				writer.WriteNull();
				return;
			}
			T[] array = value.Array;
			int offset = value.Offset;
			int count = value.Count;
			writer.WriteBeginArray();
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			if (count != 0)
			{
				formatterWithVerify.Serialize(ref writer, value.Array[offset], formatterResolver);
			}
			for (int i = 1; i < count; i++)
			{
				writer.WriteValueSeparator();
				formatterWithVerify.Serialize(ref writer, array[offset + i], formatterResolver);
			}
			writer.WriteEndArray();
		}

		public global::System.ArraySegment<T> Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return default(global::System.ArraySegment<T>);
			}
			int count = 0;
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			T[] array = arrayPool.Rent();
			try
			{
				T[] array2 = array;
				reader.ReadIsBeginArrayWithVerify();
				while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
				{
					if (array2.Length < count)
					{
						global::System.Array.Resize(ref array2, array2.Length * 2);
					}
					array2[count - 1] = formatterWithVerify.Deserialize(ref reader, formatterResolver);
				}
				T[] array3 = new T[count];
				global::System.Array.Copy(array2, array3, count);
				global::System.Array.Clear(array, 0, global::System.Math.Min(count, array.Length));
				return new global::System.ArraySegment<T>(array3, 0, array3.Length);
			}
			finally
			{
				arrayPool.Return(array);
			}
		}
	}
}
