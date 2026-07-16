namespace Utf8Json.Formatters
{
	public sealed class DoubleArrayFormatter : global::Utf8Json.IJsonFormatter<double[]>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.DoubleArrayFormatter Default = new global::Utf8Json.Formatters.DoubleArrayFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, double[] value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteBeginArray();
			if (value.Length != 0)
			{
				writer.WriteDouble(value[0]);
			}
			for (int i = 1; i < value.Length; i++)
			{
				writer.WriteValueSeparator();
				writer.WriteDouble(value[i]);
			}
			writer.WriteEndArray();
		}

		public double[] Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			reader.ReadIsBeginArrayWithVerify();
			double[] array = new double[4];
			int count = 0;
			while (!reader.ReadIsEndArrayWithSkipValueSeparator(ref count))
			{
				if (array.Length < count)
				{
					global::System.Array.Resize(ref array, count * 2);
				}
				array[count - 1] = reader.ReadDouble();
			}
			global::System.Array.Resize(ref array, count);
			return array;
		}
	}
}
