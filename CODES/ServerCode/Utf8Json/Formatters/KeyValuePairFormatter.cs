namespace Utf8Json.Formatters
{
	public sealed class KeyValuePairFormatter<TKey, TValue> : global::Utf8Json.IJsonFormatter<global::System.Collections.Generic.KeyValuePair<TKey, TValue>>, global::Utf8Json.IJsonFormatter
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Collections.Generic.KeyValuePair<TKey, TValue> value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(global::Utf8Json.Formatters.Internal.StandardClassLibraryFormatterHelper.keyValuePairName[0]);
			formatterResolver.GetFormatterWithVerify<TKey>().Serialize(ref writer, value.Key, formatterResolver);
			writer.WriteRaw(global::Utf8Json.Formatters.Internal.StandardClassLibraryFormatterHelper.keyValuePairName[1]);
			formatterResolver.GetFormatterWithVerify<TValue>().Serialize(ref writer, value.Value, formatterResolver);
			writer.WriteEndObject();
		}

		public global::System.Collections.Generic.KeyValuePair<TKey, TValue> Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("Data is Nil, KeyValuePair can not be null.");
			}
			TKey key = default(TKey);
			TValue value = default(TValue);
			reader.ReadIsBeginObjectWithVerify();
			int count = 0;
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				global::System.ArraySegment<byte> key2 = reader.ReadPropertyNameSegmentRaw();
				global::Utf8Json.Formatters.Internal.StandardClassLibraryFormatterHelper.keyValuePairAutomata.TryGetValueSafe(key2, out var value2);
				switch (value2)
				{
				case 0:
					key = formatterResolver.GetFormatterWithVerify<TKey>().Deserialize(ref reader, formatterResolver);
					break;
				case 1:
					value = formatterResolver.GetFormatterWithVerify<TValue>().Deserialize(ref reader, formatterResolver);
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new global::System.Collections.Generic.KeyValuePair<TKey, TValue>(key, value);
		}
	}
}
