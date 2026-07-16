namespace Serialization
{
	internal sealed class CommentsObjectDescriptor : global::YamlDotNet.Serialization.IObjectDescriptor
	{
		private readonly global::YamlDotNet.Serialization.IObjectDescriptor _innerDescriptor;

		public string Comment { get; private set; }

		public object Value => _innerDescriptor.Value;

		public global::System.Type Type => _innerDescriptor.Type;

		public global::System.Type StaticType => _innerDescriptor.StaticType;

		public global::YamlDotNet.Core.ScalarStyle ScalarStyle => _innerDescriptor.ScalarStyle;

		public CommentsObjectDescriptor(global::YamlDotNet.Serialization.IObjectDescriptor innerDescriptor, string comment)
		{
			_innerDescriptor = innerDescriptor;
			Comment = comment;
		}
	}
}
