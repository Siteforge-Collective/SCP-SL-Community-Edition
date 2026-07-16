namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class GrenadeRippleTrigger : global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleTriggerBase
	{
		private class ThrownGrenadeHandler
		{
			private readonly global::PlayerRoles.PlayableScps.Scp939.Ripples.GrenadeRippleTrigger _tr;

			private readonly float _startTime;

			private int _nextTime;

			public ThrownGrenadeHandler(global::PlayerRoles.PlayableScps.Scp939.Ripples.GrenadeRippleTrigger trigger)
			{
				_tr = trigger;
				_startTime = global::UnityEngine.Time.timeSinceLevelLoad;
			}

			public bool UpdateSound()
			{
				if (_nextTime >= _tr._rippleTimes.Length)
				{
					return false;
				}
				float num = global::UnityEngine.Time.timeSinceLevelLoad - _startTime;
				if (_tr._rippleTimes[_nextTime] > num)
				{
					return false;
				}
				_nextTime++;
				while (UpdateSound())
				{
				}
				return true;
			}
		}

		[global::UnityEngine.SerializeField]
		private float[] _rippleTimes;

		[global::UnityEngine.SerializeField]
		private float _audibleRangeSqr;

		private readonly global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile, global::PlayerRoles.PlayableScps.Scp939.Ripples.GrenadeRippleTrigger.ThrownGrenadeHandler> _trackedGrenades = new global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile, global::PlayerRoles.PlayableScps.Scp939.Ripples.GrenadeRippleTrigger.ThrownGrenadeHandler>();

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile.OnProjectileSpawned += OnProjectileSpawned;
			global::InventorySystem.Items.Pickups.ItemPickupBase.OnPickupDestroyed += OnPickupDestroyed;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile.OnProjectileSpawned -= OnProjectileSpawned;
			global::InventorySystem.Items.Pickups.ItemPickupBase.OnPickupDestroyed -= OnPickupDestroyed;
		}

		private void OnProjectileSpawned(global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile tp)
		{
			if (tp.Info.ItemId == ItemType.GrenadeHE)
			{
				_trackedGrenades.Add(tp, new global::PlayerRoles.PlayableScps.Scp939.Ripples.GrenadeRippleTrigger.ThrownGrenadeHandler(this));
			}
		}

		private void OnPickupDestroyed(global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			if (ipb is global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile key)
			{
				_trackedGrenades.Remove(key);
			}
		}

		private void Update()
		{
			if (!base.IsLocalOrSpectated)
			{
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile, global::PlayerRoles.PlayableScps.Scp939.Ripples.GrenadeRippleTrigger.ThrownGrenadeHandler> trackedGrenade in _trackedGrenades)
			{
				if (trackedGrenade.Value.UpdateSound())
				{
					PlayInRangeSqr(trackedGrenade.Key.RigidBody.position, _audibleRangeSqr, global::UnityEngine.Color.red);
				}
			}
		}
	}
}
