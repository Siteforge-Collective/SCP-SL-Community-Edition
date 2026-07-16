using InventorySystem.Items;
using InventorySystem.Items.Usables;

namespace Scp914.Processors
{
    public class UsableItemProcessor : StandardItemProcessor
    {
        public override ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial)
        {
            if (!hub.inventory.UserInventory.Items.TryGetValue(serial, out var value))
            {
                return null;
            }
            if (value is UsableItem { IsUsing: not false })
            {
                return null;
            }
            return base.OnInventoryItemUpgraded(setting, hub, serial);
        }
    }
}
