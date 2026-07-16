namespace Hints
{
	public static class ItemCategoryHintParameterFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::Hints.ItemCategoryHintParameter value)
		{
			value.Serialize(writer);
		}

		public static global::Hints.ItemCategoryHintParameter Deserialize(this global::Mirror.NetworkReader reader)
		{
			return global::Hints.ItemCategoryHintParameter.FromNetwork(reader);
		}
	}
}
