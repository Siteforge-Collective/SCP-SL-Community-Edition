namespace Hints
{
	public static class SByteHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.SByteHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.SByteHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.SByteHintParameter.FromNetwork(reader);
		}
	}
}
