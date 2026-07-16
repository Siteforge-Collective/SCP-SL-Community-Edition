namespace Utf8Json.Formatters
{
	public sealed class ServerListItemFormatter : global::Utf8Json.IJsonFormatter<ServerListItem>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public ServerListItemFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("serverId"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("ip"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("port"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("players"),
					3
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("info"),
					4
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("pastebin"),
					5
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("version"),
					6
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("friendlyFire"),
					7
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("modded"),
					8
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("whitelist"),
					9
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("officialCode"),
					10
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("NameFilterPoints"),
					11
				}
			};
			____stringByteKeys = new byte[12][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("serverId"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ip"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("port"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("players"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("info"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("pastebin"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("version"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("friendlyFire"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("modded"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("whitelist"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("officialCode"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("NameFilterPoints")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, ServerListItem value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteUInt32(value.serverId);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteString(value.ip);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteUInt16(value.port);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteString(value.players);
			writer.WriteRaw(____stringByteKeys[4]);
			writer.WriteString(value.info);
			writer.WriteRaw(____stringByteKeys[5]);
			writer.WriteString(value.pastebin);
			writer.WriteRaw(____stringByteKeys[6]);
			writer.WriteString(value.version);
			writer.WriteRaw(____stringByteKeys[7]);
			writer.WriteBoolean(value.friendlyFire);
			writer.WriteRaw(____stringByteKeys[8]);
			writer.WriteBoolean(value.modded);
			writer.WriteRaw(____stringByteKeys[9]);
			writer.WriteBoolean(value.whitelist);
			writer.WriteRaw(____stringByteKeys[10]);
			writer.WriteByte(value.officialCode);
			writer.WriteRaw(____stringByteKeys[11]);
			writer.WriteInt32(value.NameFilterPoints);
			writer.WriteEndObject();
		}

		public ServerListItem Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			uint serverId = 0u;
			string ip = null;
			ushort port = 0;
			string players = null;
			string info = null;
			string pastebin = null;
			string version = null;
			bool friendlyFire = false;
			bool modded = false;
			bool whitelist = false;
			byte officialCode = 0;
			int nameFilterPoints = 0;
			bool flag = false;
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
					serverId = reader.ReadUInt32();
					break;
				case 1:
					ip = reader.ReadString();
					break;
				case 2:
					port = reader.ReadUInt16();
					break;
				case 3:
					players = reader.ReadString();
					break;
				case 4:
					info = reader.ReadString();
					break;
				case 5:
					pastebin = reader.ReadString();
					break;
				case 6:
					version = reader.ReadString();
					break;
				case 7:
					friendlyFire = reader.ReadBoolean();
					break;
				case 8:
					modded = reader.ReadBoolean();
					break;
				case 9:
					whitelist = reader.ReadBoolean();
					break;
				case 10:
					officialCode = reader.ReadByte();
					break;
				case 11:
					nameFilterPoints = reader.ReadInt32();
					flag = true;
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			ServerListItem result = new ServerListItem(serverId, ip, port, players, info, pastebin, version, friendlyFire, modded, whitelist, officialCode);
			if (flag)
			{
				result.NameFilterPoints = nameFilterPoints;
			}
			return result;
		}
	}
}
