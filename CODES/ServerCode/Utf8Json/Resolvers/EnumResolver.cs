namespace Utf8Json.Resolvers
{
	public static class EnumResolver
	{
		public static readonly global::Utf8Json.IJsonFormatterResolver Default = global::Utf8Json.Resolvers.Internal.EnumDefaultResolver.Instance;

		public static readonly global::Utf8Json.IJsonFormatterResolver UnderlyingValue = global::Utf8Json.Resolvers.Internal.EnumUnderlyingValueResolver.Instance;
	}
}
