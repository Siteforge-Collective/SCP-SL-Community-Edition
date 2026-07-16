namespace Utf8Json.Resolvers
{
	public static class StandardResolver
	{
		public static readonly global::Utf8Json.IJsonFormatterResolver Default = global::Utf8Json.Resolvers.Internal.DefaultStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver CamelCase = global::Utf8Json.Resolvers.Internal.CamelCaseStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver SnakeCase = global::Utf8Json.Resolvers.Internal.SnakeCaseStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver ExcludeNull = global::Utf8Json.Resolvers.Internal.ExcludeNullStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver ExcludeNullCamelCase = global::Utf8Json.Resolvers.Internal.ExcludeNullCamelCaseStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver ExcludeNullSnakeCase = global::Utf8Json.Resolvers.Internal.ExcludeNullSnakeCaseStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivate = global::Utf8Json.Resolvers.Internal.AllowPrivateStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateCamelCase = global::Utf8Json.Resolvers.Internal.AllowPrivateCamelCaseStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateSnakeCase = global::Utf8Json.Resolvers.Internal.AllowPrivateSnakeCaseStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateExcludeNull = global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateExcludeNullCamelCase = global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullCamelCaseStandardResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver AllowPrivateExcludeNullSnakeCase = global::Utf8Json.Resolvers.Internal.AllowPrivateExcludeNullSnakeCaseStandardResolver.Instance;
	}
}
