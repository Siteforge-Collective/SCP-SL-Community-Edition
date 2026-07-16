namespace Hints
{
	public class ItemHintParameter : global::Hints.IdHintParameter
	{
		public static global::Hints.ItemHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.ItemHintParameter itemHintParameter = new global::Hints.ItemHintParameter();
			itemHintParameter.Deserialize(reader);
			return itemHintParameter;
		}

		private ItemHintParameter()
		{
		}

		public ItemHintParameter(ItemType item)
			: base((byte)item)
		{
			if (item == ItemType.None)
			{
				throw new global::System.ArgumentException("Item cannot be none (no proper translation).", "item");
			}
		}
	}
}
