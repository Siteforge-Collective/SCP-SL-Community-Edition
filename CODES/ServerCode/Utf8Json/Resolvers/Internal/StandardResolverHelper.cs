namespace Utf8Json.Resolvers.Internal
{
	internal static class StandardResolverHelper
	{
		internal static readonly global::Utf8Json.IJsonFormatterResolver[] CompositeResolverBase = new global::Utf8Json.IJsonFormatterResolver[5]
		{
			global::Utf8Json.Resolvers.BuiltinResolver.Instance,
			global::Utf8Json.Unity.UnityResolver.Instance,
			global::Utf8Json.Resolvers.EnumResolver.Default,
			global::Utf8Json.Resolvers.DynamicGenericResolver.Instance,
			global::Utf8Json.Resolvers.AttributeFormatterResolver.Instance
		};
	}
}
