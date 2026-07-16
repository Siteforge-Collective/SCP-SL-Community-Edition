namespace Utf8Json.Unity
{
	public sealed class Vector3Formatter : global::Utf8Json.IJsonFormatter<global::UnityEngine.Vector3>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public Vector3Formatter()
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
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("z"),
					2
				}
			};
			____stringByteKeys = new byte[3][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("x"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("y"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("z")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::UnityEngine.Vector3 value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			writer.WriteSingle(value.x);
			writer.WriteRaw(____stringByteKeys[1]);
			writer.WriteSingle(value.y);
			writer.WriteRaw(____stringByteKeys[2]);
			writer.WriteSingle(value.z);
			writer.WriteEndObject();
		}

		public global::UnityEngine.Vector3 Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			float x = 0f;
			float y = 0f;
			float z = 0f;
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
					z = reader.ReadSingle();
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			global::UnityEngine.Vector3 result = new global::UnityEngine.Vector3(x, y, z);
			result.x = x;
			result.y = y;
			result.z = z;
			return result;
		}
	}
}
