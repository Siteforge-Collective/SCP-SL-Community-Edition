namespace Hints
{
	public class PackedLongHintParameter : global::Hints.PrimitiveHintParameter<long>
	{
		public static global::Hints.PackedLongHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.PackedLongHintParameter packedLongHintParameter = new global::Hints.PackedLongHintParameter();
			packedLongHintParameter.Deserialize(reader);
			return packedLongHintParameter;
		}

		protected PackedLongHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, long>)global::Mirror.NetworkReaderExtensions.ReadInt64, (global::System.Action<global::Mirror.NetworkWriter, long>)global::Mirror.NetworkWriterExtensions.WriteInt64)
		{
		}

		public PackedLongHintParameter(long value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, long>)global::Mirror.NetworkReaderExtensions.ReadInt64, (global::System.Action<global::Mirror.NetworkWriter, long>)global::Mirror.NetworkWriterExtensions.WriteInt64)
		{
		}
	}
}
