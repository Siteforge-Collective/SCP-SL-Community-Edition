namespace Utf8Json.Formatters
{
	public sealed class InterfaceGroupingFormatter<TKey, TElement> : global::Utf8Json.IJsonFormatter<global::System.Linq.IGrouping<TKey, TElement>>, global::Utf8Json.IJsonFormatter
	{
		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Linq.IGrouping<TKey, TElement> value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteRaw(global::Utf8Json.Formatters.Internal.CollectionFormatterHelper.groupingName[0]);
			formatterResolver.GetFormatterWithVerify<TKey>().Serialize(ref writer, value.Key, formatterResolver);
			writer.WriteRaw(global::Utf8Json.Formatters.Internal.CollectionFormatterHelper.groupingName[1]);
			formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<TElement>>().Serialize(ref writer, global::System.Linq.Enumerable.AsEnumerable(value), formatterResolver);
			writer.WriteEndObject();
		}

		public global::System.Linq.IGrouping<TKey, TElement> Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			TKey key = default(TKey);
			global::System.Collections.Generic.IEnumerable<TElement> elements = null;
			reader.ReadIsBeginObjectWithVerify();
			int count = 0;
			while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref count))
			{
				global::System.ArraySegment<byte> key2 = reader.ReadPropertyNameSegmentRaw();
				global::Utf8Json.Formatters.Internal.CollectionFormatterHelper.groupingAutomata.TryGetValueSafe(key2, out var value);
				switch (value)
				{
				case 0:
					key = formatterResolver.GetFormatterWithVerify<TKey>().Deserialize(ref reader, formatterResolver);
					break;
				case 1:
					elements = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<TElement>>().Deserialize(ref reader, formatterResolver);
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			return new global::Utf8Json.Formatters.Grouping<TKey, TElement>(key, elements);
		}
	}
}
