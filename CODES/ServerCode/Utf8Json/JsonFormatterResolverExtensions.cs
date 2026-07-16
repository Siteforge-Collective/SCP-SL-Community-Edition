namespace Utf8Json
{
	public static class JsonFormatterResolverExtensions
	{
		public static global::Utf8Json.IJsonFormatter<T> GetFormatterWithVerify<T>(this global::Utf8Json.IJsonFormatterResolver resolver)
		{
			global::Utf8Json.IJsonFormatter<T> formatter;
			try
			{
				formatter = resolver.GetFormatter<T>();
			}
			catch (global::System.TypeInitializationException innerException)
			{
				while (innerException.InnerException != null)
				{
					innerException = (global::System.TypeInitializationException)innerException.InnerException;
				}
				throw innerException;
			}
			if (formatter == null)
			{
				throw new global::Utf8Json.FormatterNotRegisteredException(typeof(T).FullName + " is not registered in this resolver. resolver:" + resolver.GetType().Name);
			}
			return formatter;
		}

		public static object GetFormatterDynamic(this global::Utf8Json.IJsonFormatterResolver resolver, global::System.Type type)
		{
			return global::System.Reflection.RuntimeReflectionExtensions.GetRuntimeMethod(typeof(global::Utf8Json.IJsonFormatterResolver), "GetFormatter", global::System.Type.EmptyTypes).MakeGenericMethod(type).Invoke(resolver, null);
		}

		public static void DeserializeToWithFallbackReplace<T>(this global::Utf8Json.IJsonFormatterResolver formatterResolver, ref T value, ref global::Utf8Json.JsonReader reader)
		{
			global::Utf8Json.IJsonFormatter<T> formatterWithVerify = formatterResolver.GetFormatterWithVerify<T>();
			if (formatterWithVerify is global::Utf8Json.IOverwriteJsonFormatter<T> overwriteJsonFormatter)
			{
				overwriteJsonFormatter.DeserializeTo(ref value, ref reader, formatterResolver);
			}
			else
			{
				value = formatterWithVerify.Deserialize(ref reader, formatterResolver);
			}
		}
	}
}
