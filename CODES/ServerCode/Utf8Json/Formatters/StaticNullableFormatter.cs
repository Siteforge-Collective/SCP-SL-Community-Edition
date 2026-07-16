namespace Utf8Json.Formatters
{
	public sealed class StaticNullableFormatter<T> : global::Utf8Json.IJsonFormatter<T?>, global::Utf8Json.IJsonFormatter where T : struct
	{
		private readonly global::Utf8Json.IJsonFormatter<T> underlyingFormatter;

		public StaticNullableFormatter(global::Utf8Json.IJsonFormatter<T> underlyingFormatter)
		{
			this.underlyingFormatter = underlyingFormatter;
		}

		public StaticNullableFormatter(global::System.Type formatterType, object[] formatterArguments)
		{
			try
			{
				underlyingFormatter = (global::Utf8Json.IJsonFormatter<T>)global::System.Activator.CreateInstance(formatterType, formatterArguments);
			}
			catch (global::System.Exception innerException)
			{
				throw new global::System.InvalidOperationException("Can not create formatter from JsonFormatterAttribute, check the target formatter is public and has constructor with right argument. FormatterType:" + formatterType.Name, innerException);
			}
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, T? value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (!value.HasValue)
			{
				writer.WriteNull();
			}
			else
			{
				underlyingFormatter.Serialize(ref writer, value.Value, formatterResolver);
			}
		}

		public T? Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			return underlyingFormatter.Deserialize(ref reader, formatterResolver);
		}
	}
}
