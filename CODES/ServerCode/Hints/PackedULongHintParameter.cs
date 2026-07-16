namespace Hints
{
	public class PackedULongHintParameter : global::Hints.PrimitiveHintParameter<ulong>
	{
		public static global::Hints.PackedULongHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.PackedULongHintParameter packedULongHintParameter = new global::Hints.PackedULongHintParameter();
			packedULongHintParameter.Deserialize(reader);
			return packedULongHintParameter;
		}

		protected PackedULongHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, ulong>)global::Mirror.NetworkReaderExtensions.ReadUInt64, (global::System.Action<global::Mirror.NetworkWriter, ulong>)global::Mirror.NetworkWriterExtensions.WriteUInt64)
		{
		}

		public PackedULongHintParameter(ulong value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, ulong>)global::Mirror.NetworkReaderExtensions.ReadUInt64, (global::System.Action<global::Mirror.NetworkWriter, ulong>)global::Mirror.NetworkWriterExtensions.WriteUInt64)
		{
		}
	}
}
