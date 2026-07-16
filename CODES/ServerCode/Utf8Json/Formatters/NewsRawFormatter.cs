namespace Utf8Json.Formatters
{
	public sealed class NewsRawFormatter : global::Utf8Json.IJsonFormatter<NewsRaw>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public NewsRawFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary { 
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("appnews"),
				0
			} };
			____stringByteKeys = new byte[1][] { global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("appnews") };
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, NewsRaw value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			formatterResolver.GetFormatterWithVerify<NewsList>().Serialize(ref writer, value.appnews, formatterResolver);
			writer.WriteEndObject();
		}

		public NewsRaw Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			NewsList appnews = default(NewsList);
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
					appnews = formatterResolver.GetFormatterWithVerify<NewsList>().Deserialize(ref reader, formatterResolver);
				}
				else
				{
					reader.ReadNextBlock();
				}
			}
			return new NewsRaw(appnews);
		}
	}
}
