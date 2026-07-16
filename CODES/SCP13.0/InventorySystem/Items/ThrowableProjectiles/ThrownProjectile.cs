using System;
using System.Runtime.CompilerServices;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class ThrownProjectile : CollisionDetectionPickup
	{
        public static readonly CachedLayerMask HitBlockerMask = new CachedLayerMask("Default", "Glass", "CCTV", "Door");
        [SerializeField]
		private GameObject _renderersRoot;

        public static event Action<ThrownProjectile> OnProjectileSpawned;

        protected override void Start()
        {
            base.Start();
            ThrownProjectile.OnProjectileSpawned?.Invoke(this);
        }

        public virtual void ToggleRenderers(bool state)
        {
            _renderersRoot.SetActive(state);
        }

        public virtual void ServerActivate()
		{
		}
	}
}
