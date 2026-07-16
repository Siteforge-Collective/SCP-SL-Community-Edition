namespace Utf8Json.Formatters
{
	public sealed class CharArrayFormatter : global::Utf8Json.IJsonFormatter<char[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.CharArrayFormatter Default = new global::Utf8Json.Formatters.CharArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, char[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				global::Utf8Json.Formatters.CharFormatter.Default.Serialize(ref writer, value[0], formatterResolver);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				global::Utf8Json.Formatters.CharFormatter.Default.Serialize(ref writer, value[i], formatterResolver);
			}
			writer.WriteEndArray();
		}

		public char[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			char[] array = new char[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = global::Utf8Json.Formatters.CharFormatter.Default.Deserialize(ref reader, formatterResolver);
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
