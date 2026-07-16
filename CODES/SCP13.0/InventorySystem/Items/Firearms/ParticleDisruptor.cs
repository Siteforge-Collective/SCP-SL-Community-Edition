using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class ParticleDisruptor : Firearm, IEquipDequipModifier
	{
		private bool _tryRemoveNextFrame;

		[Header("Balance Settings")]
		[SerializeField]
		private FirearmBaseStats _stats;

		[SerializeField]
		private ExplosionGrenade _explosionSettings;

		public GameObject ExplosionPrefab;

		public override FirearmBaseStats BaseStats => default(FirearmBaseStats);

		public override ItemType AmmoType => default(ItemType);

		public override IAmmoManagerModule AmmoManagerModule { get; set; }

		public override IEquipperModule EquipperModule { get; set; }

		public override IActionModule ActionModule { get; set; }

		public override IInspectorModule InspectorModule { get; set; }

		public override IAdsModule AdsModule { get; set; }

		public override IHitregModule HitregModule { get; set; }

		public bool AllowHolster => false;

		public bool AllowEquip => false;

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void OnEquipped()
		{
		}

		public override void OnHolstered()
		{
		}

		public override void OnRemoved(ItemPickupBase pickup)
		{
		}

		public override void UpdateAnims()
		{
		}

		private void Update()
		{
		}

		private void TryRemove()
		{
		}
	}
}
