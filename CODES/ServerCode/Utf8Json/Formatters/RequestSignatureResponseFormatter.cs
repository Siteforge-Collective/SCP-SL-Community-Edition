namespace Utf8Json.Formatters
{
	public sealed class RequestSignatureResponseFormatter : global::Utf8Json.IJsonFormatter<RequestSignatureResponse>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public RequestSignatureResponseFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("success"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("error"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("auth"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("badge"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("pub"),
					4
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("nonce"),
					5
				}
			};
			____stringByteKeys = new byte[6][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("success"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("error"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("auth"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("badge"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("pub"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nonce")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, RequestSignatureResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteBoolean(value.success);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.error);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.auth);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.badge);
			writer.WriteRaw(____stringByteKeys[4]);
			writer.WriteString(value.pub);
			writer.WriteRaw(____stringByteKeys[5]);
			writer.WriteString(value.nonce);
			writer.WriteEndObject();
		}

		public RequestSignatureResponse Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			bool success = false;
			string error = null;
			string auth = null;
			string badge = null;
			string pub = null;
			string nonce = null;
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
					success = reader.ReadBoolean();
					break;
				case 1:
					error = reader.ReadString();
					break;
				case 2:
					auth = reader.ReadString();
					break;
				case 3:
					badge = reader.ReadString();
					break;
				case 4:
					pub = reader.ReadString();
					break;
				case 5:
					nonce = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new RequestSignatureResponse(success, error, auth, badge, pub, nonce);
		}
	}
}
