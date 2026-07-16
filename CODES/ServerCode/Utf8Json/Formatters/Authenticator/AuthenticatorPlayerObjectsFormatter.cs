namespace Utf8Json.Formatters.Authenticator
{
	public sealed class AuthenticatorPlayerObjectsFormatter : global::Utf8Json.IJsonFormatter<global::Authenticator.AuthenticatorPlayerObjects>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;

		private readonly byte[][] ____stringByteKeys;

		public AuthenticatorPlayerObjectsFormatter()
		{
			____keyMapping = new global::Utf8Json.Internal.AutomataDictionary { 
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("objects"),
				0
			} };
			____stringByteKeys = new byte[1][] { global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("objects") };
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::Authenticator.AuthenticatorPlayerObjects value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			writer.WriteRaw(____stringByteKeys[0]);
			formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Authenticator.AuthenticatorPlayerObject>>().Serialize(ref writer, value.objects, formatterResolver);
			writer.WriteEndObject();
		}

		public global::Authenticator.AuthenticatorPlayerObjects Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				throw new global::System.InvalidOperationException("typecode is null, struct not supported");
			}
			global::System.Collections.Generic.List<global::Authenticator.AuthenticatorPlayerObject> objects = null;
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
					objects = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Authenticator.AuthenticatorPlayerObject>>().Deserialize(ref reader, formatterResolver);
				}
				else
				{
					reader.ReadNextBlock();
				}
			}
			return new global::Authenticator.AuthenticatorPlayerObjects(objects);
		}
	}
}
