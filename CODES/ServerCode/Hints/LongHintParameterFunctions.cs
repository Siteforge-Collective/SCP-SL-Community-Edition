namespace Hints
{
	public static class LongHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.LongHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.LongHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.LongHintParameter.FromNetwork(reader);
		}
	}
}
