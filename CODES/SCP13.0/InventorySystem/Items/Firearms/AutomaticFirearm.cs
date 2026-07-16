using CameraShaking;
using InventorySystem.Drawers;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class AutomaticFirearm : Firearm, IItemProgressbarDrawer, IItemDrawer
	{
		[Header("General Settings")]
		[SerializeField]
		private ItemType _ammoType;

		[SerializeField]
		private AttachmentSlot[] _animatorExposedSlots;

		[SerializeField]
		private byte _dryfireClipId;

		[SerializeField]
		private byte _triggerClipId;

		[SerializeField]
		private byte _adsInClip;

		[SerializeField]
		private byte _adsOutClip;

		[SerializeField]
		private float _gunshotPitchRandomization;

		[Header("Balance Settings")]
		[SerializeField]
		private FirearmBaseStats _stats;

		[SerializeField]
		private float _fireRate;

		[SerializeField]
		private float _boltTravelTime;

		[SerializeField]
		private bool _hasBoltLock;

		[SerializeField]
		private RecoilSettings _recoil;

		[SerializeField]
		private FirearmRecoilPattern _recoilPattern;

		[SerializeField]
		private byte _baseMaxAmmo;

		[SerializeField]
		private bool _semiAutomatic;

		[SerializeField]
		private float _standardAdsTime;

		[SerializeField]
		private int _chamberSize;

		[Header("Debug")]
		[SerializeField]
		private bool _debugRecoilPattern;

		public override FirearmBaseStats BaseStats => default(FirearmBaseStats);

		public override ItemType AmmoType => default(ItemType);

		public override IAmmoManagerModule AmmoManagerModule { get; set; }

		public override IEquipperModule EquipperModule { get; set; }

		public override IActionModule ActionModule { get; set; }

		public override IInspectorModule InspectorModule { get; set; }

		public override IAdsModule AdsModule { get; set; }

		public override IHitregModule HitregModule { get; set; }

		public bool ProgressbarEnabled => false;

		public float ProgressbarMin => 0f;

		public float ProgressbarMax => 0f;

		public float ProgressbarValue => 0f;

		public float ProgressbarWidth => 0f;

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void OnEquipped()
		{
		}

		public override void UpdateAnims()
		{
		}
	}
}
