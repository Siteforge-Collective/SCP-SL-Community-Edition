namespace Utf8Json.Formatters
{
	public sealed class Int32ArrayFormatter : global::Utf8Json.IJsonFormatter<int[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.Int32ArrayFormatter Default = new global::Utf8Json.Formatters.Int32ArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, int[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteInt32(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteInt32(value[i]);
			}
			writer.WriteEndArray();
		}

		public int[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			int[] array = new int[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadInt32();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
