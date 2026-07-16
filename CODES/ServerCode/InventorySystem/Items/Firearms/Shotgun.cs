namespace InventorySystem.Items.Firearms
{
	public class Shotgun : global::InventorySystem.Items.Firearms.Firearm
	{
		[global::UnityEngine.SerializeField]
		private byte _ammoCapacity;

		[global::UnityEngine.SerializeField]
		private float _adsTime;

		[global::UnityEngine.SerializeField]
		private float _timeBetweenShots;

		[global::UnityEngine.SerializeField]
		private float _pumpingTime;

		[global::UnityEngine.SerializeField]
		private byte _numberOfChambers;

		[global::UnityEngine.SerializeField]
		private global::CameraShaking.RecoilSettings _recoil;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmBaseStats _stats;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.BuckshotSettings _buckshotStats;

		public override global::InventorySystem.Items.Firearms.FirearmBaseStats BaseStats => _stats;

		public override global::System.Type CrosshairType { get; protected set; } = typeof(global::InventorySystem.Crosshairs.ShotgunCrosshair);

		public override ItemType AmmoType => ItemType.Ammo12gauge;

		public override global::InventorySystem.Items.Firearms.Modules.IAmmoManagerModule AmmoManagerModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IEquipperModule EquipperModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IActionModule ActionModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IInspectorModule InspectorModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IHitregModule HitregModule { get; set; }

		public override global::InventorySystem.Items.Firearms.Modules.IAdsModule AdsModule { get; set; }

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnAdded(pickup);
			AmmoManagerModule = new global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager(this, base.ItemSerial, _ammoCapacity, _numberOfChambers, 0.5f, 3, "ShellsToLoad", ActionName.Zoom, ActionName.Shoot);
			EquipperModule = new global::InventorySystem.Items.Firearms.Modules.EventBasedEquipper(this);
			ActionModule = new global::InventorySystem.Items.Firearms.Modules.PumpAction(this, base.ItemSerial, _numberOfChambers, _timeBetweenShots, _pumpingTime, _recoil, "Pump", 2, 14);
			InspectorModule = new global::InventorySystem.Items.Firearms.Modules.SimpleInspector(this, 0);
			AdsModule = new global::InventorySystem.Items.Firearms.Modules.StandardAds(this, base.ItemSerial, _adsTime, 2, 3, 4);
			HitregModule = new global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg(this, base.Owner, _buckshotStats);
		}

		public override void OnEquipped()
		{
			base.OnEquipped();
		}

		public override void UpdateAnims()
		{
			this.AnimSetBool(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.IsCocked, (ActionModule as global::InventorySystem.Items.Firearms.Modules.PumpAction).ChamberedRounds > 0);
			this.AnimSetInt(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Ammo, Status.Ammo);
			this.AnimSetFloat(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.DrawSpeedMultiplier, global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(this, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.DrawSpeedMultiplier));
			float num = global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(this, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ReloadSpeedMultiplier);
			this.AnimSetFloat(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.ReloadSpeedMultiplier, num);
			this.AnimSetFloat(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.FireRateMultiplier, num * global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(this, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.FireRateMultiplier));
		}
	}
}
