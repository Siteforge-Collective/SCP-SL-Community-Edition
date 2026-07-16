using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Configs;
using Hints;
using UnityEngine;
using System;
using InventorySystem.Items.Firearms.Ammo;

namespace InventorySystem.Searching
{
    public class AmmoSearchCompletor : SearchCompletor
    {
        private readonly ItemType _ammoType;

        private ushort CurrentAmmo
        {
            get
            {
                if (Hub.inventory.UserInventory.ReserveAmmo.TryGetValue(_ammoType, out ushort ammo))
                {
                    return ammo;
                }
                return 0;
            }
            set
            {
                Hub.inventory.ServerSetAmmo(_ammoType, value);
            }
        }

        private ushort MaxAmmo => InventoryLimits.GetAmmoLimit(_ammoType, Hub);

        public AmmoSearchCompletor(ReferenceHub hub, ItemPickupBase targetPickup, ItemBase targetItem, double maxDistanceSquared)
            : base(hub, targetPickup, targetItem, maxDistanceSquared)
        {
            _ammoType = targetItem.ItemTypeId;
        }

        protected override bool ValidateAny()
        {
            if (!base.ValidateAny())
                return false;

            ushort ammoLimit = MaxAmmo; 
            if (CurrentAmmo < ammoLimit)
                return true;

            HintParameter[] parameters = new HintParameter[]
            {
                new AmmoHintParameter((byte)_ammoType),
                new PackedULongHintParameter(ammoLimit) 
            };

            HintEffect[] effects = new HintEffect[]
            {
                HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.7f) 
            };

            Hub.hints.Show(new TranslationHint(
                HintTranslations.MaxAmmoAlreadyReached,
                parameters,
                effects,
                0f
            ));

            return false;
        }

        public override void Complete()
        {
            if (TargetPickup is AmmoPickup ammoPickup)
            {
                ushort current = CurrentAmmo;
                ushort limit = MaxAmmo;

                ushort amountToTake = (ushort)Mathf.Min(ammoPickup.SavedAmmo, limit - current);

                if (amountToTake < ammoPickup.SavedAmmo)
                {
                    ammoPickup.SavedAmmo -= amountToTake;

                    PickupSyncInfo info = ammoPickup.Info;
                    info.InUse = false;
                    ammoPickup.Info = info;

                    Hub.hints.Show(new TranslationHint(
                        HintTranslations.MaxAmmoReached,
                        new HintParameter[]
                        {
                            new AmmoHintParameter((byte)_ammoType),
                            new PackedULongHintParameter(amountToTake)
                        },
                        HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)
                    ));
                }
                else
                {
                    ammoPickup.DestroySelf();
                }

                CurrentAmmo = (ushort)(current + amountToTake);
            }
            else
            {
                Debug.LogError("The pickup needs to derive from AmmoPickup");
            }
        }
    }
}