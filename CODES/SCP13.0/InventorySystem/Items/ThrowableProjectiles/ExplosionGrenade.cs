using System;
using System.Collections.Generic;
using CustomPlayerEffects;
using Footprinting;
using Interactables;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.MicroHID;
using Mirror;
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Events;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class ExplosionGrenade : EffectGrenade
	{
		[Header("Hitreg")]
		[SerializeField]
		private LayerMask _detectionMask;

		[SerializeField]
		private float _maxRadius;

		[SerializeField]
		private float _scpDamageMultiplier;

		[Header("Curves")]
		[SerializeField]
		private AnimationCurve _playerDamageOverDistance;

		[SerializeField]
		private AnimationCurve _effectDurationOverDistance;

		[SerializeField]
		private AnimationCurve _doorDamageOverDistance;

		[SerializeField]
		private AnimationCurve _shakeOverDistance;

		[Header("Player Effects")]
		[SerializeField]
		private float _burnedDuration;

		[SerializeField]
		private float _deafenedDuration;

		[SerializeField]
		private float _concussedDuration;

		[SerializeField]
		private float _minimalDuration;

		[Header("Physics")]
		[SerializeField]
		private float _rigidbodyBaseForce;

		[SerializeField]
		private float _rigidbodyLiftForce;

		[SerializeField]
		private float _humeShieldMultipler;

		private const float MinimalMass = 0.5f;

		private const float MaxMass = 10f;

		private const float MassFactor = 3f;

		public static event Action<Footprint, Vector3, ExplosionGrenade> OnExploded;

		protected override void ServerFuseEnd()
		{
			Explode(PreviousOwner, base.transform.position, this);
			base.ServerFuseEnd();
		}

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

		public static void Explode(Footprint attacker, Vector3 position, ExplosionGrenade settingsReference)
		{
			if (!EventManager.ExecuteEvent(new GrenadeExplodedEvent(attacker, position, settingsReference)))
			{
				return;
			}

			var processedDestructibles = HashSetPool<uint>.Shared.Rent();
			var processedDoors = HashSetPool<uint>.Shared.Rent();

			float maxRadius = settingsReference._maxRadius;
			Collider[] colliders = Physics.OverlapSphere(position, maxRadius, settingsReference._detectionMask);

			foreach (Collider collider in colliders)
			{
				if (NetworkServer.active)
				{
					if (collider.TryGetComponent<IExplosionTrigger>(out var trigger))
					{
						trigger.OnExplosionDetected(attacker, position, maxRadius);
					}

					if (collider.TryGetComponent<IDestructible>(out var destructible))
					{
						if (!processedDestructibles.Contains(destructible.NetworkId) && ExplodeDestructible(destructible, attacker, position, settingsReference))
						{
							processedDestructibles.Add(destructible.NetworkId);
						}
					}
					else if (collider.TryGetComponent<InteractableCollider>(out var interact) && interact.Target is DoorVariant doorVariant)
					{
						if (processedDoors.Add(doorVariant.netId))
						{
							ExplodeDoor(doorVariant, position, settingsReference);
						}
					}
				}

				if (collider.attachedRigidbody != null)
				{
					ExplodeRigidbody(collider.attachedRigidbody, position, maxRadius, settingsReference);
				}
			}

			HashSetPool<uint>.Shared.Return(processedDestructibles);
			HashSetPool<uint>.Shared.Return(processedDoors);

			OnExploded?.Invoke(attacker, position, settingsReference);
		}

		private static void ExplodeRigidbody(Rigidbody rb, Vector3 pos, float radius, ExplosionGrenade setts)
		{
			if (!rb.isKinematic && !Physics.Linecast(rb.gameObject.transform.position, pos, MicroHIDItem.WallMask))
			{
				float massFactor = Mathf.Clamp01(Mathf.InverseLerp(MinimalMass, MaxMass, rb.mass)) * MassFactor + 1f;
				rb.AddExplosionForce(setts._rigidbodyBaseForce / massFactor, pos, radius, setts._rigidbodyLiftForce / massFactor, ForceMode.VelocityChange);
			}
		}

		private static bool ExplodeDestructible(IDestructible dest, Footprint attacker, Vector3 pos, ExplosionGrenade setts)
		{
			if (Physics.Linecast(dest.CenterOfMass, pos, MicroHIDItem.WallMask))
			{
				return false;
			}

			Vector3 dir = dest.CenterOfMass - pos;
			float distance = dir.magnitude;
			float damage = setts._playerDamageOverDistance.Evaluate(distance);

			bool isPlayer = ReferenceHub.TryGetHubNetID(dest.NetworkId, out ReferenceHub hub);
			if (isPlayer && hub.GetRoleId().GetTeam() == Team.SCPs)
			{
				damage *= setts._scpDamageMultiplier;
				if (hub.playerStats.TryGetModule<HumeShieldStat>(out var hume))
				{
					damage = ((!(damage * setts._humeShieldMultipler < hume.CurValue)) ? (damage + hume.CurValue / setts._humeShieldMultipler) : (damage * setts._humeShieldMultipler));
				}
			}

			Vector3 force = (1f - distance / setts._maxRadius) * (dir / (distance > 0f ? distance : 1f)) * setts._rigidbodyBaseForce + Vector3.up * setts._rigidbodyLiftForce;

			if (damage > 0f && dest.Damage(damage, new ExplosionDamageHandler(attacker, force, damage, 50), dest.CenterOfMass) && isPlayer)
			{
				float durationFactor = setts._effectDurationOverDistance.Evaluate(distance);
				bool selfHit = attacker.Hub == hub;

				if (durationFactor > 0f && (selfHit || HitboxIdentity.IsDamageable(attacker.Role, hub.GetRoleId())))
				{
					float minimal = setts._minimalDuration;
					TriggerEffect<Burned>(hub, durationFactor * setts._burnedDuration, minimal);
					TriggerEffect<Deafened>(hub, durationFactor * setts._deafenedDuration, minimal);
					TriggerEffect<Concussed>(hub, durationFactor * setts._concussedDuration, minimal);
				}

				if (!selfHit && attacker.Hub != null)
				{
					Hitmarker.SendHitmarkerDirectly(attacker.Hub, 1f);
				}
			}

			return true;
		}

		private static void ExplodeDoor(DoorVariant dv, Vector3 pos, ExplosionGrenade setts)
		{
			if (dv is IDamageableDoor damageableDoor)
			{
				float distance = Vector3.Distance(dv.transform.position, pos);
				float damage = setts._doorDamageOverDistance != null ? setts._doorDamageOverDistance.Evaluate(distance) : 0f;
				damageableDoor.ServerDamage(damage, DoorDamageType.Grenade);
			}
		}

		private static void TriggerEffect<T>(ReferenceHub hub, float duration, float minimal) where T : StatusEffectBase
		{
			if (duration >= minimal)
			{
				hub.playerEffectsController.EnableEffect<T>(duration, addDuration: true);
			}
		}
	}
}
