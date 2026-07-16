namespace Utf8Json.Formatters
{
	public sealed class AuthenticatorResponseFormatter : global::Utf8Json.IJsonFormatter<AuthenticatorResponse>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public AuthenticatorResponseFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("success"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("verified"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("error"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("token"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("messages"),
					4
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("actions"),
					5
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("authAccepted"),
					6
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("authRejected"),
					7
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("verificationChallenge"),
					8
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("verificationResponse"),
					9
				}
			};
			____stringByteKeys = new byte[10][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("success"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("verified"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("error"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("token"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("messages"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("actions"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("authAccepted"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("authRejected"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("verificationChallenge"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("verificationResponse")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, AuthenticatorResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteBoolean(value.success);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteBoolean(value.verified);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.error);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.token);
			writer.WriteRaw(____stringByteKeys[4]);
			formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.messages, formatterResolver);
			writer.WriteRaw(____stringByteKeys[5]);
			formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.actions, formatterResolver);
			writer.WriteRaw(____stringByteKeys[6]);
			formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.authAccepted, formatterResolver);
			writer.WriteRaw(____stringByteKeys[7]);
			formatterResolver.GetFormatterWithVerify<AuthenticatiorAuthReject[]>().Serialize(ref writer, value.authRejected, formatterResolver);
			writer.WriteRaw(____stringByteKeys[8]);
			writer.WriteString(value.verificationChallenge);
			writer.WriteRaw(____stringByteKeys[9]);
			writer.WriteString(value.verificationResponse);
			writer.WriteEndObject();
		}

		public AuthenticatorResponse Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			bool success = false;
			bool verified = false;
			string error = null;
			string token = null;
			string[] messages = null;
			string[] actions = null;
			string[] authAccepted = null;
			AuthenticatiorAuthReject[] authRejected = null;
			string verificationChallenge = null;
			string verificationResponse = null;
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
					verified = reader.ReadBoolean();
					break;
				case 2:
					error = reader.ReadString();
					break;
				case 3:
					token = reader.ReadString();
					break;
				case 4:
					messages = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 5:
					actions = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 6:
					authAccepted = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 7:
					authRejected = formatterResolver.GetFormatterWithVerify<AuthenticatiorAuthReject[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 8:
					verificationChallenge = reader.ReadString();
					break;
				case 9:
					verificationResponse = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new AuthenticatorResponse(success, verified, error, token, messages, actions, authAccepted, authRejected, verificationChallenge, verificationResponse);
		}
	}
}
