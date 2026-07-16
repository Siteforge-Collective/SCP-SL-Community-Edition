namespace Utf8Json.Unity
{
	public sealed class Vector2Formatter : global::Utf8Json.IJsonFormatter<global::UnityEngine.Vector2>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public Vector2Formatter()
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
				}
			};
			____stringByteKeys = new byte[2][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("x"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("y")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::UnityEngine.Vector2 value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteSingle(value.x);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteSingle(value.y);
			writer.WriteEndObject();
		}

		public global::UnityEngine.Vector2 Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			float x = 0f;
			float y = 0f;
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
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			global::UnityEngine.Vector2 result = new global::UnityEngine.Vector2(x, y);
			result.x = x;
			result.y = y;
			return result;
		}
	}
}
