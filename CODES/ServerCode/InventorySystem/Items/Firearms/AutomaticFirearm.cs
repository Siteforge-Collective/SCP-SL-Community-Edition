namespace InventorySystem.Items.Firearms
{
	public class AutomaticFirearm : global::InventorySystem.Items.Firearms.Firearm, global::InventorySystem.Drawers.IItemProgressbarDrawer, global::InventorySystem.Drawers.IItemDrawer
	{
		[global::UnityEngine.Header("General Settings")]
		[global::UnityEngine.SerializeField]
		private ItemType _ammoType;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot[] _animatorExposedSlots;

		[global::UnityEngine.SerializeField]
		private byte _dryfireClipId;

		[global::UnityEngine.SerializeField]
		private byte _triggerClipId;

		[global::UnityEngine.SerializeField]
		private byte _adsInClip;

		[global::UnityEngine.SerializeField]
		private byte _adsOutClip;

		[global::UnityEngine.SerializeField]
		private float _gunshotPitchRandomization;

		[global::UnityEngine.Header("Balance Settings")]
		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmBaseStats _stats;

		[global::UnityEngine.SerializeField]
		private float _fireRate;

		[global::UnityEngine.SerializeField]
		private float _boltTravelTime;

		[global::UnityEngine.SerializeField]
		private bool _hasBoltLock;

		[global::UnityEngine.SerializeField]
		private global::CameraShaking.RecoilSettings _recoil;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmRecoilPattern _recoilPattern;

		[global::UnityEngine.SerializeField]
		private byte _baseMaxAmmo;

		[global::UnityEngine.SerializeField]
		private bool _semiAutomatic;

		[global::UnityEngine.SerializeField]
		private float _standardAdsTime;

		[global::UnityEngine.SerializeField]
		private int _chamberSize;

		[global::UnityEngine.Header("Debug")]
		[global::UnityEngine.SerializeField]
		private bool _debugRecoilPattern;

		public override global::InventorySystem.Items.Firearms.FirearmBaseStats BaseStats => _stats;

		public override ItemType AmmoType => _ammoType;

		public override global::InventorySystem.Items.Firearms.Modules.IAmmoManagerModule AmmoManagerModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IEquipperModule EquipperModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IActionModule ActionModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IInspectorModule InspectorModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IAdsModule AdsModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IHitregModule HitregModule { get; set; }

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnAdded(pickup);
			AmmoManagerModule = new global::InventorySystem.Items.Firearms.Modules.AutomaticAmmoManager(this, _baseMaxAmmo, 1, _chamberSize);
			EquipperModule = new global::InventorySystem.Items.Firearms.Modules.EventBasedEquipper(this);
			ActionModule = new global::InventorySystem.Items.Firearms.Modules.AutomaticAction(this, _semiAutomatic, _boltTravelTime, 1f / _fireRate, _dryfireClipId, _triggerClipId, _gunshotPitchRandomization, _recoil, _recoilPattern, _hasBoltLock, global::UnityEngine.Mathf.Max(1, _chamberSize));
			InspectorModule = new global::InventorySystem.Items.Firearms.Modules.SimpleInspector(this, 1);
			AdsModule = new global::InventorySystem.Items.Firearms.Modules.StandardAds(this, base.ItemSerial, _standardAdsTime, 2, _adsInClip, _adsOutClip);
			HitregModule = new global::InventorySystem.Items.Firearms.Modules.SingleBulletHitreg(this, base.Owner, _recoilPattern);
		}

		public override void OnEquipped()
		{
			base.OnEquipped();
		}

		public override void UpdateAnims()
		{
			global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = ActionModule.PredictedStatus.Flags;
			this.AnimSetFloat(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.DrawSpeedMultiplier, global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(this, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.DrawSpeedMultiplier));
			this.AnimSetFloat(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.ReloadSpeedMultiplier, global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(this, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ReloadSpeedMultiplier));
			this.AnimSetInt(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Ammo, ActionModule.PredictedStatus.Ammo);
			this.AnimSetBool(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.IsCocked, global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked));
			this.AnimSetBool(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.IsChambered, global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Chambered));
			this.AnimSetBool(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.IsMagInserted, global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted));
			int num = Attachments.Length;
			global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot[] animatorExposedSlots = _animatorExposedSlots;
			foreach (global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot attachmentSlot in animatorExposedSlots)
			{
				int num2 = -1;
				for (int j = 0; j < num; j++)
				{
					if (Attachments[j].Slot == attachmentSlot)
					{
						num2++;
						if (Attachments[j].IsEnabled)
						{
							break;
						}
					}
				}
				if (num2 >= 0 && global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Slots.TryGetValue(attachmentSlot, out var value))
				{
					this.AnimSetInt(value, num2);
				}
			}
		}
	}
}
