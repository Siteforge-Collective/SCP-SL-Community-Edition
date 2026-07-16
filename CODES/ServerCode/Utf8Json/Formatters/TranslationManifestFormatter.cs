namespace Utf8Json.Formatters
{
	public sealed class TranslationManifestFormatter : global::Utf8Json.IJsonFormatter<TranslationManifest>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public TranslationManifestFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Name"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Authors"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("InterfaceLocales"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("SystemLocales"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("ForcedFontOrder"),
					4
				}
			};
			____stringByteKeys = new byte[5][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("Name"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Authors"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("InterfaceLocales"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("SystemLocales"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ForcedFontOrder")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, TranslationManifest value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteString(value.Name);
			writer.WriteRaw(____stringByteKeys[1]);
			formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.Authors, formatterResolver);
			writer.WriteRaw(____stringByteKeys[2]);
			formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.InterfaceLocales, formatterResolver);
			writer.WriteRaw(____stringByteKeys[3]);
			formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.SystemLocales, formatterResolver);
			writer.WriteRaw(____stringByteKeys[4]);
			formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.ForcedFontOrder, formatterResolver);
			writer.WriteEndObject();
		}

		public TranslationManifest Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			string name = null;
			string[] authors = null;
			string[] interfaceLocales = null;
			string[] systemLocales = null;
			string[] forcedFontOrder = null;
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
					authors = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 2:
					interfaceLocales = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 3:
					systemLocales = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
					break;
				case 4:
					forcedFontOrder = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new TranslationManifest(name, authors, interfaceLocales, systemLocales, forcedFontOrder);
		}
	}
}
