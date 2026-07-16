namespace Hints
{
	public static class UShortHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.UShortHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.UShortHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.UShortHintParameter.FromNetwork(reader);
		}
	}
}
