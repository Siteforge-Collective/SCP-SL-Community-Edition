namespace Utf8Json.Resolvers.Internal
{
	internal class DynamicMethodAnonymousFormatter<T> : global::Utf8Json.IJsonFormatter<T>, global::Utf8Json.IJsonFormatter
	{
		private readonly byte[][] stringByteKeysField;

		private readonly object[] serializeCustomFormatters;

		private readonly object[] deserializeCustomFormatters;

		private readonly global::Utf8Json.Resolvers.Internal.AnonymousJsonSerializeAction<T> serialize;

		private readonly global::Utf8Json.Resolvers.Internal.AnonymousJsonDeserializeFunc<T> deserialize;

		public DynamicMethodAnonymousFormatter(byte[][] stringByteKeysField, object[] serializeCustomFormatters, object[] deserializeCustomFormatters, global::Utf8Json.Resolvers.Internal.AnonymousJsonSerializeAction<T> serialize, global::Utf8Json.Resolvers.Internal.AnonymousJsonDeserializeFunc<T> deserialize)
		{
			this.stringByteKeysField = stringByteKeysField;
			this.serializeCustomFormatters = serializeCustomFormatters;
			this.deserializeCustomFormatters = deserializeCustomFormatters;
			this.serialize = serialize;
			this.deserialize = deserialize;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (serialize == null)
			{
				throw new global::System.InvalidOperationException(GetType().Name + " does not support Serialize.");
			}
			serialize(stringByteKeysField, serializeCustomFormatters, ref writer, value, formatterResolver);
		}

		public T Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (deserialize == null)
			{
				throw new global::System.InvalidOperationException(GetType().Name + " does not support Deserialize.");
			}
			return deserialize(deserializeCustomFormatters, ref reader, formatterResolver);
		}
	}
}
