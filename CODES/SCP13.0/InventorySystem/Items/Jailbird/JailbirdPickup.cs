using System.Runtime.InteropServices;
using InventorySystem.Items.Pickups;
using Mirror;
using Scp914;
using UnityEngine;

namespace InventorySystem.Items.Jailbird
{
	public class JailbirdPickup : CollisionDetectionPickup, IUpgradeTrigger
	{
		[SyncVar]
		public JailbirdWearState Wear;

		[SerializeField]
		private JailbirdMaterialController _materialController;

		private JailbirdWearState _prevWear;

		public float TotalMelee { get; set; }

		public int TotalCharges { get; set; }

		public JailbirdWearState NetworkWear
		{
			get
			{
				return default(JailbirdWearState);
			}
			[param: In]
			set
			{
			}
		}

		public void ServerOnUpgraded(Scp914KnobSetting setting)
		{
		}

		private void Update()
		{
		}

		private void UpdateWearFromSyncvar()
		{
		}

		protected override void Start()
		{
		}
	}
}
