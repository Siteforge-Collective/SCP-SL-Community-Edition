namespace Hints
{
	public class UIntHintParameter : global::Hints.PrimitiveHintParameter<uint>
	{
		public static global::Hints.UIntHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.UIntHintParameter uIntHintParameter = new global::Hints.UIntHintParameter();
			uIntHintParameter.Deserialize(reader);
			return uIntHintParameter;
		}

		protected UIntHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, uint>)global::Mirror.NetworkReaderExtensions.ReadUInt32, (global::System.Action<global::Mirror.NetworkWriter, uint>)delegate(global::Mirror.NetworkWriter writer, uint writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, writerValue);
			})
		{
		}

		public UIntHintParameter(uint value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, uint>)global::Mirror.NetworkReaderExtensions.ReadUInt32, (global::System.Action<global::Mirror.NetworkWriter, uint>)delegate(global::Mirror.NetworkWriter writer, uint writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, writerValue);
			})
		{
		}
	}
}
