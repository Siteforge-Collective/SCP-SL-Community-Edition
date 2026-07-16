namespace Utf8Json.Resolvers
{
	public sealed class DynamicGenericResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				formatter = (global::Utf8Json.IJsonFormatter<T>)global::Utf8Json.Resolvers.Internal.DynamicGenericResolverGetFormatterHelper.GetFormatter(typeof(T));
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.DynamicGenericResolver();

		private DynamicGenericResolver()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.DynamicGenericResolver.FormatterCache<T>.formatter;
		}
	}
}
