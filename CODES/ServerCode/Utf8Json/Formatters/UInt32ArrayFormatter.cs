namespace Utf8Json.Formatters
{
	public sealed class UInt32ArrayFormatter : global::Utf8Json.IJsonFormatter<uint[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.UInt32ArrayFormatter Default = new global::Utf8Json.Formatters.UInt32ArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, uint[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteUInt32(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteUInt32(value[i]);
			}
			writer.WriteEndArray();
		}

		public uint[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			uint[] array = new uint[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadUInt32();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
