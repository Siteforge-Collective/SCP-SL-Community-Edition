namespace InventorySystem.Searching
{
	public static class SearchInvalidationFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Searching.SearchInvalidation value)
		{
			value.Serialize(writer);
		}

		public static global::InventorySystem.Searching.SearchInvalidation Deserialize(this global::Mirror.NetworkReader reader)
		{
			global::InventorySystem.Searching.SearchInvalidation result = default(global::InventorySystem.Searching.SearchInvalidation);
			result.Deserialize(reader);
			return result;
		}
	}
}
