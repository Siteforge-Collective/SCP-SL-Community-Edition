namespace InventorySystem.Items
{
    public static class EquipDequipModifierExtensions
    {
        public static bool CanHolster(this ItemBase ib)
        {
            if (ib is IEquipDequipModifier equipDequipModifier)
            {
                return equipDequipModifier.AllowHolster;
            }
            return true;
        }

        public static bool CanEquip(this ItemBase ib)
        {
            if (ib is IEquipDequipModifier equipDequipModifier)
            {
                return equipDequipModifier.AllowEquip;
            }
            return true;
        }
    }
}
