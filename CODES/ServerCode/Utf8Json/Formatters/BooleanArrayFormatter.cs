namespace Utf8Json.Formatters
{
	public sealed class BooleanArrayFormatter : global::Utf8Json.IJsonFormatter<bool[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.BooleanArrayFormatter Default = new global::Utf8Json.Formatters.BooleanArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, bool[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteBoolean(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteBoolean(value[i]);
			}
			writer.WriteEndArray();
		}

		public bool[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			bool[] array = new bool[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadBoolean();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
