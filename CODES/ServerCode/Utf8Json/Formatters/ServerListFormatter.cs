namespace Utf8Json.Formatters
{
	public sealed class ServerListFormatter : global::Utf8Json.IJsonFormatter<ServerList>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public ServerListFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary { 
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("servers"),
				0
			} };
			____stringByteKeys = new byte[1][] { global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("servers") };
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, ServerList value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			formatterResolver.GetFormatterWithVerify<ServerListItem[]>().Serialize(ref writer, value.servers, formatterResolver);
			writer.WriteEndObject();
		}

		public ServerList Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			ServerListItem[] servers = null;
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
					servers = formatterResolver.GetFormatterWithVerify<ServerListItem[]>().Deserialize(ref reader, formatterResolver);
				}
				else
				{
					reader.ReadNextBlock();
				}
			}
			return new ServerList(servers);
		}
	}
}
