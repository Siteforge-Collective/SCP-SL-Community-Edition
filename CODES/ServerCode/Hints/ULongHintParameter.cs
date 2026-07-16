namespace Hints
{
	public class ULongHintParameter : global::Hints.PrimitiveHintParameter<ulong>
	{
		public static global::Hints.ULongHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.ULongHintParameter uLongHintParameter = new global::Hints.ULongHintParameter();
			uLongHintParameter.Deserialize(reader);
			return uLongHintParameter;
		}

		protected ULongHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, ulong>)global::Mirror.NetworkReaderExtensions.ReadUInt64, (global::System.Action<global::Mirror.NetworkWriter, ulong>)delegate(global::Mirror.NetworkWriter writer, ulong writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, writerValue);
			})
		{
		}

		public ULongHintParameter(ulong value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, ulong>)global::Mirror.NetworkReaderExtensions.ReadUInt64, (global::System.Action<global::Mirror.NetworkWriter, ulong>)delegate(global::Mirror.NetworkWriter writer, ulong writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, writerValue);
			})
		{
		}
	}
}
