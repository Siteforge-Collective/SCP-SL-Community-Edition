namespace Utf8Json
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Struct | global::System.AttributeTargets.Property | global::System.AttributeTargets.Field | global::System.AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public class JsonFormatterAttribute : global::System.Attribute
	{
		public global::System.Type FormatterType { get; private set; }

		public object[] Arguments { get; private set; }

		public JsonFormatterAttribute(global::System.Type formatterType)
		{
			FormatterType = formatterType;
		}

		public JsonFormatterAttribute(global::System.Type formatterType, params object[] arguments)
		{
			FormatterType = formatterType;
			Arguments = arguments;
		}
	}
}
