namespace Utf8Json.Unity
{
	public sealed class BoundsFormatter : global::Utf8Json.IJsonFormatter<global::UnityEngine.Bounds>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public BoundsFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("center"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("size"),
					1
				}
			};
			____stringByteKeys = new byte[2][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("center"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("size")
			};
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::UnityEngine.Bounds value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref writer, value.center, formatterResolver);
			writer.WriteRaw(____stringByteKeys[1]);
			formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref writer, value.size, formatterResolver);
			writer.WriteEndObject();
		}

		public global::UnityEngine.Bounds Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			global::UnityEngine.Vector3 center = default(global::UnityEngine.Vector3);
			global::UnityEngine.Vector3 size = default(global::UnityEngine.Vector3);
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
					center = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(ref reader, formatterResolver);
					break;
				case 1:
					size = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(ref reader, formatterResolver);
					break;
				default:
					reader.ReadNextBlock();
					break;
				}
			}
			global::UnityEngine.Bounds result = new global::UnityEngine.Bounds(center, size);
			result.center = center;
			result.size = size;
			return result;
		}
	}
}
