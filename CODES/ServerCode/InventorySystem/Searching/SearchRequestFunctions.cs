namespace InventorySystem.Searching
{
	public static class SearchRequestFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Searching.SearchRequest value)
		{
			value.Serialize(writer);
		}

		public static global::InventorySystem.Searching.SearchRequest Deserialize(this global::Mirror.NetworkReader reader)
		{
			global::InventorySystem.Searching.SearchRequest result = default(global::InventorySystem.Searching.SearchRequest);
			result.Deserialize(reader);
			return result;
		}
	}
}
