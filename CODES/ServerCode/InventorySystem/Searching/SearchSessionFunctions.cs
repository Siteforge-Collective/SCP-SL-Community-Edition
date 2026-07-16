namespace InventorySystem.Searching
{
	public static class SearchSessionFunctions
	{
		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Searching.SearchSession value)
		{
			value.Serialize(writer);
		}

		public static global::InventorySystem.Searching.SearchSession Deserialize(this global::Mirror.NetworkReader reader)
		{
			global::InventorySystem.Searching.SearchSession result = default(global::InventorySystem.Searching.SearchSession);
			result.Deserialize(reader);
			return result;
		}
	}
}
