namespace Utf8Json.Formatters
{
	public sealed class NewsListFormatter : global::Utf8Json.IJsonFormatter<NewsList>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public NewsListFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("appid"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("newsitems"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("count"),
					2
				}
			};
			____stringByteKeys = new byte[3][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("appid"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("newsitems"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("count")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, NewsList value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteInt32(value.appid);
			writer.WriteRaw(____stringByteKeys[1]);
			formatterResolver.GetFormatterWithVerify<NewsListItem[]>().Serialize(ref writer, value.newsitems, formatterResolver);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteInt32(value.count);
			writer.WriteEndObject();
		}

		public NewsList Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			int appid = 0;
			NewsListItem[] newsitems = null;
			int count = 0;
			int count2 = 0;
			reader.ReadIsBeginObjectWithVerify();
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count2))
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
					appid = reader.ReadInt32();
					break;
				case 1:
					newsitems = formatterResolver.GetFormatterWithVerify<NewsListItem[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 2:
					count = reader.ReadInt32();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new NewsList(appid, newsitems, count);
		}
	}
}
