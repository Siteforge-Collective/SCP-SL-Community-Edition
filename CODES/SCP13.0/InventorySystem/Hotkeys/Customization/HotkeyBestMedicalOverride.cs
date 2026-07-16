using System.Collections.Generic;
using InventorySystem.Items;
using PlayerStatsSystem;
using CustomPlayerEffects;

namespace InventorySystem.Hotkeys.Customization
{
    public class HotkeyBestMedicalOverride : HotkeyOverrideBase
    {
        private static readonly List<ItemType> BestToWorst = new List<ItemType>();

        private static readonly ItemType[] HandledItems = new ItemType[]
        {
            ItemType.SCP500,      // 17
            ItemType.Adrenaline,  // 33
            ItemType.Painkillers, // 34
            ItemType.Medkit       // 14
        };

        public override HotkeyOverrideOption OptionType => HotkeyOverrideOption.BestMedical; 

        public override HotkeysTranslation OptionName => new HotkeysTranslation();

        public override HotkeysTranslation? Description => null;

        private void UpdateConditions(PlayerStats stats, PlayerEffectsController effectsController)
        {
            BestToWorst.Clear();

            float hp = stats.GetModule<HealthStat>().CurValue;
            float stamina = stats.GetModule<StaminaStat>().NormalizedValue;
            bool hasCardiacArrest = effectsController.TryGetEffect<CardiacArrest>(out var cardiac) && cardiac.IsEnabled;

            if (hasCardiacArrest)
            {
                BestToWorst.Add(ItemType.Adrenaline);
                BestToWorst.Add(ItemType.SCP500);
                BestToWorst.Add(ItemType.Painkillers);
                BestToWorst.Add(ItemType.Medkit);
            }
            else if (hp < 50f || stamina < 0.3f)
            {
                BestToWorst.Add(ItemType.SCP500);
                BestToWorst.Add(ItemType.Adrenaline);
                BestToWorst.Add(ItemType.Medkit);
                BestToWorst.Add(ItemType.Painkillers);
            }
            else
            {
                BestToWorst.Add(ItemType.Medkit);
                BestToWorst.Add(ItemType.Painkillers);
                BestToWorst.Add(ItemType.Adrenaline);
                BestToWorst.Add(ItemType.SCP500);
            }
        }

        public override bool TryGetOverride(ReferenceHub player, int hotkeyId, List<ItemBase> matches, out ItemBase result)
        {
            result = null;

            if (player == null || matches == null) return false;

            UpdateConditions(player.playerStats, player.playerEffectsController);

            ItemType[] queue = HotkeyUtils.GetItemsByQueue(hotkeyId);

            foreach (ItemType type in BestToWorst)
            {
                if (System.Array.IndexOf(queue, type) == -1) continue;

                if (player.inventory.TryGetInventoryItem(type, out ItemBase item))
                {
                    result = item;
                    return true;
                }
            }

            return false;
        }

        public override bool CheckAvailability(List<PoolElementData> elements)
        {
            return HotkeyUtils.CountMatches(elements, HandledItems) > 0;
        }
    }
}