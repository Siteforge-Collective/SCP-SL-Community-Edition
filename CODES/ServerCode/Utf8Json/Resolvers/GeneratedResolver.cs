namespace Utf8Json.Resolvers
{
	public class GeneratedResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				object obj = global::Utf8Json.Resolvers.GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
				if (obj != null)
				{
					formatter = (global::Utf8Json.IJsonFormatter<T>)obj;
				}
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.GeneratedResolver();

		private GeneratedResolver()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.GeneratedResolver.FormatterCache<T>.formatter;
		}
	}
}
