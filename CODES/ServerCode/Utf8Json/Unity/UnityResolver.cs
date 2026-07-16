namespace Utf8Json.Unity
{
	public class UnityResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				object obj = global::Utf8Json.Unity.UnityResolverGetFormatterHelper.GetFormatter(typeof(T));
				if (obj != null)
				{
					formatter = (global::Utf8Json.IJsonFormatter<T>)obj;
				}
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Unity.UnityResolver();

		private UnityResolver()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Unity.UnityResolver.FormatterCache<T>.formatter;
		}
	}
}
