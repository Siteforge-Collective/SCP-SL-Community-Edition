namespace Hints
{
	public class IntHintParameter : global::Hints.PrimitiveHintParameter<int>
	{
		public static global::Hints.IntHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.IntHintParameter intHintParameter = new global::Hints.IntHintParameter();
			intHintParameter.Deserialize(reader);
			return intHintParameter;
		}

		protected IntHintParameter()
			: base((global::System.Func<global::Mirror.NetworkReader, int>)global::Mirror.NetworkReaderExtensions.ReadInt32, (global::System.Action<global::Mirror.NetworkWriter, int>)delegate(global::Mirror.NetworkWriter writer, int writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt32(writer, writerValue);
			})
		{
		}

		public IntHintParameter(int value)
			: base(value, (global::System.Func<global::Mirror.NetworkReader, int>)global::Mirror.NetworkReaderExtensions.ReadInt32, (global::System.Action<global::Mirror.NetworkWriter, int>)delegate(global::Mirror.NetworkWriter writer, int writerValue)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt32(writer, writerValue);
			})
		{
		}
	}
}
