namespace InventorySystem.Items.Jailbird
{
	public class JailbirdPickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup
	{
		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Jailbird.JailbirdMaterialController _materialController;

		public float TotalMelee { get; set; }

		public int TotalCharges { get; set; }

		protected override void Start()
		{
			base.Start();
			_materialController.SetSerial(Info.Serial);
		}

		private void MirrorProcessed()
		{
		}
	}
}
