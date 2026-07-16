namespace Utf8Json.Formatters
{
	public sealed class PublicKeyResponseFormatter : global::Utf8Json.IJsonFormatter<PublicKeyResponse>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public PublicKeyResponseFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("key"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("signature"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("credits"),
					2
				}
			};
			____stringByteKeys = new byte[3][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("key"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("signature"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("credits")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, PublicKeyResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.key);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.signature);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.credits);
			writer.WriteEndObject();
		}

		public PublicKeyResponse Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string key = null;
			string signature = null;
			string credits = null;
			int count = 0;
			reader.ReadIsBeginObjectWithVerify();
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				global::System.ArraySegment<byte> key2 = reader.ReadPropertyNameSegmentRaw();
				if (!____keyMapping.TryGetValueSafe(key2, out var value))
				{
					reader.ReadNextBlock();
					continue;
				}
				switch (value)
				{
				case 0:
					key = reader.ReadString();
					break;
				case 1:
					signature = reader.ReadString();
					break;
				case 2:
					credits = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new PublicKeyResponse(key, signature, credits);
		}
	}
}
