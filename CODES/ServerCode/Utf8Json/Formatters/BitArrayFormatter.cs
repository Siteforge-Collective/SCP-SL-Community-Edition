namespace Utf8Json.Formatters
{
	public sealed class BitArrayFormatter : global::Utf8Json.IJsonFormatter<global::System.Collections.BitArray>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Collections.BitArray> Default = new global::Utf8Json.Formatters.BitArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Collections.BitArray value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			for (int i = 0; i < value.Length; i++)
			{
				if (i != 0)
				{
					writer.WriteValueSeparator();
				}
				writer.WriteBoolean(value[i]);
			}
			writer.WriteEndArray();
		}

		public global::System.Collections.BitArray Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			int count = 0;
			global::Utf8Json.Internal.ArrayBuffer<bool> arrayBuffer = new global::Utf8Json.Internal.ArrayBuffer<bool>(4);
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				arrayBuffer.Add(reader.ReadBoolean());
			}
			return new global::System.Collections.BitArray(arrayBuffer.ToArray());
		}
	}
}
