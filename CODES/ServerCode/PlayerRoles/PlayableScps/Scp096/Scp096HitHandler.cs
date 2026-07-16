namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096HitHandler
	{
		private static readonly global::UnityEngine.Collider[] Hits = new global::UnityEngine.Collider[32];

		private static readonly CachedLayerMask SolidObjectMask = new CachedLayerMask("Default", "Door", "Glass");

		private static readonly CachedLayerMask AttackHitMask = new CachedLayerMask("Hitbox", "Door", "Glass");

		private readonly global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker _targetCounter;

		private readonly global::System.Collections.Generic.HashSet<uint> _hitNetIDs;

		private readonly global::PlayerRoles.PlayableScps.Scp096.Scp096Role _scpRole;

		private readonly float _windowDamage;

		private readonly float _doorDamage;

		private readonly float _humanTargetDamage;

		private readonly float _humanNontargetDamage;

		private readonly global::PlayerStatsSystem.Scp096DamageHandler.AttackType _damageType;

		public global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult HitResult { get; private set; }

		public event global::System.Action<ReferenceHub> OnPlayerHit;

		public event global::System.Action<BreakableWindow> OnWindowHit;

		public event global::System.Action<global::Interactables.Interobjects.DoorUtils.IDamageableDoor> OnDoorHit;

		public Scp096HitHandler(global::PlayerRoles.PlayableScps.Scp096.Scp096Role scpRole, global::PlayerStatsSystem.Scp096DamageHandler.AttackType damageType, float windowDamage, float doorDamage, float humanTargetDamage, float humanNontargetDamage)
		{
			_scpRole = scpRole;
			_damageType = damageType;
			_windowDamage = windowDamage;
			_doorDamage = doorDamage;
			_humanTargetDamage = humanTargetDamage;
			_humanNontargetDamage = humanNontargetDamage;
			HitResult = global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.None;
			_hitNetIDs = new global::System.Collections.Generic.HashSet<uint>();
			_scpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker>(out _targetCounter);
		}

		public void Clear()
		{
			_hitNetIDs.Clear();
			HitResult = global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.None;
		}

		public global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult DamageSphere(global::UnityEngine.Vector3 position, float radius)
		{
			return ProcessHits(global::UnityEngine.Physics.OverlapSphereNonAlloc(position, radius, Hits, AttackHitMask));
		}

		public global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult DamageBox(global::UnityEngine.Vector3 position, global::UnityEngine.Vector3 halfExtents, global::UnityEngine.Quaternion orientation)
		{
			return ProcessHits(global::UnityEngine.Physics.OverlapBoxNonAlloc(position, halfExtents, Hits, orientation, AttackHitMask));
		}

		private global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult ProcessHits(int count)
		{
			global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult scp096HitResult = global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.None;
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.Collider collider = Hits[i];
				CheckDoorHit(collider);
				if (!collider.TryGetComponent<IDestructible>(out var component))
				{
					continue;
				}
				int layerMask = (int)SolidObjectMask & ~(1 << collider.gameObject.layer);
				if (global::UnityEngine.Physics.Linecast(_scpRole.CameraPosition, component.CenterOfMass, layerMask) || !_hitNetIDs.Add(component.NetworkId))
				{
					continue;
				}
				if (component is BreakableWindow breakableWindow)
				{
					if (DealDamage(breakableWindow, _windowDamage))
					{
						scp096HitResult |= global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.Window;
						this.OnWindowHit?.Invoke(breakableWindow);
					}
				}
				else
				{
					if (!(component is HitboxIdentity hitboxIdentity) || !IsHumanHitbox(hitboxIdentity))
					{
						continue;
					}
					ReferenceHub targetHub = hitboxIdentity.TargetHub;
					bool flag = _targetCounter.HasTarget(targetHub);
					if (DealDamage(hitboxIdentity, flag ? _humanTargetDamage : _humanNontargetDamage))
					{
						scp096HitResult |= global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.Human;
						this.OnPlayerHit?.Invoke(targetHub);
						if (!targetHub.IsAlive())
						{
							scp096HitResult |= global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.Lethal;
						}
					}
				}
			}
			HitResult |= scp096HitResult;
			return scp096HitResult;
		}

		private bool DealDamage(IDestructible target, float dmg)
		{
			if (dmg <= 0f)
			{
				return false;
			}
			global::PlayerStatsSystem.Scp096DamageHandler handler = new global::PlayerStatsSystem.Scp096DamageHandler(_scpRole, dmg, _damageType);
			return target.Damage(dmg, handler, _scpRole.FpcModule.Position);
		}

		private void CheckDoorHit(global::UnityEngine.Collider col)
		{
			if (col.TryGetComponent<global::Interactables.InteractableCollider>(out var component) && component.Target is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor && damageableDoor is global::Mirror.NetworkBehaviour networkBehaviour && _hitNetIDs.Add(networkBehaviour.netId) && damageableDoor.ServerDamage(_doorDamage, global::Interactables.Interobjects.DoorUtils.DoorDamageType.Scp096))
			{
				HitResult |= global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.Door;
				this.OnDoorHit?.Invoke(damageableDoor);
			}
		}

		private bool IsHumanHitbox(HitboxIdentity hid)
		{
			return hid.TargetHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole;
		}
	}
}
