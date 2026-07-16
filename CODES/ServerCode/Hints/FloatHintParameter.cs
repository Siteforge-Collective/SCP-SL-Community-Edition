namespace Hints
{
	public class FloatHintParameter : global::Hints.FormattablePrimitiveHintParameter<float>
	{
		public static global::Hints.FloatHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.FloatHintParameter floatHintParameter = new global::Hints.FloatHintParameter();
			floatHintParameter.Deserialize(reader);
			return floatHintParameter;
		}

		protected FloatHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, float>)global::Mirror.NetworkReaderExtensions.ReadSingle, (global::System.Action<global::Mirror.NetworkWriter, float>)global::Mirror.NetworkWriterExtensions.WriteSingle)
		{
		}

		public FloatHintParameter(float value, string format)
			: base(value, format, (global::System.Func<global::Mirror.NetworkReader, float>)global::Mirror.NetworkReaderExtensions.ReadSingle, (global::System.Action<global::Mirror.NetworkWriter, float>)global::Mirror.NetworkWriterExtensions.WriteSingle)
		{
		}
	}
}
