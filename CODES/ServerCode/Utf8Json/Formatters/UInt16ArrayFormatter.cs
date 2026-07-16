namespace Utf8Json.Formatters
{
	public sealed class UInt16ArrayFormatter : global::Utf8Json.IJsonFormatter<ushort[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.UInt16ArrayFormatter Default = new global::Utf8Json.Formatters.UInt16ArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, ushort[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteUInt16(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteUInt16(value[i]);
			}
			writer.WriteEndArray();
		}

		public ushort[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			ushort[] array = new ushort[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadUInt16();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
