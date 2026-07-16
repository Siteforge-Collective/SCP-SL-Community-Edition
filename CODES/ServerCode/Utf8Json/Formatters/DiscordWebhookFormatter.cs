namespace Utf8Json.Formatters
{
	public sealed class DiscordWebhookFormatter : global::Utf8Json.IJsonFormatter<DiscordWebhook>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public DiscordWebhookFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("content"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("username"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("avatar_url"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("tts"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("embeds"),
					4
				}
			};
			____stringByteKeys = new byte[5][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("content"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("username"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("avatar_url"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tts"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("embeds")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, DiscordWebhook value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.content);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.username);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.avatar_url);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteBoolean(value.tts);
			writer.WriteRaw(____stringByteKeys[4]);
			formatterResolver.GetFormatterWithVerify<DiscordEmbed[]>().Serialize(ref writer, value.embeds, formatterResolver);
			writer.WriteEndObject();
		}

		public DiscordWebhook Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string content = null;
			string username = null;
			string avatar_url = null;
			bool tts = false;
			DiscordEmbed[] embeds = null;
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
					content = reader.ReadString();
					break;
				case 1:
					username = reader.ReadString();
					break;
				case 2:
					avatar_url = reader.ReadString();
					break;
				case 3:
					tts = reader.ReadBoolean();
					break;
				case 4:
					embeds = formatterResolver.GetFormatterWithVerify<DiscordEmbed[]>().Deserialize(ref reader, formatterResolver);
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new DiscordWebhook(content, username, avatar_url, tts, embeds);
		}
	}
}
