namespace Utf8Json.Unity
{
	public sealed class RectFormatter : global::Utf8Json.IJsonFormatter<global::UnityEngine.Rect>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public RectFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("x"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("y"),
					1
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("width"),
					2
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("height"),
					3
				}
			};
			____stringByteKeys = new byte[4][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("x"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("y"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("width"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("height")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::UnityEngine.Rect value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteSingle(value.x);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteSingle(value.y);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteSingle(value.width);
			writer.WriteRaw(____stringByteKeys[3]);
			writer.WriteSingle(value.height);
			writer.WriteEndObject();
		}

		public global::UnityEngine.Rect Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			float x = 0f;
			float y = 0f;
			float width = 0f;
			float height = 0f;
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
					x = reader.ReadSingle();
					break;
				case 1:
					y = reader.ReadSingle();
					break;
				case 2:
					width = reader.ReadSingle();
					break;
				case 3:
					height = reader.ReadSingle();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			global::UnityEngine.Rect result = new global::UnityEngine.Rect(x, y, width, height);
			result.x = x;
			result.y = y;
			result.width = width;
			result.height = height;
			return result;
		}
	}
}
