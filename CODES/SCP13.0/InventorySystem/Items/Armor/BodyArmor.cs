using System;
using CustomPlayerEffects;
using InventorySystem.Items.Pickups;
using InventorySystem.Searching;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace InventorySystem.Items.Armor
{
    public class BodyArmor : ItemBase, IEquipDequipModifier, IWearableItem, IItemNametag, ICustomSearchCompletorItem, IMovementSpeedModifier, IStaminaModifier
    {
        [Serializable]
        public struct ArmorAmmoLimit
        {
            public ItemType AmmoType;
            public ushort Limit;
        }

        [Serializable]
        public struct ArmorCategoryLimitModifier
        {
            public ItemCategory Category;
            public byte Limit;
        }

        [NonSerialized] public bool DontRemoveExcessOnDrop;

        public int HelmetEfficacy;
        public int VestEfficacy;
        public float CivilianClassDownsidesMultiplier = 1f;

        public ArmorAmmoLimit[] AmmoLimits;
        public ArmorCategoryLimitModifier[] CategoryLimits;

        [SerializeField] private float _staminaUseMultiplier = 1f;
        [SerializeField] private float _movementSpeedMultiplier = 1f;
        [SerializeField] private float _weight;

        public override float Weight => _weight;
        public bool IsWorn => true; 

        public bool AllowEquip => false;
        public bool AllowHolster => true;

        public bool MovementModifierActive => !IHeavyItemPenaltyImmunity.IsImmune(Owner);
        public float MovementSpeedMultiplier => ProcessMultiplier(_movementSpeedMultiplier);
        public float MovementSpeedLimit => float.MaxValue;

        public bool StaminaModifierActive => !IHeavyItemPenaltyImmunity.IsImmune(Owner);
        public float StaminaUsageMultiplier => ProcessMultiplier(_staminaUseMultiplier);
        public bool SprintingDisabled => false;
        public float StaminaRegenMultiplier => 1f;

        public override ItemDescriptionType DescriptionType => ItemDescriptionType.Armor;

        public string Name => ItemTranslationReader.GetName(ItemTypeId);

        private float ProcessMultiplier(float f)
        {
            if (Owner == null) return f;

            Team team = Owner.GetTeam();
            if (team == Team.ClassD || team == Team.Scientists)
            {
                return (f - 1f) * CivilianClassDownsidesMultiplier + 1f;
            }
            return f;
        }

        public SearchCompletor GetCustomSearchCompletor(ReferenceHub hub, ItemPickupBase ipb, ItemBase ib, double disSqrt)
        {
            return new ArmorSearchCompletor(hub, ipb, ib, disSqrt);
        }

        public override void OnRemoved(ItemPickupBase pickup)
        {
            base.OnRemoved(pickup);

            if (NetworkServer.active && !DontRemoveExcessOnDrop)
            {
                BodyArmorUtils.RemoveEverythingExceedingLimits(OwnerInventory, this, true, true);
            }
        }
    }
}