namespace Utf8Json.Formatters.Authenticator
{
	public sealed class AuthenticatorPlayerObjectFormatter : global::Utf8Json.IJsonFormatter<global::Authenticator.AuthenticatorPlayerObject>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public AuthenticatorPlayerObjectFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Id"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Ip"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("RequestIp"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Asn"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("AuthSerial"),
					4
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("VacSession"),
					5
				}
			};
			____stringByteKeys = new byte[6][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("Id"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Ip"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("RequestIp"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Asn"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("AuthSerial"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("VacSession")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::Authenticator.AuthenticatorPlayerObject value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.Id);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.Ip);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.RequestIp);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.Asn);
			writer.WriteRaw(____stringByteKeys[4]);
			writer.WriteString(value.AuthSerial);
			writer.WriteRaw(____stringByteKeys[5]);
			writer.WriteString(value.VacSession);
			writer.WriteEndObject();
		}

		public global::Authenticator.AuthenticatorPlayerObject Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string id = null;
			string ip = null;
			string requestIp = null;
			string asn = null;
			string authSerial = null;
			string vacSession = null;
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
					id = reader.ReadString();
					break;
				case 1:
					ip = reader.ReadString();
					break;
				case 2:
					requestIp = reader.ReadString();
					break;
				case 3:
					asn = reader.ReadString();
					break;
				case 4:
					authSerial = reader.ReadString();
					break;
				case 5:
					vacSession = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new global::Authenticator.AuthenticatorPlayerObject(id, ip, requestIp, asn, authSerial, vacSession);
		}
	}
}
