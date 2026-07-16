namespace Utf8Json.Formatters
{
	public sealed class SByteArrayFormatter : global::Utf8Json.IJsonFormatter<sbyte[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.SByteArrayFormatter Default = new global::Utf8Json.Formatters.SByteArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, sbyte[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteSByte(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteSByte(value[i]);
			}
			writer.WriteEndArray();
		}

		public sbyte[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			sbyte[] array = new sbyte[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadSByte();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
