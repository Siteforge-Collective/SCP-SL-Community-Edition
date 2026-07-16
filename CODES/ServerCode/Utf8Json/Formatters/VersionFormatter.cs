namespace Utf8Json.Formatters
{
	public sealed class VersionFormatter : global::Utf8Json.IJsonFormatter<global::System.Version>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.Version> Default = new global::Utf8Json.Formatters.VersionFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Version value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteString(value.ToString());
			}
		}

		public global::System.Version Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return new global::System.Version(reader.ReadString());
		}
	}
}
