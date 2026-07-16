namespace Hints
{
	public static class DoubleHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.DoubleHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.DoubleHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.DoubleHintParameter.FromNetwork(reader);
		}
	}
}
