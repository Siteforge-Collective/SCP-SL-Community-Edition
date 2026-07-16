namespace Hints
{
	public static class TimespanHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.TimespanHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.TimespanHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.TimespanHintParameter.FromNetwork(reader);
		}
	}
}
