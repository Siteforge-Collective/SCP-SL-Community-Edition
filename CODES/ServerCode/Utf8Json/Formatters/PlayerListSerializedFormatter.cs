namespace Utf8Json.Formatters
{
	public sealed class PlayerListSerializedFormatter : global::Utf8Json.IJsonFormatter<PlayerListSerialized>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public PlayerListSerializedFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary { 
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("objects"),
				0
			} };
			____stringByteKeys = new byte[1][] { global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("objects") };
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, PlayerListSerialized value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Serialize(ref writer, value.objects, formatterResolver);
			writer.WriteEndObject();
		}

		public PlayerListSerialized Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			global::System.Collections.Generic.List<string> objects = null;
			int count = 0;
			reader.ReadIsBeginObjectWithVerify();
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				global::System.ArraySegment<byte> key = reader.ReadPropertyNameSegmentRaw();
				if (!____keyMapping.TryGetValueSafe(key, out var value))
				{
					reader.ReadNextBlock();
				}
				else if (value == 0)
				{
					objects = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Deserialize(ref reader, formatterResolver);
				}
				else
				{
					reader.ReadNextBlock();
				}
			}
			return new PlayerListSerialized(objects);
		}
	}
}
