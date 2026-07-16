using System;
using System.Collections.Generic;
using Mirror;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace InventorySystem.Items.Jailbird
{
	[Serializable]
	public class JailbirdHitreg
	{
		private const int MaxDetections = 128;

		private static readonly Collider[] DetectedColliders;

		private static readonly IDestructible[] DetectedDestructibles;

		private static readonly CachedLayerMask DetectionMask;

		private static readonly CachedLayerMask LinecastMask;

		private static readonly HashSet<uint> DetectedNetIds;

		private static readonly HashSet<FpcBacktracker> BacktrackedPlayers;

		private static int _detectionsLen;

		[SerializeField]
		private float _hitregOffset;

		[SerializeField]
		private float _hitregRadius;

		[SerializeField]
		private float _damageMelee;

		[SerializeField]
		private float _damageCharge;

		[Tooltip("How long in seconds the 'concussed' effect is applied for on attacked targets.")]
		[SerializeField]
		private float _concussionDuration;

		[Tooltip("How long in seconds the 'flashed' effect is applied for on attacked targets.")]
		[SerializeField]
		private float _flashedDuration;

		private JailbirdItem _item;

		public float TotalMeleeDamageDealt { get; internal set; }

		public bool AnyDetected => false;

		public void Setup(JailbirdItem target)
		{
		}

		public bool ClientTryAttack()
		{
			return false;
		}

		public bool ServerAttack(bool isCharging, NetworkReader reader)
		{
			return false;
		}

		private void DetectDestructibles()
		{
		}
	}
}
