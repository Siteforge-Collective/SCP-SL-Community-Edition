namespace Utf8Json.Resolvers.Internal
{
	internal sealed class AllowPrivateExcludeNullCamelCaseStandardResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				if (typeof(T) == typeof(object))
				{
					formatter = (global::Utf8Json.IJsonFormatter<T>)fallbackFormatter;
				}
				else
				{
					formatter = global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver.InnerResolver.Instance.GetFormatter<T>();
				}
			}
		}

		private sealed class InnerResolver : global::Utf8Json.IJsonFormatterResolver
		{
			private static class FormatterCache<T>
			{
				public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

				static FormatterCache()
				{
					global::Utf8Json.IJsonFormatterResolver[] resolvers = global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver.InnerResolver.resolvers;
					for (int i = 0; i < resolvers.Length; i++)
					{
						global::Utf8Json.IJsonFormatter<T> jsonFormatter = resolvers[i].GetFormatter<T>();
						if (jsonFormatter != null)
						{
							formatter = jsonFormatter;
							break;
						}
					}
				}
			}

			public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver.InnerResolver();

			private static readonly global::Utf8Json.IJsonFormatterResolver[] resolvers = global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Concat(global::Utf8Json.Resolvers.Internal.StandardResolverHelper.CompositeResolverBase, new global::Utf8Json.IJsonFormatterResolver[1] { global::Utf8Json.Resolvers.DynamicObjectResolver.AllowPrivateExcludeNullCamelCase }));

			private InnerResolver()
			{
			}

			public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
			{
				return global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver.InnerResolver.FormatterCache<T>.formatter;
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver();

		private static readonly global::Utf8Json.IJsonFormatter<object> fallbackFormatter = new global::Utf8Json.Formatters.DynamicObjectTypeFallbackFormatter(global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver.InnerResolver.Instance);

		private AllowPrivateExcludeNullCamelCaseStandardResolver()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver.FormatterCache<T>.formatter;
		}
	}
}
