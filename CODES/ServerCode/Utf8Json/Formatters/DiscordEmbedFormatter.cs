namespace Utf8Json.Formatters
{
	public sealed class DiscordEmbedFormatter : global::Utf8Json.IJsonFormatter<DiscordEmbed>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public DiscordEmbedFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("type"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("color"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("fields"),
					4
				}
			};
			____stringByteKeys = new byte[5][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("color"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("fields")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, DiscordEmbed value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.title);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.type);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.description);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteInt32(value.color);
			writer.WriteRaw(____stringByteKeys[4]);
			formatterResolver.GetFormatterWithVerify<DiscordEmbedField[]>().Serialize(ref writer, value.fields, formatterResolver);
			writer.WriteEndObject();
		}

		public DiscordEmbed Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string title = null;
			string type = null;
			string description = null;
			int color = 0;
			DiscordEmbedField[] fields = null;
			int count = 0;
			reader.ReadIsBeginObjectWithVerify();
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				global::System.ArraySegment<byte> key = reader.ReadPropertyNameSegmentRaw();
				if (!____keyMapping.TryGetValueSafe(key, out var value))
				{
					reader.ReadNextBlock();
					continue;
				}
				switch (value)
				{
				case 0:
					title = reader.ReadString();
					break;
				case 1:
					type = reader.ReadString();
					break;
				case 2:
					description = reader.ReadString();
					break;
				case 3:
					color = reader.ReadInt32();
					break;
				case 4:
					fields = formatterResolver.GetFormatterWithVerify<DiscordEmbedField[]>().Deserialize(ref reader, formatterResolver);
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new DiscordEmbed(title, type, description, color, fields);
		}
	}
}
