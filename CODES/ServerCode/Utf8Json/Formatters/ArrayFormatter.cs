namespace Utf8Json.Formatters
{
	public class ArrayFormatter<T> : global::Utf8Json.IJsonFormatter<T[]>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IOverwriteJsonFormatter<T[]>
	{
		private static readonly global::Utf8Json.Internal.ArrayPool<T> arrayPool = new global::Utf8Json.Internal.ArrayPool<T>(99);

		private readonly global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour;

		public ArrayFormatter()
			: this(global::Utf8Json.CollectionDeserializeToBehaviour.Add)
		{
		}

		public ArrayFormatter(global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour)
		{
			this.deserializeToBehaviour = deserializeToBehaviour;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, T[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			if (value.Length != 0)
			{
				formatterWithVerify.Serialize(ref writer, value[0], formatterResolver);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				formatterWithVerify.Serialize(ref writer, value[i], formatterResolver);
			}
			writer.WriteEndArray();
		}

		public T[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
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
				return array3;
			}
			finally
			{
				arrayPool.Return(array);
			}
		}

		public void DeserializeTo(ref T[] value, ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return;
			}
			int count = 0;
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			if (deserializeToBehaviour == global::Utf8Json.CollectionDeserializeToBehaviour.Add)
			{
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
					if (count != 0)
					{
						T[] destinationArray = new T[value.Length + count];
						global::System.Array.Copy(value, 0, destinationArray, 0, value.Length);
						global::System.Array.Copy(array2, 0, destinationArray, value.Length, count);
						global::System.Array.Clear(array, 0, global::System.Math.Min(count, array.Length));
					}
					return;
				}
				finally
				{
					arrayPool.Return(array);
				}
			}
			T[] array3 = value;
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array3.Length < count)
				{
					global::System.Array.Resize(ref array3, array3.Length * 2);
				}
				array3[count - 1] = formatterWithVerify.Deserialize(ref reader, formatterResolver);
			}
			global::System.Array.Resize(ref array3, count);
		}
	}
}
