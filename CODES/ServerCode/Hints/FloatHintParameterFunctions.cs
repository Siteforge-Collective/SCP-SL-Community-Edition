namespace Hints
{
	public static class FloatHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.FloatHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.FloatHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.FloatHintParameter.FromNetwork(reader);
		}
	}
}
