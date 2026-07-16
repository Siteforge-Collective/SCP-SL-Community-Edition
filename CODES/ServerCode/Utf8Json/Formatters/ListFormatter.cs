namespace Utf8Json.Formatters
{
	public class ListFormatter<T> : global::Utf8Json.IJsonFormatter<global::System.Collections.Generic.List<T>>, global::Utf8Json.IJsonFormatter, global::Utf8Json.IOverwriteJsonFormatter<global::System.Collections.Generic.List<T>>
	{
		private readonly global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour;

		public ListFormatter()
			: this(global::Utf8Json.CollectionDeserializeToBehaviour.Add)
		{
		}

		public ListFormatter(global::Utf8Json.CollectionDeserializeToBehaviour deserializeToBehaviour)
		{
			this.deserializeToBehaviour = deserializeToBehaviour;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Collections.Generic.List<T> value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			if (value.Count != 0)
			{
				formatterWithVerify.Serialize(ref writer, value[0], formatterResolver);
			}
			for (int i = 1; i < value.Count; i++)
			{
				writer.WriteValueSeparator();
				formatterWithVerify.Serialize(ref writer, value[i], formatterResolver);
			}
			writer.WriteEndArray();
		}

		public global::System.Collections.Generic.List<T> Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			int count = 0;
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
			reader.ReadIsBeginArrayWithVerify();
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				list.Add(formatterWithVerify.Deserialize(ref reader, formatterResolver));
			}
			return list;
		}

		public void DeserializeTo(ref global::System.Collections.Generic.List<T> value, ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!reader.ReadIsNull())
			{
				int count = 0;
				global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
				global::System.Collections.Generic.List<T> list = value;
				if (deserializeToBehaviour == global::Utf8Json.CollectionDeserializeToBehaviour.OverwriteReplace)
				{
					list.Clear();
				}
				reader.ReadIsBeginArrayWithVerify();
				while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
				{
					list.Add(formatterWithVerify.Deserialize(ref reader, formatterResolver));
				}
			}
		}
	}
}
