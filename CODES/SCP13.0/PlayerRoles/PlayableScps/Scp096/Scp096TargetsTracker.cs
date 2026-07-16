using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096TargetsTracker : StandardSubroutine<Scp096Role>
	{
		private const float Vision096InnerAngle = 0.1f;

		private const float VisionTriggerDistance = 60f;

		private const float HeadSize = 0.12f;

		private const float PostRageCooldownDuration = 10f;

		public GameObject TargetMarker;

		public readonly HashSet<ReferenceHub> Targets;

		private readonly AbilityCooldown _postRageCooldown;

		private readonly Dictionary<ReferenceHub, GameObject> _markers;

		private readonly HashSet<ReferenceHub> _unvalidatedTargets;

		[SerializeField]
		private AudioClip _targetSound;

		private bool _sendTargetsNextFrame;

		private bool _eventsAssigned;

		public bool CanReceiveTargets => false;

		public static event Action<ReferenceHub, ReferenceHub> OnTargetAdded
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

		public event Action<ReferenceHub> OnTargetAttacked
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

		public static event Action<ReferenceHub, ReferenceHub> OnTargetRemoved
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

		public bool AddTarget(ReferenceHub target, bool isLooking)
		{
			return false;
		}

		public bool RemoveTarget(ReferenceHub target)
		{
			return false;
		}

		public void ClearAllTargets()
		{
		}

		public bool IsObservedBy(ReferenceHub target)
		{
			return false;
		}

		public bool HasTarget(ReferenceHub target)
		{
			return false;
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		protected override void Awake()
		{
		}

		private void PlayTargetSound()
		{
		}

		private void AddTargetOnDamage(DamageHandlerBase obj)
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void CheckRemovedPlayer(ReferenceHub ply)
		{
		}

		private void ServerCheckTargets()
		{
		}

		private void UpdateTarget(ReferenceHub target)
		{
		}
	}
}
