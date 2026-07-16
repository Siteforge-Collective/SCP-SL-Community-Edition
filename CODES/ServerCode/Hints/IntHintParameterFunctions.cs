namespace Hints
{
	public static class IntHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.IntHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.IntHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.IntHintParameter.FromNetwork(reader);
		}
	}
}
