namespace Hints
{
	public static class ItemHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.ItemHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.ItemHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.ItemHintParameter.FromNetwork(reader);
		}
	}
}
