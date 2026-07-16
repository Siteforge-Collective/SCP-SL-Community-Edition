namespace Utf8Json.Formatters
{
	public sealed class AuthenticatiorAuthRejectFormatter : global::Utf8Json.IJsonFormatter<AuthenticatiorAuthReject>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public AuthenticatiorAuthRejectFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Id"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Reason"),
					1
				}
			};
			____stringByteKeys = new byte[2][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("Id"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Reason")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, AuthenticatiorAuthReject value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.Id);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.Reason);
			writer.WriteEndObject();
		}

		public AuthenticatiorAuthReject Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string id = null;
			string reason = null;
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
					reason = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new AuthenticatiorAuthReject(id, reason);
		}
	}
}
