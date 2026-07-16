using System.Collections.Generic;
using Interactables;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096ChargeAbility : KeySubroutine<Scp096Role>
	{
		private static readonly Collider[] DoorDetections;

		private static readonly CachedLayerMask ClientsideDoorDetectorMask;

		private static readonly HashSet<Collider> DisabledColliders;

		public const float DefaultChargeCooldown = 5f;

		private const float DefaultChargeDuration = 1f;

		private const float DamageObjects = 750f;

		private const float DamageTarget = 90f;

		private const float DamageNonTarget = 35f;

		private const float ConcussionDurationTargets = 10f;

		private const float ConcussionDurationNonTargets = 4f;

		private Scp096HitHandler _hitHandler;

		private Scp096TargetsTracker _targetsTracker;

		private Scp096AudioPlayer _audioPlayer;

		private Transform _tr;

		[SerializeField]
		private Vector3 _detectionOffset;

		[SerializeField]
		private Vector3 _detectionExtents;

		[SerializeField]
		private AudioClip[] _soundsLethal;

		[SerializeField]
		private AudioClip[] _soundsNonLethal;

		[SerializeField]
		private float _soundDistance;

		public readonly AbilityCooldown Cooldown;

		public readonly AbilityCooldown Duration;

		public bool CanCharge => false;

		protected override ActionName TargetKey => default(ActionName);

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		private void UpdateServer()
		{
		}

		private void UpdateLocalClient()
		{
		}

		private void CheckDoor(IInteractable inter)
		{
		}
	}
}
