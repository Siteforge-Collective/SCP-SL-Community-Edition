namespace Utf8Json.Formatters
{
	public sealed class QueryRaReplyFormatter : global::Utf8Json.IJsonFormatter<QueryRaReply>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public QueryRaReplyFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Text"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Success"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("LogToConsole"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("OverrideDisplay"),
					3
				}
			};
			____stringByteKeys = new byte[4][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("Text"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Success"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LogToConsole"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("OverrideDisplay")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, QueryRaReply value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.Text);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteBoolean(value.Success);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteBoolean(value.LogToConsole);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.OverrideDisplay);
			writer.WriteEndObject();
		}

		public QueryRaReply Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string text = null;
			bool success = false;
			bool logToConsole = false;
			string overrideDisplay = null;
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
					text = reader.ReadString();
					break;
				case 1:
					success = reader.ReadBoolean();
					break;
				case 2:
					logToConsole = reader.ReadBoolean();
					break;
				case 3:
					overrideDisplay = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new QueryRaReply(text, success, logToConsole, overrideDisplay);
		}
	}
}
