namespace Utf8Json.Resolvers.Internal
{
	internal sealed class DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				formatter = (global::Utf8Json.IJsonFormatter<T>)global::Utf8Json.Resolvers.Internal.DynamicObjectTypeBuilder.BuildFormatterToAssembly<T>(assembly, Instance, nameMutator, excludeNull);
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance;

		private static readonly global::System.Func<string, string> nameMutator;

		private static readonly bool excludeNull;

		private const string ModuleName = "Utf8Json.Resolvers.DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase";

		private static readonly global::Utf8Json.Internal.Emit.DynamicAssembly assembly;

		static DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase()
		{
			Instance = new global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase();
			nameMutator = global::Utf8Json.Internal.StringMutator.ToSnakeCase;
			excludeNull = false;
			assembly = new global::Utf8Json.Internal.Emit.DynamicAssembly("Utf8Json.Resolvers.DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase");
		}

		private DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase.FormatterCache<T>.formatter;
		}
	}
}
