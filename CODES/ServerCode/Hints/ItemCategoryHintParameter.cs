namespace Hints
{
	public class ItemCategoryHintParameter : global::Hints.IdHintParameter
	{
		public static global::Hints.ItemCategoryHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.ItemCategoryHintParameter itemCategoryHintParameter = new global::Hints.ItemCategoryHintParameter();
			itemCategoryHintParameter.Deserialize(reader);
			return itemCategoryHintParameter;
		}

		private ItemCategoryHintParameter()
		{
		}

		public ItemCategoryHintParameter(ItemCategory category)
		{
			if (category == ItemCategory.None)
			{
				throw new global::System.ArgumentException("Item category cannot be none (no proper translation).", "category");
			}
			base._002Ector((byte)category);
		}
	}
}
