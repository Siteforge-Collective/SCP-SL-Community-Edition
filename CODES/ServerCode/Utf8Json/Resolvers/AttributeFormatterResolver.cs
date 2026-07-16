namespace Utf8Json.Resolvers
{
	public sealed class AttributeFormatterResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				global::Utf8Json.JsonFormatterAttribute customAttribute = global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::Utf8Json.JsonFormatterAttribute>(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T)));
				if (customAttribute == null)
				{
					return;
				}
				try
				{
					if (customAttribute.FormatterType.IsGenericType && !global::System.Reflection.ReflectionExtensions.IsConstructedGenericType(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(customAttribute.FormatterType)))
					{
						formatter = (global::Utf8Json.IJsonFormatter<T>)global::System.Activator.CreateInstance(customAttribute.FormatterType.MakeGenericType(typeof(T)), customAttribute.Arguments);
					}
					else
					{
						formatter = (global::Utf8Json.IJsonFormatter<T>)global::System.Activator.CreateInstance(customAttribute.FormatterType, customAttribute.Arguments);
					}
				}
				catch (global::System.Exception innerException)
				{
					throw new global::System.InvalidOperationException("Can not create formatter from JsonFormatterAttribute, check the target formatter is public and has constructor with right argument. FormatterType:" + customAttribute.FormatterType.Name, innerException);
				}
			}
		}

		public static global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.AttributeFormatterResolver();

		private AttributeFormatterResolver()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.AttributeFormatterResolver.FormatterCache<T>.formatter;
		}
	}
}
