namespace Utf8Json.Formatters
{
	public sealed class FourDimentionalArrayFormatter<T> : global::Utf8Json.IJsonFormatter<T[,,,]>, global::Utf8Json.IJsonFormatter
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, T[,,,] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			int length = value.GetLength(0);
			int length2 = value.GetLength(1);
			int length3 = value.GetLength(2);
			int length4 = value.GetLength(3);
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
					writer.WriteBeginArray();
					for (int k = 0; k < length3; k++)
					{
						if (k != 0)
						{
							writer.WriteValueSeparator();
						}
						writer.WriteBeginArray();
						for (int l = 0; l < length4; l++)
						{
							if (l != 0)
							{
								writer.WriteValueSeparator();
							}
							formatterWithVerify.Serialize(ref writer, value[i, j, k, l], formatterResolver);
						}
						writer.WriteEndArray();
					}
					writer.WriteEndArray();
				}
				writer.WriteEndArray();
			}
			writer.WriteEndArray();
		}

		public T[,,,] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>>>> arrayBuffer = new global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>>>>(4);
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int count = 0;
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>>> value = new global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>>>((num3 == 0) ? 4 : num3);
				int count2 = 0;
				reader.ReadIsBeginArrayWithVerify();
				while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count2))
				{
					global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>> value2 = new global::Utf8Json.Internal.ArrayBuffer<global::Utf8Json.Internal.ArrayBuffer<T>>((num2 == 0) ? 4 : num2);
					int count3 = 0;
					reader.ReadIsBeginArrayWithVerify();
					while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count3))
					{
						global::Utf8Json.Internal.ArrayBuffer<T> value3 = new global::Utf8Json.Internal.ArrayBuffer<T>((num == 0) ? 4 : num);
						int count4 = 0;
						reader.ReadIsBeginArrayWithVerify();
						while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count4))
						{
							value3.Add(formatterWithVerify.Deserialize(ref reader, formatterResolver));
						}
						num = value3.Size;
						value2.Add(value3);
					}
					num2 = value2.Size;
					value.Add(value2);
				}
				num3 = value.Size;
				arrayBuffer.Add(value);
			}
			T[,,,] array = new T[arrayBuffer.Size, num3, num2, num];
			for (int i = 0; i < arrayBuffer.Size; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num2; k++)
					{
						for (int l = 0; l < num; l++)
						{
							array[i, j, k, l] = arrayBuffer.Buffer[i].Buffer[j].Buffer[k].Buffer[l];
						}
					}
				}
			}
			return array;
		}
	}
}
