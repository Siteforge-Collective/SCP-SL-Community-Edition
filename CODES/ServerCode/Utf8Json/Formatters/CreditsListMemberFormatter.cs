namespace Utf8Json.Formatters
{
	public sealed class CreditsListMemberFormatter : global::Utf8Json.IJsonFormatter<CreditsListMember>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public CreditsListMemberFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("color"),
					2
				}
			};
			____stringByteKeys = new byte[3][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("name"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("color")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, CreditsListMember value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.name);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.title);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteString(value.color);
			writer.WriteEndObject();
		}

		public CreditsListMember Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string name = null;
			string title = null;
			string color = null;
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
					name = reader.ReadString();
					break;
				case 1:
					title = reader.ReadString();
					break;
				case 2:
					color = reader.ReadString();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new CreditsListMember(name, title, color);
		}
	}
}
