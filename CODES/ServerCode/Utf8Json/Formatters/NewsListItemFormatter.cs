namespace Utf8Json.Formatters
{
	public sealed class NewsListItemFormatter : global::Utf8Json.IJsonFormatter<NewsListItem>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public NewsListItemFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("gid"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("url"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("is_external_url"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("author"),
					4
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("contents"),
					5
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("feedlabel"),
					6
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("date"),
					7
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("feedname"),
					8
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("feedtype"),
					9
				}
			};
			____stringByteKeys = new byte[10][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("gid"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("url"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("is_external_url"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("author"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("contents"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("feedlabel"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("date"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("feedname"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("feedtype")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, NewsListItem value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.gid);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.title);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.url);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteBoolean(value.is_external_url);
			writer.WriteRaw(____stringByteKeys[4]);
			writer.WriteString(value.author);
			writer.WriteRaw(____stringByteKeys[5]);
			writer.WriteString(value.contents);
			writer.WriteRaw(____stringByteKeys[6]);
			writer.WriteString(value.feedlabel);
			writer.WriteRaw(____stringByteKeys[7]);
			writer.WriteInt64(value.date);
			writer.WriteRaw(____stringByteKeys[8]);
			writer.WriteString(value.feedname);
			writer.WriteRaw(____stringByteKeys[9]);
			writer.WriteInt32(value.feedtype);
			writer.WriteEndObject();
		}

		public NewsListItem Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string gid = null;
			string title = null;
			string url = null;
			bool is_external_url = false;
			string author = null;
			string contents = null;
			string feedlabel = null;
			long date = 0L;
			string feedname = null;
			int feedtype = 0;
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
					gid = reader.ReadString();
					break;
				case 1:
					title = reader.ReadString();
					break;
				case 2:
					url = reader.ReadString();
					break;
				case 3:
					is_external_url = reader.ReadBoolean();
					break;
				case 4:
					author = reader.ReadString();
					break;
				case 5:
					contents = reader.ReadString();
					break;
				case 6:
					feedlabel = reader.ReadString();
					break;
				case 7:
					date = reader.ReadInt64();
					break;
				case 8:
					feedname = reader.ReadString();
					break;
				case 9:
					feedtype = reader.ReadInt32();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new NewsListItem(gid, title, url, is_external_url, author, contents, feedlabel, date, feedname, feedtype);
		}
	}
}
