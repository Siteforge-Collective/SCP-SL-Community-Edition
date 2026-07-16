namespace Utils.Networking
{
	public static class HintParameterArrayReaderWriter
	{
		public static global::Hints.HintParameter[] ReadHintParameterArray(this global::Mirror.NetworkReader reader)
		{
			return global::Utils.Networking.ArrayReaderWriter<global::Hints.HintParameter>.ReadArray(reader, global::Utils.Networking.HintParameterReaderWriter.ReadHintParameter);
		}

		public static void WriteHintParameterArray(this global::Mirror.NetworkWriter writer, global::System.Collections.Generic.IReadOnlyCollection<global::Hints.HintParameter> array)
		{
			global::Utils.Networking.ArrayReaderWriter<global::Hints.HintParameter>.WriteArray(writer, array, global::Utils.Networking.HintParameterReaderWriter.WriteHintParameter);
		}
	}
}
