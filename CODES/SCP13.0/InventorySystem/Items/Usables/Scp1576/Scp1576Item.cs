using System.Collections.Generic;
using System.Diagnostics;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp1576
{
	public class Scp1576Item : UsableItem
	{
		public const float TransmissionDuration = 30f;

		public const float UseCooldown = 120f;

		public const float HornReturnSpeed = 0.4f;

		public const float HornReturnDelay = 1.1f;

		public const float SqrAudibleRange = 110f;

		public const float WarningDuration = 2f;

		public static HashSet<ReferenceHub> ValidatedTransmitters;

		public static HashSet<ReferenceHub> ValidatedReceivers;

		private static readonly List<Vector3> ActiveNonAllocPositions;

		private static bool _locallyUsed;

		private static bool _eventAssigned;

		[SerializeField]
		private AudioClip _warningStart;

		[SerializeField]
		private AudioClip _warningStop;

		private float _serverHornPos;

		private bool _startWarningTriggered;

		private readonly Stopwatch _useStopwatch;

		public static bool LocallyUsed
		{
			get
			{
				return false;
			}
			internal set
			{
			}
		}

		public override bool AllowHolster => false;

		[field: SerializeField]
		public Scp1576Playback PlaybackTemplate { get; private set; }

		public override void ServerOnUsingCompleted()
		{
		}

		public override void OnUsingStarted()
		{
		}

		public override void OnUsingCancelled()
		{
		}

		public override bool ServerValidateStartRequest(PlayerHandler handler)
		{
			return false;
		}

		public override bool ServerValidateCancelRequest(PlayerHandler handler)
		{
			return false;
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void OnRemoved(ItemPickupBase pickup)
		{
		}

		public override void EquipUpdate()
		{
		}

		public override void OnHolstered()
		{
		}

		internal override void OnTemplateReloaded(bool wasEverLoaded)
		{
		}

		private void ServerStopTransmitting()
		{
		}

		private static void PlayWarningSound(AudioClip sound)
		{
		}

		private static void ContinueCheckingLocalUse()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RevalidateReceivers()
		{
		}
	}
}
