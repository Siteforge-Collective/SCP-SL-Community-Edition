namespace Utf8Json.Formatters
{
	public sealed class AnonymousFormatter<T> : global::Utf8Json.IJsonFormatter<T>, global::Utf8Json.IJsonFormatter
	{
		private readonly global::Utf8Json.JsonSerializeAction<T> serialize;

		private readonly global::Utf8Json.JsonDeserializeFunc<T> deserialize;

		public AnonymousFormatter(global::Utf8Json.JsonSerializeAction<T> serialize, global::Utf8Json.JsonDeserializeFunc<T> deserialize)
		{
			this.serialize = serialize;
			this.deserialize = deserialize;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, T value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (serialize == null)
			{
				throw new global::System.InvalidOperationException(GetType().Name + " does not support Serialize.");
			}
			serialize(ref writer, value, formatterResolver);
		}

		public T Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (deserialize == null)
			{
				throw new global::System.InvalidOperationException(GetType().Name + " does not support Deserialize.");
			}
			return deserialize(ref reader, formatterResolver);
		}
	}
}
