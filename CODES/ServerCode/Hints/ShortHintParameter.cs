namespace Hints
{
	public class ShortHintParameter : global::Hints.PrimitiveHintParameter<short>
	{
		public static global::Hints.ShortHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.ShortHintParameter shortHintParameter = new global::Hints.ShortHintParameter();
			shortHintParameter.Deserialize(reader);
			return shortHintParameter;
		}

		protected ShortHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, short>)global::Mirror.NetworkReaderExtensions.ReadInt16, (global::System.Action<global::Mirror.NetworkWriter, short>)global::Mirror.NetworkWriterExtensions.WriteInt16)
		{
		}

		public ShortHintParameter(short value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, short>)global::Mirror.NetworkReaderExtensions.ReadInt16, (global::System.Action<global::Mirror.NetworkWriter, short>)global::Mirror.NetworkWriterExtensions.WriteInt16)
		{
		}
	}
}
