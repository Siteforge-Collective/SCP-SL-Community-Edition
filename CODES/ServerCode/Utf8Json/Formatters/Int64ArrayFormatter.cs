namespace Utf8Json.Formatters
{
	public sealed class Int64ArrayFormatter : global::Utf8Json.IJsonFormatter<long[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.Int64ArrayFormatter Default = new global::Utf8Json.Formatters.Int64ArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, long[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteInt64(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteInt64(value[i]);
			}
			writer.WriteEndArray();
		}

		public long[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			long[] array = new long[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadInt64();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
