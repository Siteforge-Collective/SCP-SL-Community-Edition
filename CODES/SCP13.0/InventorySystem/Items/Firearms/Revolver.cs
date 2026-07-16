using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class Revolver : Firearm
	{
		private FirearmBaseStats _uncockedStats;

		private bool _uncockedStatsSet;

		[SerializeField]
		private FirearmBaseStats _stats;

		[SerializeField]
		private float _uncockedInaccuracyAddition;

		private static readonly int StockAttachmentIndex;

		private FirearmBaseStats UncockedStats => default(FirearmBaseStats);

		public override FirearmBaseStats BaseStats => default(FirearmBaseStats);

		public override ItemType AmmoType => default(ItemType);

		public override IAmmoManagerModule AmmoManagerModule { get; set; }

		public override IEquipperModule EquipperModule { get; set; }

		public override IActionModule ActionModule { get; set; }

		public override IInspectorModule InspectorModule { get; set; }

		public override IAdsModule AdsModule { get; set; }

		public override IHitregModule HitregModule { get; set; }

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void UpdateAnims()
		{
		}
	}
}
