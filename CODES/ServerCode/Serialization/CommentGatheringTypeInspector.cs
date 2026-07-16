namespace Serialization
{
	public class CommentGatheringTypeInspector : global::YamlDotNet.Serialization.TypeInspectors.TypeInspectorSkeleton
	{
		private readonly global::YamlDotNet.Serialization.ITypeInspector _innerTypeDescriptor;

		public CommentGatheringTypeInspector(global::YamlDotNet.Serialization.ITypeInspector innerTypeDescriptor)
		{
			_innerTypeDescriptor = innerTypeDescriptor ?? throw new global::System.ArgumentNullException("innerTypeDescriptor");
		}

		public override global::System.Collections.Generic.IEnumerable<global::YamlDotNet.Serialization.IPropertyDescriptor> GetProperties(global::System.Type type, object container)
		{
			return global::System.Linq.Enumerable.Select(_innerTypeDescriptor.GetProperties(type, container), (global::YamlDotNet.Serialization.IPropertyDescriptor descriptor) => new global::Serialization.CommentsPropertyDescriptor(descriptor));
		}
	}
}
