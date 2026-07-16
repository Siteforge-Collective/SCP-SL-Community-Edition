using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Interactables.Interobjects.DoorUtils;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096HitHandler
	{
		private static readonly Collider[] Hits;

		private static readonly CachedLayerMask SolidObjectMask;

		private static readonly CachedLayerMask AttackHitMask;

		private readonly Scp096TargetsTracker _targetCounter;

		private readonly HashSet<uint> _hitNetIDs;

		private readonly Scp096Role _scpRole;

		private readonly float _windowDamage;

		private readonly float _doorDamage;

		private readonly float _humanTargetDamage;

		private readonly float _humanNontargetDamage;

		private readonly Scp096DamageHandler.AttackType _damageType;

		public Scp096HitResult HitResult { get; private set; }

		public event Action<ReferenceHub> OnPlayerHit
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public event Action<BreakableWindow> OnWindowHit
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public event Action<IDamageableDoor> OnDoorHit
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public Scp096HitHandler(Scp096Role scpRole, Scp096DamageHandler.AttackType damageType, float windowDamage, float doorDamage, float humanTargetDamage, float humanNontargetDamage)
		{
		}

		public void Clear()
		{
		}

		public Scp096HitResult DamageSphere(Vector3 position, float radius)
		{
			return default(Scp096HitResult);
		}

		public Scp096HitResult DamageBox(Vector3 position, Vector3 halfExtents, Quaternion orientation)
		{
			return default(Scp096HitResult);
		}

		private Scp096HitResult ProcessHits(int count)
		{
			return default(Scp096HitResult);
		}

		private bool DealDamage(IDestructible target, float dmg)
		{
			return false;
		}

		private void CheckDoorHit(Collider col)
		{
		}
	}
}
