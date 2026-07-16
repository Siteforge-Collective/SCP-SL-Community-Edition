namespace Utf8Json.Formatters
{
	public sealed class AuthenticateResponseFormatter : global::Utf8Json.IJsonFormatter<AuthenticateResponse>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public AuthenticateResponseFormatter()
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
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("token"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("nonce"),
					4
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"),
					5
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("flags"),
					6
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("expiration"),
					7
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("preauth"),
					8
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("globalBan"),
					9
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("lifetime"),
					10
				}
			};
			____stringByteKeys = new byte[11][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("success"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("error"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("token"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("id"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nonce"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("country"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("flags"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("expiration"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("preauth"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("globalBan"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lifetime")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, AuthenticateResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteBoolean(value.success);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.error);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.token);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.id);
			writer.WriteRaw(____stringByteKeys[4]);
			writer.WriteString(value.nonce);
			writer.WriteRaw(____stringByteKeys[5]);
			writer.WriteString(value.country);
			writer.WriteRaw(____stringByteKeys[6]);
			writer.WriteByte(value.flags);
			writer.WriteRaw(____stringByteKeys[7]);
			writer.WriteInt64(value.expiration);
			writer.WriteRaw(____stringByteKeys[8]);
			writer.WriteString(value.preauth);
			writer.WriteRaw(____stringByteKeys[9]);
			writer.WriteString(value.globalBan);
			writer.WriteRaw(____stringByteKeys[10]);
			writer.WriteUInt16(value.lifetime);
			writer.WriteEndObject();
		}

		public AuthenticateResponse Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			bool success = false;
			string error = null;
			string token = null;
			string id = null;
			string nonce = null;
			string country = null;
			byte flags = 0;
			long expiration = 0L;
			string preauth = null;
			string globalBan = null;
			ushort lifetime = 0;
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
					token = reader.ReadString();
					break;
				case 3:
					id = reader.ReadString();
					break;
				case 4:
					nonce = reader.ReadString();
					break;
				case 5:
					country = reader.ReadString();
					break;
				case 6:
					flags = reader.ReadByte();
					break;
				case 7:
					expiration = reader.ReadInt64();
					break;
				case 8:
					preauth = reader.ReadString();
					break;
				case 9:
					globalBan = reader.ReadString();
					break;
				case 10:
					lifetime = reader.ReadUInt16();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new AuthenticateResponse(success, error, token, id, nonce, country, flags, expiration, preauth, globalBan, lifetime);
		}
	}
}
