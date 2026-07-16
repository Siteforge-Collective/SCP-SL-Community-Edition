namespace InventorySystem.Searching
{
	public class AmmoSearchCompletor : global::InventorySystem.Searching.SearchCompletor
	{
		private readonly ItemType _ammoType;

		private ushort CurrentAmmo
		{
			get
			{
				return Hub.inventory.GetCurAmmo(_ammoType);
			}
			set
			{
				Hub.inventory.ServerSetAmmo(_ammoType, value);
			}
		}

		private ushort MaxAmmo => global::InventorySystem.Configs.InventoryLimits.GetAmmoLimit(_ammoType, Hub);

		public AmmoSearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase targetPickup, global::InventorySystem.Items.ItemBase targetItem, double maxDistanceSquared)
			: base(hub, targetPickup, targetItem, maxDistanceSquared)
		{
			_ammoType = targetItem.ItemTypeId;
		}

		protected override bool ValidateAny()
		{
			if (!base.ValidateAny())
			{
				return false;
			}
			uint maxAmmo = MaxAmmo;
			if (CurrentAmmo >= maxAmmo)
			{
				Hub.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxAmmoAlreadyReached, new global::Hints.HintParameter[2]
				{
					new global::Hints.AmmoHintParameter((byte)_ammoType),
					new global::Hints.PackedULongHintParameter(maxAmmo)
				}, new global::Hints.HintEffect[1] { global::Hints.HintEffectPresets.TrailingPulseAlpha(0.5f, 1f, 0.5f, 2f, 0f, 2) }, 2f));
				return false;
			}
			return true;
		}

		public override void Complete()
		{
			if (TargetPickup is global::InventorySystem.Items.Firearms.Ammo.AmmoPickup ammoPickup)
			{
				uint currentAmmo = CurrentAmmo;
				uint num = global::System.Math.Min(currentAmmo + ammoPickup.SavedAmmo, MaxAmmo) - currentAmmo;
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerPickupAmmo, Hub, ammoPickup))
				{
					if (num >= ammoPickup.SavedAmmo)
					{
						TargetPickup.DestroySelf();
					}
					else
					{
						ammoPickup.NetworkSavedAmmo = (ushort)(ammoPickup.SavedAmmo - (byte)num);
						global::InventorySystem.Items.Pickups.PickupSyncInfo info = TargetPickup.Info;
						info.InUse = false;
						TargetPickup.NetworkInfo = info;
						Hub.hints.Show(new global::Hints.TranslationHint(global::Hints.HintTranslations.MaxAmmoReached, new global::Hints.HintParameter[2]
						{
							new global::Hints.AmmoHintParameter((byte)_ammoType),
							new global::Hints.PackedULongHintParameter(MaxAmmo)
						}, global::Hints.HintEffectPresets.FadeInAndOut(0.25f), 1.5f));
					}
					CurrentAmmo += (ushort)num;
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError("The pickup needs to derive from AmmoPickup");
			}
		}
	}
}
