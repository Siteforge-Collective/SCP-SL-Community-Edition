using System.Collections.Generic;
using InventorySystem.Items;

namespace InventorySystem.Hotkeys.Customization
{
	public class HotkeySkipEmptyFirearms : HotkeyOverrideBase
	{
		public override HotkeyOverrideOption OptionType => default(HotkeyOverrideOption);

		public override HotkeysTranslation OptionName => default(HotkeysTranslation);

		public override HotkeysTranslation? Description => null;

		public override bool TryGetOverride(ReferenceHub player, int hotkeyId, List<ItemBase> matches, out ItemBase result)
		{
			result = null;
			return false;
		}

		public override bool CheckAvailability(List<PoolElementData> elements)
		{
			return false;
		}

		private bool CheckAvailability(List<ItemType> items)
		{
			return false;
		}
	}
}
