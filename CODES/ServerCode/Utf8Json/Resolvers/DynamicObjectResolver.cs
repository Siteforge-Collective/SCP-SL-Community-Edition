namespace Utf8Json.Resolvers
{
	public static class DynamicObjectResolver
	{
		public static readonly global::Utf8Json.IJsonFormatterResolver Default = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateOriginal.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver CamelCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateCamelCase.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver SnakeCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullFalseNameMutateSnakeCase.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver ExcludeNull = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullTrueNameMutateOriginal.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver ExcludeNullCamelCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullTrueNameMutateCamelCase.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver ExcludeNullSnakeCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateFalseExcludeNullTrueNameMutateSnakeCase.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivate = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullFalseNameMutateOriginal.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateCamelCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullFalseNameMutateCamelCase.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateSnakeCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullFalseNameMutateSnakeCase.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateExcludeNull = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullTrueNameMutateOriginal.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateExcludeNullCamelCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullTrueNameMutateCamelCase.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateExcludeNullSnakeCase = global::Utf8Json.Resolvers.Internal.DynamicObjectResolverAllowPrivateTrueExcludeNullTrueNameMutateSnakeCase.Instance;
	}
}
