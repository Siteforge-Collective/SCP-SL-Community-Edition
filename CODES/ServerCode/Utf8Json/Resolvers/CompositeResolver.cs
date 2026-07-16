namespace Utf8Json.Resolvers
{
	public sealed class CompositeResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				isFreezed = true;
				global::Utf8Json.IJsonFormatter[] formatters = global::Utf8Json.Resolvers.CompositeResolver.formatters;
				foreach (global::Utf8Json.IJsonFormatter jsonFormatter in formatters)
				{
					foreach (global::System.Type implementedInterface in global::System.Reflection.IntrospectionExtensions.GetTypeInfo(jsonFormatter.GetType()).ImplementedInterfaces)
					{
						global::System.Reflection.TypeInfo typeInfo = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(implementedInterface);
						if (typeInfo.IsGenericType && typeInfo.GenericTypeArguments[0] == typeof(T))
						{
							formatter = (global::Utf8Json.IJsonFormatter<T>)jsonFormatter;
							return;
						}
					}
				}
				global::Utf8Json.IJsonFormatterResolver[] resolvers = global::Utf8Json.Resolvers.CompositeResolver.resolvers;
				for (int i = 0; i < resolvers.Length; i++)
				{
					global::Utf8Json.IJsonFormatter<T> jsonFormatter2 = resolvers[i].GetFormatter<T>();
					if (jsonFormatter2 != null)
					{
						formatter = jsonFormatter2;
						break;
					}
				}
			}
		}

		public static readonly global::Utf8Json.Resolvers.CompositeResolver Instance = new global::Utf8Json.Resolvers.CompositeResolver();

		private static bool isFreezed = false;

		private static global::Utf8Json.IJsonFormatter[] formatters = new global::Utf8Json.IJsonFormatter[0];

		private static global::Utf8Json.IJsonFormatterResolver[] resolvers = new global::Utf8Json.IJsonFormatterResolver[0];

		private CompositeResolver()
		{
		}

		public static void Register(params global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			if (isFreezed)
			{
				throw new global::System.InvalidOperationException("Register must call on startup(before use GetFormatter<T>).");
			}
			global::Utf8Json.Resolvers.CompositeResolver.resolvers = resolvers;
		}

		public static void Register(params global::Utf8Json.IJsonFormatter[] formatters)
		{
			if (isFreezed)
			{
				throw new global::System.InvalidOperationException("Register must call on startup(before use GetFormatter<T>).");
			}
			global::Utf8Json.Resolvers.CompositeResolver.formatters = formatters;
		}

		public static void Register(global::Utf8Json.IJsonFormatter[] formatters, global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			if (isFreezed)
			{
				throw new global::System.InvalidOperationException("Register must call on startup(before use GetFormatter<T>).");
			}
			global::Utf8Json.Resolvers.CompositeResolver.resolvers = resolvers;
			global::Utf8Json.Resolvers.CompositeResolver.formatters = formatters;
		}

		public static void RegisterAndSetAsDefault(params global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			Register(resolvers);
			global::Utf8Json.JsonSerializer.SetDefaultResolver(Instance);
		}

		public static void RegisterAndSetAsDefault(params global::Utf8Json.IJsonFormatter[] formatters)
		{
			Register(formatters);
			global::Utf8Json.JsonSerializer.SetDefaultResolver(Instance);
		}

		public static void RegisterAndSetAsDefault(global::Utf8Json.IJsonFormatter[] formatters, global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			Register(formatters);
			Register(resolvers);
			global::Utf8Json.JsonSerializer.SetDefaultResolver(Instance);
		}

		public static global::Utf8Json.IJsonFormatterResolver Create(params global::Utf8Json.IJsonFormatter[] formatters)
		{
			return Create(formatters, new global::Utf8Json.IJsonFormatterResolver[0]);
		}

		public static global::Utf8Json.IJsonFormatterResolver Create(params global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			return Create(new global::Utf8Json.IJsonFormatter[0], resolvers);
		}

		public static global::Utf8Json.IJsonFormatterResolver Create(global::Utf8Json.IJsonFormatter[] formatters, global::Utf8Json.IJsonFormatterResolver[] resolvers)
		{
			return global::Utf8Json.Resolvers.DynamicCompositeResolver.Create(formatters, resolvers);
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.CompositeResolver.FormatterCache<T>.formatter;
		}
	}
}
