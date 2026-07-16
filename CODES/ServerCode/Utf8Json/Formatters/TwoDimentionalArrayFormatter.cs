namespace Utf8Json.Formatters
{
	public sealed class TwoDimentionalArrayFormatter<T> : global::Utf8Json.IJsonFormatter<T[,]>, global::Utf8Json.IJsonFormatter
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, T[,] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			int length = value.GetLength(0);
			int length2 = value.GetLength(1);
			writer.WriteBeginArray();
			for (int i = 0; i < length; i++)
			{
				if (i != 0)
				{
					writer.WriteValueSeparator();
				}
				writer.WriteBeginArray();
				for (int j = 0; j < length2; j++)
				{
					if (j != 0)
					{
						writer.WriteValueSeparator();
					}
					formatterWithVerify.Serialize(ref writer, value[i, j], formatterResolver);
				}
				writer.WriteEndArray();
			}
			writer.WriteEndArray();
		}

		public T[,] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>> arrayBuffer = new global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>>(4);
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			int num = 0;
			int count = 0;
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				global::Utf8Json.Internal.ArrayBuffer<T> value = new global::Utf8Json.Internal.ArrayBuffer<T>((num == 0) ? 4 : num);
				int count2 = 0;
				reader.ReadIsBeginArrayWithVerify();
				while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count2))
				{
					value.Add(formatterWithVerify.Deserialize(ref reader, formatterResolver));
				}
				num = value.Size;
				arrayBuffer.Add(value);
			}
			T[,] array = new T[arrayBuffer.Size, num];
			for (int i = 0; i < arrayBuffer.Size; i++)
			{
				for (int j = 0; j < num; j++)
				{
					array[i, j] = arrayBuffer.Buffer[i].Buffer[j];
				}
			}
			return array;
		}
	}
}
