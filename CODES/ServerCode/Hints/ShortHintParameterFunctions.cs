namespace Hints
{
	public static class ShortHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.ShortHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.ShortHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.ShortHintParameter.FromNetwork(reader);
		}
	}
}
