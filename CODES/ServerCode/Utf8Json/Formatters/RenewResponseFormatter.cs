namespace Utf8Json.Formatters
{
	public sealed class RenewResponseFormatter : global::Utf8Json.IJsonFormatter<RenewResponse>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public RenewResponseFormatter()
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
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("id"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("nonce"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("country"),
					4
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("flags"),
					5
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("expiration"),
					6
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("preauth"),
					7
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("globalBan"),
					8
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("lifetime"),
					9
				}
			};
			____stringByteKeys = new byte[10][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("success"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("error"),
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

		public void Serialize(ref global::Utf8Json.JsonWriter writer, RenewResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteBoolean(value.success);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.error);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.id);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.nonce);
			writer.WriteRaw(____stringByteKeys[4]);
			writer.WriteString(value.country);
			writer.WriteRaw(____stringByteKeys[5]);
			writer.WriteByte(value.flags);
			writer.WriteRaw(____stringByteKeys[6]);
			writer.WriteInt64(value.expiration);
			writer.WriteRaw(____stringByteKeys[7]);
			writer.WriteString(value.preauth);
			writer.WriteRaw(____stringByteKeys[8]);
			writer.WriteString(value.globalBan);
			writer.WriteRaw(____stringByteKeys[9]);
			writer.WriteUInt16(value.lifetime);
			writer.WriteEndObject();
		}

		public RenewResponse Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			bool success = false;
			string error = null;
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
					id = reader.ReadString();
					break;
				case 3:
					nonce = reader.ReadString();
					break;
				case 4:
					country = reader.ReadString();
					break;
				case 5:
					flags = reader.ReadByte();
					break;
				case 6:
					expiration = reader.ReadInt64();
					break;
				case 7:
					preauth = reader.ReadString();
					break;
				case 8:
					globalBan = reader.ReadString();
					break;
				case 9:
					lifetime = reader.ReadUInt16();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new RenewResponse(success, error, id, nonce, country, flags, expiration, preauth, globalBan, lifetime);
		}
	}
}
