namespace Utf8Json.Formatters
{
	public sealed class CreditsListFormatter : global::Utf8Json.IJsonFormatter<CreditsList>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public CreditsListFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary { 
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("credits"),
				0
			} };
			____stringByteKeys = new byte[1][] { global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("credits") };
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, CreditsList value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			formatterResolver.GetFormatterWithVerify<CreditsListCategory[]>().Serialize(ref writer, value.credits, formatterResolver);
			writer.WriteEndObject();
		}

		public CreditsList Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			CreditsListCategory[] credits = null;
			int count = 0;
			reader.ReadIsBeginObjectWithVerify();
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				global::System.ArraySegment<byte> key = reader.ReadPropertyNameSegmentRaw();
				if (!____keyMapping.TryGetValueSafe(key, out var value))
				{
					reader.ReadNextBlock();
				}
				else if (value == 0)
				{
					credits = formatterResolver.GetFormatterWithVerify<CreditsListCategory[]>().Deserialize(ref reader, formatterResolver);
				}
				else
				{
					reader.ReadNextBlock();
				}
			}
			return new CreditsList(credits);
		}
	}
}
