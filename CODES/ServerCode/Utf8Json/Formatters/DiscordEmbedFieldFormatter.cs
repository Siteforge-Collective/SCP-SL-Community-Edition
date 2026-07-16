namespace Utf8Json.Formatters
{
	public sealed class DiscordEmbedFieldFormatter : global::Utf8Json.IJsonFormatter<DiscordEmbedField>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public DiscordEmbedFieldFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("value"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("inline"),
					2
				}
			};
			____stringByteKeys = new byte[3][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("name"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("inline")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, DiscordEmbedField value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.name);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.value);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteBoolean(value.inline);
			writer.WriteEndObject();
		}

		public DiscordEmbedField Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string name = null;
			string value = null;
			bool inline = false;
			int count = 0;
			reader.ReadIsBeginObjectWithVerify();
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				global::System.ArraySegment<byte> key = reader.ReadPropertyNameSegmentRaw();
				if (!____keyMapping.TryGetValueSafe(key, out var value2))
				{
					reader.ReadNextBlock();
					continue;
				}
				switch (value2)
				{
				case 0:
					name = reader.ReadString();
					break;
				case 1:
					value = reader.ReadString();
					break;
				case 2:
					inline = reader.ReadBoolean();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new DiscordEmbedField(name, value, inline);
		}
	}
}
