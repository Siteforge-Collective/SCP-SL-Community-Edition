namespace Utf8Json.Formatters
{
	public sealed class CreditsListCategoryFormatter : global::Utf8Json.IJsonFormatter<CreditsListCategory>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public CreditsListCategoryFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("category"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("members"),
					1
				}
			};
			____stringByteKeys = new byte[2][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("category"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("members")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, CreditsListCategory value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.category);
			writer.WriteRaw(____stringByteKeys[1]);
			formatterResolver.GetFormatterWithVerify<CreditsListMember[]>().Serialize(ref writer, value.members, formatterResolver);
			writer.WriteEndObject();
		}

		public CreditsListCategory Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string category = null;
			CreditsListMember[] members = null;
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
					category = reader.ReadString();
					break;
				case 1:
					members = formatterResolver.GetFormatterWithVerify<CreditsListMember[]>().Deserialize(ref reader, formatterResolver);
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new CreditsListCategory(category, members);
		}
	}
}
