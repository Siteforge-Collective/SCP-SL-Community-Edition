namespace InventorySystem.Items.ThrowableProjectiles
{
	public class ThrownProjectile : global::InventorySystem.Items.Pickups.CollisionDetectionPickup
	{
		public float AdditionalGravity;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _renderersRoot;

		public static event global::System.Action<global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile> OnProjectileSpawned;

		protected override void Start()
		{
			base.Start();
			global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile.OnProjectileSpawned?.Invoke(this);
		}

		protected virtual void Update()
		{
			Rb.AddForce(global::UnityEngine.Vector3.down * AdditionalGravity, global::UnityEngine.ForceMode.Force);
		}

		public virtual void ToggleRenderers(bool state)
		{
			_renderersRoot.SetActive(state);
		}

		public virtual void ServerActivate()
		{
		}

		static ThrownProjectile()
		{
			global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile.OnProjectileSpawned = delegate
			{
			};
		}

		private void MirrorProcessed()
		{
		}
	}
}
