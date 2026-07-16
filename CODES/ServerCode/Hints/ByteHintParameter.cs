namespace Hints
{
	public class ByteHintParameter : global::Hints.PrimitiveHintParameter<byte>
	{
		public static global::Hints.ByteHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.ByteHintParameter byteHintParameter = new global::Hints.ByteHintParameter();
			byteHintParameter.Deserialize(reader);
			return byteHintParameter;
		}

		protected ByteHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, byte>)global::Mirror.NetworkReaderExtensions.ReadByte, (global::System.Action<global::Mirror.NetworkWriter, byte>)global::Mirror.NetworkWriterExtensions.WriteByte)
		{
		}

		public ByteHintParameter(byte value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, byte>)global::Mirror.NetworkReaderExtensions.ReadByte, (global::System.Action<global::Mirror.NetworkWriter, byte>)global::Mirror.NetworkWriterExtensions.WriteByte)
		{
		}
	}
}
