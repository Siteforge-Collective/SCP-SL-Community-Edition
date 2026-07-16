namespace Utf8Json.Formatters
{
	public sealed class ServerListSignedFormatter : global::Utf8Json.IJsonFormatter<ServerListSigned>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public ServerListSignedFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("payload"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("timestamp"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("signature"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("nonce"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("error"),
					4
				}
			};
			____stringByteKeys = new byte[5][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("payload"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("timestamp"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("signature"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nonce"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("error")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, ServerListSigned value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.payload);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteInt64(value.timestamp);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.signature);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.nonce);
			writer.WriteRaw(____stringByteKeys[4]);
			writer.WriteString(value.error);
			writer.WriteEndObject();
		}

		public ServerListSigned Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string payload = null;
			long timestamp = 0L;
			string signature = null;
			string nonce = null;
			string error = null;
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
					payload = reader.ReadString();
					break;
				case 1:
					timestamp = reader.ReadInt64();
					break;
				case 2:
					signature = reader.ReadString();
					break;
				case 3:
					nonce = reader.ReadString();
					break;
				case 4:
					error = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new ServerListSigned(payload, timestamp, signature, nonce, error);
		}
	}
}
