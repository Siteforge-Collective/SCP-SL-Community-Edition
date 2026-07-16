namespace Hints
{
	public static class ULongHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.ULongHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.ULongHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.ULongHintParameter.FromNetwork(reader);
		}
	}
}
