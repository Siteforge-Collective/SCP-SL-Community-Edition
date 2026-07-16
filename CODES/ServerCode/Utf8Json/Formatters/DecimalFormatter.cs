namespace Utf8Json.Formatters
{
	public sealed class DecimalFormatter : global::Utf8Json.IJsonFormatter<decimal>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<decimal> Default = new global::Utf8Json.Formatters.DecimalFormatter();

		private readonly bool serializeAsString;

		public DecimalFormatter()
			: this(serializeAsString: false)
		{
		}

		public DecimalFormatter(bool serializeAsString)
		{
			this.serializeAsString = serializeAsString;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, decimal value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (serializeAsString)
			{
				writer.WriteString(value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
			}
			else
			{
				writer.WriteRaw(global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes(value.ToString(global::System.Globalization.CultureInfo.InvariantCulture)));
			}
		}

		public decimal Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::Utf8Json.JsonToken currentJsonToken = reader.GetCurrentJsonToken();
			switch (currentJsonToken)
			{
			case global::Utf8Json.JsonToken.Number:
			{
				global::System.ArraySegment<byte> arraySegment = reader.ReadNumberSegment();
				return decimal.Parse(global::Utf8Json.Internal.StringEncoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count), global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture);
			}
			case global::Utf8Json.JsonToken.String:
				return decimal.Parse(reader.ReadString(), global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture);
			default:
				throw new global::System.InvalidOperationException("Invalid Json Token for DecimalFormatter:" + currentJsonToken);
			}
		}
	}
}
