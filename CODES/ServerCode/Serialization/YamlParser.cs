namespace Serialization
{
	public static class YamlParser
	{
		public static global::YamlDotNet.Serialization.ISerializer Serializer { get; } = new global::YamlDotNet.Serialization.SerializerBuilder().WithEmissionPhaseObjectGraphVisitor((global::YamlDotNet.Serialization.EmissionPhaseObjectGraphVisitorArgs visitor) => new global::Serialization.CommentsObjectGraphVisitor(visitor.InnerVisitor)).WithTypeInspector((global::YamlDotNet.Serialization.ITypeInspector typeInspector) => new global::Serialization.CommentGatheringTypeInspector(typeInspector)).WithNamingConvention(global::YamlDotNet.Serialization.NamingConventions.UnderscoredNamingConvention.Instance)
			.DisableAliases()
			.IgnoreFields()
			.Build();

		public static global::YamlDotNet.Serialization.IDeserializer Deserializer { get; } = new global::YamlDotNet.Serialization.DeserializerBuilder().WithNamingConvention(global::YamlDotNet.Serialization.NamingConventions.UnderscoredNamingConvention.Instance).IgnoreUnmatchedProperties().IgnoreFields()
			.Build();
	}
}
