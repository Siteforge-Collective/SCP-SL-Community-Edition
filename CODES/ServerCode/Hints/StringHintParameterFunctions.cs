namespace Hints
{
	public static class StringHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.StringHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.StringHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.StringHintParameter.FromNetwork(reader);
		}
	}
}
