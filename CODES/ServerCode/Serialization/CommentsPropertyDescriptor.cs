namespace Serialization
{
	internal sealed class CommentsPropertyDescriptor : global::YamlDotNet.Serialization.IPropertyDescriptor
	{
		private readonly global::YamlDotNet.Serialization.IPropertyDescriptor _baseDescriptor;

		public string Name { get; set; }

		public global::System.Type Type => _baseDescriptor.Type;

		public global::System.Type TypeOverride
		{
			get
			{
				return _baseDescriptor.TypeOverride;
			}
			set
			{
				_baseDescriptor.TypeOverride = value;
			}
		}

		public int Order { get; set; }

		public global::YamlDotNet.Core.ScalarStyle ScalarStyle
		{
			get
			{
				return _baseDescriptor.ScalarStyle;
			}
			set
			{
				_baseDescriptor.ScalarStyle = value;
			}
		}

		public bool CanWrite => _baseDescriptor.CanWrite;

		public CommentsPropertyDescriptor(global::YamlDotNet.Serialization.IPropertyDescriptor baseDescriptor)
		{
			_baseDescriptor = baseDescriptor;
			Name = baseDescriptor.Name;
		}

		public void Write(object target, object value)
		{
			_baseDescriptor.Write(target, value);
		}

		public T GetCustomAttribute<T>() where T : global::System.Attribute
		{
			return _baseDescriptor.GetCustomAttribute<T>();
		}

		public global::YamlDotNet.Serialization.IObjectDescriptor Read(object target)
		{
			global::System.ComponentModel.DescriptionAttribute customAttribute = _baseDescriptor.GetCustomAttribute<global::System.ComponentModel.DescriptionAttribute>();
			if (customAttribute == null)
			{
				return _baseDescriptor.Read(target);
			}
			return new global::Serialization.CommentsObjectDescriptor(_baseDescriptor.Read(target), customAttribute.Description);
		}
	}
}
