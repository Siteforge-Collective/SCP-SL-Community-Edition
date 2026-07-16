namespace Hints
{
	public class SByteHintParameter : global::Hints.PrimitiveHintParameter<sbyte>
	{
		public static global::Hints.SByteHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.SByteHintParameter sByteHintParameter = new global::Hints.SByteHintParameter();
			sByteHintParameter.Deserialize(reader);
			return sByteHintParameter;
		}

		protected SByteHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, sbyte>)global::Mirror.NetworkReaderExtensions.ReadSByte, (global::System.Action<global::Mirror.NetworkWriter, sbyte>)global::Mirror.NetworkWriterExtensions.WriteSByte)
		{
		}

		public SByteHintParameter(sbyte value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, sbyte>)global::Mirror.NetworkReaderExtensions.ReadSByte, (global::System.Action<global::Mirror.NetworkWriter, sbyte>)global::Mirror.NetworkWriterExtensions.WriteSByte)
		{
		}
	}
}
