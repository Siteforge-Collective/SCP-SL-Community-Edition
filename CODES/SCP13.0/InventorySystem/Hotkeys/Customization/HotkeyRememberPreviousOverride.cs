using System.Collections.Generic;
using InventorySystem.Items;
using UnityEngine;

namespace InventorySystem.Hotkeys.Customization
{
	public class HotkeyRememberPreviousOverride : HotkeyOverrideBase
	{
		private static readonly Dictionary<KeyCode, ushort> PrevItems;

		public override HotkeyOverrideOption OptionType => default(HotkeyOverrideOption);

		public override HotkeysTranslation OptionName => default(HotkeysTranslation);

		public override HotkeysTranslation? Description => null;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		public override bool CheckAvailability(List<PoolElementData> elements)
		{
			return false;
		}

		public override bool TryGetOverride(ReferenceHub player, int hotkeyId, List<ItemBase> matches, out ItemBase result)
		{
			result = null;
			return false;
		}
	}
}
