namespace Hints
{
	public class DoubleHintParameter : global::Hints.FormattablePrimitiveHintParameter<double>
	{
		public static global::Hints.DoubleHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.DoubleHintParameter doubleHintParameter = new global::Hints.DoubleHintParameter();
			doubleHintParameter.Deserialize(reader);
			return doubleHintParameter;
		}

		protected DoubleHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, double>)global::Mirror.NetworkReaderExtensions.ReadDouble, (global::System.Action<global::Mirror.NetworkWriter, double>)global::Mirror.NetworkWriterExtensions.WriteDouble)
		{
		}

		public DoubleHintParameter(double value, string format)
			: base(value, format, (global::System.Func<global::Mirror.NetworkReader, double>)global::Mirror.NetworkReaderExtensions.ReadDouble, (global::System.Action<global::Mirror.NetworkWriter, double>)global::Mirror.NetworkWriterExtensions.WriteDouble)
		{
		}
	}
}
