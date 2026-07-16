namespace InventorySystem.Items.ThrowableProjectiles
{
	public class FlashbangGrenade : global::InventorySystem.Items.ThrowableProjectiles.EffectGrenade
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _blindingOverDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _turnedAwayBlindingDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _blindingOverDot;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _deafenDurationOverDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _turnedAwayDeafenDurationOverDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _shakeOverDistance;

		[global::UnityEngine.SerializeField]
		private float _surfaceZoneDistanceIntensifier;

		[global::UnityEngine.SerializeField]
		private float _additionalBlurDuration;

		[global::UnityEngine.SerializeField]
		private float _minimalEffectDuration;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.LayerMask _blindingMask;

		[global::UnityEngine.SerializeField]
		private float _blindTime;

		protected override void PlayExplosionEffects()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			float time = _blindingOverDistance.keys[_blindingOverDistance.length - 1].time;
			float num = time * time;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!((base.transform.position - allHub.transform.position).sqrMagnitude > num) && !(allHub == PreviousOwner.Hub) && HitboxIdentity.CheckFriendlyFire(PreviousOwner.Role, global::PlayerRoles.PlayerRolesUtils.GetRoleId(allHub)))
				{
					ProcessPlayer(allHub);
				}
			}
		}

		private void ProcessPlayer(ReferenceHub hub)
		{
			if (!global::UnityEngine.Physics.Linecast(base.transform.position, hub.PlayerCameraReference.position, _blindingMask))
			{
				global::UnityEngine.Vector3 vector = base.transform.position - hub.PlayerCameraReference.position;
				float num = vector.magnitude;
				if (hub.transform.position.y > 900f)
				{
					num /= _surfaceZoneDistanceIntensifier;
				}
				bool num2 = global::UnityEngine.Vector3.Dot(hub.PlayerCameraReference.forward, vector.normalized) >= 0.5f;
				float num3 = (num2 ? _blindingOverDistance.Evaluate(num) : _turnedAwayBlindingDistance.Evaluate(num));
				float num4 = (num2 ? num3 : _turnedAwayDeafenDurationOverDistance.Evaluate(num));
				float num5 = (num2 ? _deafenDurationOverDistance.Evaluate(num) : (num4 * _blindTime));
				if (num5 > _minimalEffectDuration)
				{
					hub.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Deafened>(num5, addDuration: true);
				}
				if (num3 > _minimalEffectDuration)
				{
					hub.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Flashed>(num3 * _blindTime, addDuration: true);
				}
				if (num <= 10f)
				{
					hub.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Blinded>(num4 * _blindTime + _additionalBlurDuration * num4, addDuration: true);
				}
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
