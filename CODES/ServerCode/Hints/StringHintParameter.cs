namespace Hints
{
	public class StringHintParameter : global::Hints.PrimitiveHintParameter<string>
	{
		public static global::Hints.StringHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.StringHintParameter stringHintParameter = new global::Hints.StringHintParameter();
			stringHintParameter.Deserialize(reader);
			return stringHintParameter;
		}

		protected StringHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, string>)global::Mirror.NetworkReaderExtensions.ReadString, (global::System.Action<global::Mirror.NetworkWriter, string>)global::Mirror.NetworkWriterExtensions.WriteString)
		{
		}

		public StringHintParameter(string value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, string>)global::Mirror.NetworkReaderExtensions.ReadString, (global::System.Action<global::Mirror.NetworkWriter, string>)global::Mirror.NetworkWriterExtensions.WriteString)
		{
		}
	}
}
