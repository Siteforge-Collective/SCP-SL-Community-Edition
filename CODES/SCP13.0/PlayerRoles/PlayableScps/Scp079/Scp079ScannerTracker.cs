using System;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ScannerTracker : Scp079AbilityBase
	{
		private const int InitialTrackerSize = 32;

		private Scp079ScannerSequence _sequence;

		private int _lastRefreshedIndex;

		private bool _sequenceActive;

		private string _statusScanning;

		private string _statusNextScan;

		private string _statusDisabled;

		[SerializeField]
		private float _sequenceTime;

		[SerializeField]
		private float _warningTime;

		[SerializeField]
		private float _maxCampingTime;

		[SerializeField]
		private float _areaBaselineRadius;

		[SerializeField]
		private float _areaAdditiveRadius;

		[SerializeField]
		private float _addZonesPenalty;

		[SerializeField]
		private float _scannedEffectDuration;

		[SerializeField]
		private AudioClip _alarmSound;

		[SerializeField]
		private float _alarmHeight;

		[SerializeField]
		private float _alarmRange;

		public Scp079ScannerTrackedPlayer[] TrackedPlayers;

		public string StatusText => null;

		public event Action<ReferenceHub> OnDetected
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

		private void AddTarget(ReferenceHub hub)
		{
		}

		private void RemoveTarget(ReferenceHub hub)
		{
		}

		private void OnRoleChanged(ReferenceHub ply, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void Update()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		protected override void Awake()
		{
		}

		internal void ClientProcessScanResult(ReferenceHub ply, int nextScan, NetworkReader data)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
