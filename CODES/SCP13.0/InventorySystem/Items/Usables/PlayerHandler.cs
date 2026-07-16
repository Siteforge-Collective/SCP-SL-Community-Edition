using System.Collections.Generic;

namespace InventorySystem.Items.Usables
{
	public class PlayerHandler
	{
		public CurrentlyUsedItem CurrentUsable;

		public readonly List<RegenerationProcess> ActiveRegenerations;

		public readonly Dictionary<ItemType, float> PersonalCooldowns;

		public void DoUpdate(ReferenceHub hub)
		{
		}

		public void ResetAll()
		{
		}
	}
}
