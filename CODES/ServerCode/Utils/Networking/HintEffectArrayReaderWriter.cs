namespace Utils.Networking
{
	public static class HintEffectArrayReaderWriter
	{
		public static global::Hints.HintEffect[] ReadHintEffectArray(this global::Mirror.NetworkReader reader)
		{
			return global::Utils.Networking.ArrayReaderWriter<global::Hints.HintEffect>.ReadArray(reader, global::Utils.Networking.HintEffectReaderWriter.ReadHintEffect);
		}

		public static void WriteHintEffectArray(this global::Mirror.NetworkWriter writer, global::System.Collections.Generic.IReadOnlyCollection<global::Hints.HintEffect> array)
		{
			global::Utils.Networking.ArrayReaderWriter<global::Hints.HintEffect>.WriteArray(writer, array, global::Utils.Networking.HintEffectReaderWriter.WriteHintEffect);
		}
	}
}
