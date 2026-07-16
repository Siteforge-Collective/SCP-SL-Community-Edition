using System;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class FlashbangGrenade : EffectGrenade
	{
		[SerializeField]
		private AnimationCurve _blindingOverDistance;

		[SerializeField]
		private AnimationCurve _turnedAwayBlindingDistance;

		[SerializeField]
		private AnimationCurve _blindingOverDot;

		[SerializeField]
		private AnimationCurve _deafenDurationOverDistance;

		[SerializeField]
		private AnimationCurve _turnedAwayDeafenDurationOverDistance;

		[SerializeField]
		private AnimationCurve _shakeOverDistance;

		[SerializeField]
		private float _surfaceZoneDistanceIntensifier;

		[SerializeField]
		private float _additionalBlurDuration;

		[SerializeField]
		private float _minimalEffectDuration;

		[SerializeField]
		private LayerMask _blindingMask;

		[SerializeField]
		private float _blindTime;

		private int _hitPlayerCount;

		public override void PlayExplosionEffects(Vector3 pos)
		{
			base.PlayExplosionEffects(pos);

			try
			{
				if (ExplosionCameraShake.singleton != null && MainCameraController.CurrentCamera != null && _shakeOverDistance != null)
				{
					float distance = Vector3.Distance(MainCameraController.CurrentCamera.transform.position, pos);
					float shake = _shakeOverDistance.Evaluate(distance);
					ExplosionCameraShake.singleton.Shake(shake);
				}
			}
			catch (Exception)
			{
			}
		}

		protected override void ServerFuseEnd()
		{
			float maxDetectDistance = 0f;
			if (_blindingOverDistance != null && _blindingOverDistance.keys != null && _blindingOverDistance.length > 0)
			{
				var keys = _blindingOverDistance.keys;
				maxDetectDistance = keys[_blindingOverDistance.length - 1].time;
			}

			if (_surfaceZoneDistanceIntensifier > 0f)
			{
				maxDetectDistance *= 1f + _surfaceZoneDistanceIntensifier;
			}

			_hitPlayerCount = 0;

			foreach (ReferenceHub hub in ReferenceHub.AllHubs)
			{
				if (hub == null) continue;

				Vector3 grenadePos = base.transform.position;
				Vector3 playerPos = hub.transform.position;
				float distance = Vector3.Distance(grenadePos, playerPos);

				if (distance <= maxDetectDistance)
				{
					if (PreviousOwner.IsSet)
					{
						var attackerRole = PreviousOwner.Role;
						var victimRole = PlayerRoles.PlayerRolesUtils.GetRoleId(hub);
						if (!HitboxIdentity.IsDamageable(attackerRole, victimRole))
							continue;
					}

					ProcessPlayer(hub);
				}
			}

			if (_hitPlayerCount == 0 && PreviousOwner.IsSet && PreviousOwner.Hub != null)
			{
				Hitmarker.SendHitmarkerDirectly(PreviousOwner.Hub, 1f);
			}

			try
			{
				Utils.ExplosionUtils.ServerSpawnEffect(base.transform.position, ItemType.GrenadeFlash);
			}
			catch (Exception)
			{
			}

			DestroySelf();
		}

		private void ProcessPlayer(ReferenceHub hub)
		{
			if (hub == null) return;

			Vector3 grenadePos = base.transform.position;
			Vector3 camPos = (hub.PlayerCameraReference != null) ? hub.PlayerCameraReference.position : hub.transform.position;

			if (Physics.Linecast(grenadePos, camPos, _blindingMask))
				return;

			float distance = Vector3.Distance(grenadePos, camPos);

			Vector3 forward = (hub.PlayerCameraReference != null) ? hub.PlayerCameraReference.forward : hub.transform.forward;
			Vector3 dirToGrenade = (grenadePos - camPos).normalized;
			float dot = Vector3.Dot(forward, dirToGrenade); 
			float dot01 = Mathf.Clamp01((dot + 1f) * 0.5f);  

			bool turnedAway = dot01 < 0.5f;
			AnimationCurve blindingCurve = turnedAway ? _turnedAwayBlindingDistance : _blindingOverDistance;
			AnimationCurve deafenCurve = turnedAway ? _turnedAwayDeafenDurationOverDistance : _deafenDurationOverDistance;

			float blindFactor = 0f;
			if (blindingCurve != null)
				blindFactor = blindingCurve.Evaluate(distance);

			if (_blindingOverDot != null)
				blindFactor *= _blindingOverDot.Evaluate(dot01);

			float deafenFactor = 0f;
			if (deafenCurve != null)
				deafenFactor = deafenCurve.Evaluate(distance);

			float finalBlindDuration = blindFactor * _blindTime;
			float finalDeafenDuration = deafenFactor * _blindTime;

			if (finalDeafenDuration >= _minimalEffectDuration)
			{
				hub.playerEffectsController.EnableEffect<CustomPlayerEffects.Deafened>(finalDeafenDuration, addDuration: true);
			}

			if (finalBlindDuration >= _minimalEffectDuration)
			{
				hub.playerEffectsController.EnableEffect<CustomPlayerEffects.Flashed>(finalBlindDuration, addDuration: true);
			}

			float blurDuration = Mathf.Max(0f, finalBlindDuration + _additionalBlurDuration);
			if (blurDuration >= _minimalEffectDuration)
			{
				hub.playerEffectsController.EnableEffect<CustomPlayerEffects.Blinded>(blurDuration, addDuration: true);
			}

			_hitPlayerCount++;
		}
	}
}
