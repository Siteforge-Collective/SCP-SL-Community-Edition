namespace Utf8Json.Unity
{
	public sealed class ColorFormatter : global::Utf8Json.IJsonFormatter<global::UnityEngine.Color>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public ColorFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("r"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("g"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("b"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("a"),
					3
				}
			};
			____stringByteKeys = new byte[4][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("r"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("g"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("b"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("a")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::UnityEngine.Color value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteSingle(value.r);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteSingle(value.g);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteSingle(value.b);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteSingle(value.a);
			writer.WriteEndObject();
		}

		public global::UnityEngine.Color Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			float r = 0f;
			float g = 0f;
			float b = 0f;
			float a = 0f;
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
					r = reader.ReadSingle();
					break;
				case 1:
					g = reader.ReadSingle();
					break;
				case 2:
					b = reader.ReadSingle();
					break;
				case 3:
					a = reader.ReadSingle();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			global::UnityEngine.Color result = new global::UnityEngine.Color(r, g, b, a);
			result.r = r;
			result.g = g;
			result.b = b;
			result.a = a;
			return result;
		}
	}
}
