namespace Utf8Json.Resolvers.Internal
{
	internal sealed class DynamicObjectResolverAllowPrivateTrueExcludeNullTrueNameMutateCamelCase : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				formatter = (global::Utf8Json.IJsonFormatter<T>)global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.BuildFormatterToDynamicMethod<T>(Instance, nameMutator, excludeNull, allowPrivate: true);
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullTrueNameMutateCamelCase();

		private static readonly global::System.Func<string, string> nameMutator = global::Utf8Json.Internal.StringMutator.ToCamelCase;

		private static readonly bool excludeNull = true;

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullTrueNameMutateCamelCase.FormatterCache<T>.formatter;
		}
	}
}
