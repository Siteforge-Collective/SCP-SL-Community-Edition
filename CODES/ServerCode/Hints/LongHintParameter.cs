namespace Hints
{
	public class LongHintParameter : global::Hints.PrimitiveHintParameter<long>
	{
		public static global::Hints.LongHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.LongHintParameter longHintParameter = new global::Hints.LongHintParameter();
			longHintParameter.Deserialize(reader);
			return longHintParameter;
		}

		protected LongHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, long>)global::Mirror.NetworkReaderExtensions.ReadInt64, (global::System.Action<global::Mirror.NetworkWriter, long>)delegate(global::Mirror.NetworkWriter writer, long writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt64(writer, writerValue);
			})
		{
		}

		public LongHintParameter(long value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, long>)global::Mirror.NetworkReaderExtensions.ReadInt64, (global::System.Action<global::Mirror.NetworkWriter, long>)delegate(global::Mirror.NetworkWriter writer, long writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt64(writer, writerValue);
			})
		{
		}
	}
}
