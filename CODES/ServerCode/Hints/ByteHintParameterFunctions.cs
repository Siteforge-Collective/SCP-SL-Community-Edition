namespace Hints
{
	public static class ByteHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.ByteHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.ByteHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.ByteHintParameter.FromNetwork(reader);
		}
	}
}
