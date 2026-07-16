namespace Hints
{
	public class UShortHintParameter : global::Hints.PrimitiveHintParameter<ushort>
	{
		public static global::Hints.UShortHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.UShortHintParameter uShortHintParameter = new global::Hints.UShortHintParameter();
			uShortHintParameter.Deserialize(reader);
			return uShortHintParameter;
		}

		protected UShortHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, ushort>)global::Mirror.NetworkReaderExtensions.ReadUInt16, (global::System.Action<global::Mirror.NetworkWriter, ushort>)global::Mirror.NetworkWriterExtensions.WriteUInt16)
		{
		}

		public UShortHintParameter(ushort value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, ushort>)global::Mirror.NetworkReaderExtensions.ReadUInt16, (global::System.Action<global::Mirror.NetworkWriter, ushort>)global::Mirror.NetworkWriterExtensions.WriteUInt16)
		{
		}
	}
}
