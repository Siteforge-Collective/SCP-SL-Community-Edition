using System;
using CameraShaking;
using InventorySystem.Items.Firearms.Attachments.Components;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class Shotgun : Firearm
	{
		private BuckshotPatternAttachment[] _buckshotAttachments;

		[SerializeField]
		private byte _ammoCapacity;

		[SerializeField]
		private float _adsTime;

		[SerializeField]
		private float _timeBetweenShots;

		[SerializeField]
		private float _pumpingTime;

		[SerializeField]
		private byte _numberOfChambers;

		[SerializeField]
		private RecoilSettings _recoil;

		[SerializeField]
		private FirearmBaseStats _stats;

		public override FirearmBaseStats BaseStats => default(FirearmBaseStats);

		public override Type CrosshairType { get; protected set; }

		public override ItemType AmmoType => default(ItemType);

		public override IAmmoManagerModule AmmoManagerModule { get; set; }

		public override IEquipperModule EquipperModule { get; set; }

		public override IActionModule ActionModule { get; set; }

		public override IInspectorModule InspectorModule { get; set; }

		public override IHitregModule HitregModule { get; set; }

		public override IAdsModule AdsModule { get; set; }

		private BuckshotHitreg.BuckshotSettings GetBuckshotPattern()
		{
			return default(BuckshotHitreg.BuckshotSettings);
		}

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
