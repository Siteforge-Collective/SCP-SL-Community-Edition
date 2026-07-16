namespace Utf8Json.Formatters
{
	public sealed class NullableStringArrayFormatter : global::Utf8Json.IJsonFormatter<string[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.NullableStringArrayFormatter Default = new global::Utf8Json.Formatters.NullableStringArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, string[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteString(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteString(value[i]);
			}
			writer.WriteEndArray();
		}

		public string[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			string[] array = new string[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadString();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
