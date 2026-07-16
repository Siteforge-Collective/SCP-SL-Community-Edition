namespace Utf8Json.Resolvers.Internal
{
	internal sealed class EnumUnderlyingValueResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T));
				if (global::Utf8Json.Internal.ReflectionExtensions.IsNullable(typeInfo))
				{
					typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeInfo.GenericTypeArguments[0]);
					if (typeInfo.IsEnum)
					{
						object formatterDynamic = Instance.GetFormatterDynamic(typeInfo.AsType());
						if (formatterDynamic != null)
						{
							formatter = (global::Utf8Json.IJsonFormatter<T>)global::System.Activator.CreateInstance(typeof(global::Utf8Json.Formatters.StaticNullableFormatter<>).MakeGenericType(typeInfo.AsType()), formatterDynamic);
						}
					}
				}
				else if (typeof(T).IsEnum)
				{
					formatter = new global::Utf8Json.Formatters.EnumFormatter<T>(serializeByName: false);
				}
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.Internal.EnumUnderlyingValueResolver();

		private EnumUnderlyingValueResolver()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.Internal.EnumUnderlyingValueResolver.FormatterCache<T>.formatter;
		}
	}
}
