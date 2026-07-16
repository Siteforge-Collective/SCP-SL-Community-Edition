namespace InventorySystem.Items.ThrowableProjectiles
{
	public class ExplosionGrenade : global::InventorySystem.Items.ThrowableProjectiles.EffectGrenade
	{
		[global::UnityEngine.Header("Hitreg")]
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.LayerMask _detectionMask;

		[global::UnityEngine.SerializeField]
		private float _maxRadius;

		[global::UnityEngine.SerializeField]
		private float _scpDamageMultiplier;

		[global::UnityEngine.Header("Curves")]
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _playerDamageOverDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _effectDurationOverDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _doorDamageOverDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _shakeOverDistance;

		[global::UnityEngine.Header("Player Effects")]
		[global::UnityEngine.SerializeField]
		private float _burnedDuration;

		[global::UnityEngine.SerializeField]
		private float _deafenedDuration;

		[global::UnityEngine.SerializeField]
		private float _concussedDuration;

		[global::UnityEngine.SerializeField]
		private float _minimalDuration;

		[global::UnityEngine.Header("Physics")]
		[global::UnityEngine.SerializeField]
		private float _rigidbodyBaseForce;

		[global::UnityEngine.SerializeField]
		private float _rigidbodyLiftForce;

		[global::UnityEngine.SerializeField]
		private float _humeShieldMultipler;

		private const float MinimalMass = 0.5f;

		private const float MaxMass = 10f;

		private const float MassFactor = 3f;

		public static event global::System.Action<global::Footprinting.Footprint, global::UnityEngine.Vector3, global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade> OnExploded;

		protected override void PlayExplosionEffects()
		{
			Explode(PreviousOwner, base.transform.position, this);
		}

		public override void ServerActivate()
		{
			base.ServerActivate();
		}

		public static void Explode(global::Footprinting.Footprint attacker, global::UnityEngine.Vector3 position, global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade settingsReference)
		{
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.GrenadeExploded, attacker, position, settingsReference))
			{
				return;
			}
			global::System.Collections.Generic.HashSet<uint> hashSet = global::NorthwoodLib.Pools.HashSetPool<uint>.Shared.Rent();
			global::System.Collections.Generic.HashSet<uint> hashSet2 = global::NorthwoodLib.Pools.HashSetPool<uint>.Shared.Rent();
			float maxRadius = settingsReference._maxRadius;
			global::UnityEngine.Collider[] array = global::UnityEngine.Physics.OverlapSphere(position, maxRadius, settingsReference._detectionMask);
			foreach (global::UnityEngine.Collider collider in array)
			{
				if (global::Mirror.NetworkServer.active)
				{
					if (collider.TryGetComponent<global::InventorySystem.Items.ThrowableProjectiles.IExplosionTrigger>(out var component))
					{
						component.OnExplosionDetected(attacker, position, maxRadius);
					}
					global::Interactables.InteractableCollider component3;
					if (collider.TryGetComponent<IDestructible>(out var component2))
					{
						if (!hashSet.Contains(component2.NetworkId) && ExplodeDestructible(component2, attacker, position, settingsReference))
						{
							hashSet.Add(component2.NetworkId);
						}
					}
					else if (collider.TryGetComponent<global::Interactables.InteractableCollider>(out component3) && component3.Target is global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant && hashSet2.Add(doorVariant.netId))
					{
						ExplodeDoor(doorVariant, position, settingsReference);
					}
				}
				if (collider.attachedRigidbody != null)
				{
					ExplodeRigidbody(collider.attachedRigidbody, position, maxRadius, settingsReference);
				}
			}
			global::NorthwoodLib.Pools.HashSetPool<uint>.Shared.Return(hashSet);
			global::NorthwoodLib.Pools.HashSetPool<uint>.Shared.Return(hashSet2);
			global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade.OnExploded?.Invoke(attacker, position, settingsReference);
		}

		private static void ExplodeRigidbody(global::UnityEngine.Rigidbody rb, global::UnityEngine.Vector3 pos, float radius, global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade setts)
		{
			if (!rb.isKinematic && !global::UnityEngine.Physics.Linecast(rb.gameObject.transform.position, pos, global::InventorySystem.Items.MicroHID.MicroHIDItem.WallMask))
			{
				float num = global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Mathf.InverseLerp(0.5f, 10f, rb.mass)) * 3f + 1f;
				rb.AddExplosionForce(setts._rigidbodyBaseForce / num, pos, radius, setts._rigidbodyLiftForce / num, global::UnityEngine.ForceMode.VelocityChange);
			}
		}

		private static bool ExplodeDestructible(IDestructible dest, global::Footprinting.Footprint attacker, global::UnityEngine.Vector3 pos, global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade setts)
		{
			if (global::UnityEngine.Physics.Linecast(dest.CenterOfMass, pos, global::InventorySystem.Items.MicroHID.MicroHIDItem.WallMask))
			{
				return false;
			}
			global::UnityEngine.Vector3 vector = dest.CenterOfMass - pos;
			float magnitude = vector.magnitude;
			float num = setts._playerDamageOverDistance.Evaluate(magnitude);
			ReferenceHub hub;
			bool flag = ReferenceHub.TryGetHubNetID(dest.NetworkId, out hub);
			if (flag && global::PlayerRoles.PlayerRolesUtils.GetTeam(global::PlayerRoles.PlayerRolesUtils.GetRoleId(hub)) == global::PlayerRoles.Team.SCPs)
			{
				num *= setts._scpDamageMultiplier;
				if (hub.playerStats.TryGetModule<global::PlayerStatsSystem.HumeShieldStat>(out var module))
				{
					num = ((!(num * setts._humeShieldMultipler < module.CurValue)) ? (num + module.CurValue / setts._humeShieldMultipler) : (num * setts._humeShieldMultipler));
				}
			}
			global::UnityEngine.Vector3 force = (1f - magnitude / setts._maxRadius) * (vector / magnitude) * setts._rigidbodyBaseForce + global::UnityEngine.Vector3.up * setts._rigidbodyLiftForce;
			if (num > 0f && dest.Damage(num, new global::PlayerStatsSystem.ExplosionDamageHandler(attacker, force, num, 50), dest.CenterOfMass) && flag)
			{
				float num2 = setts._effectDurationOverDistance.Evaluate(magnitude);
				bool flag2 = attacker.Hub == hub;
				if (num2 > 0f && (flag2 || HitboxIdentity.CheckFriendlyFire(attacker.Role, global::PlayerRoles.PlayerRolesUtils.GetRoleId(hub))))
				{
					float minimalDuration = setts._minimalDuration;
					TriggerEffect<global::CustomPlayerEffects.Burned>(hub, num2 * setts._burnedDuration, minimalDuration);
					TriggerEffect<global::CustomPlayerEffects.Deafened>(hub, num2 * setts._deafenedDuration, minimalDuration);
					TriggerEffect<global::CustomPlayerEffects.Concussed>(hub, num2 * setts._concussedDuration, minimalDuration);
				}
				if (!flag2 && attacker.Hub != null)
				{
					Hitmarker.SendHitmarker(attacker.Hub, 1f);
				}
				hub.inventory.connectionToClient.Send(new global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage(drawBlood: false, num, pos));
			}
			return true;
		}

		private static void ExplodeDoor(global::Interactables.Interobjects.DoorUtils.DoorVariant dv, global::UnityEngine.Vector3 pos, global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade setts)
		{
			if (dv is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor)
			{
				float time = global::UnityEngine.Vector3.Distance(dv.transform.position, pos);
				damageableDoor.ServerDamage(setts._doorDamageOverDistance.Evaluate(time), global::Interactables.Interobjects.DoorUtils.DoorDamageType.Grenade);
			}
		}

		private static void TriggerEffect<T>(ReferenceHub hub, float duration, float minimal) where T : global::CustomPlayerEffects.StatusEffectBase
		{
			if (!(duration < minimal))
			{
				hub.playerEffectsController.EnableEffect<T>(duration, addDuration: true);
			}
		}

		static ExplosionGrenade()
		{
			global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade.OnExploded = delegate
			{
			};
		}

		private void MirrorProcessed()
		{
		}
	}
}
