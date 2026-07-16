using MapGeneration;
using Mirror;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ScannerSequence
	{
		private enum ScanSequenceStep
		{
			Init = 0,
			CountingDown = 1,
			ScanningFindNewTarget = 2,
			ScanningFailedCooldown = 3,
			ScanningUpdateTarget = 4
		}

		private enum TrackerMessage
		{
			None = 0,
			ScannerDisabled = 1,
			ScanTimeSync = 2,
			ScanNoResults = 3,
			ScanSuccessful = 4
		}

		private const float TotalCountdownTime = 20f;

		private const float AddZonesDuringCooldownPenalty = 4f;

		private const float ScanningTime = 3.2f;

		private const float ScannedEffectDuration = 7.5f;

		private readonly Scp079ScannerMenuToggler _menuToggler;

		private readonly Scp079ScannerZoneSelector _zoneSelector;

		private readonly Scp079ScannerTeamFilterSelector _teamSelector;

		private readonly Scp079ScannerTracker _tracker;

		private int _prevZonesCnt;

		private int _detectedPlayer;

		private double _nextScanTime;

		private double _scanCompleteTime;

		private bool _wasEnabled;

		private Team[] _teamsToDetect;

		private FacilityZone[] _zonesToDetect;

		private ScanSequenceStep _curStep;

		private TrackerMessage _rpcToSend;

		public bool SequencePaused { get; private set; }

		public float RemainingTime => 0f;

		private bool ScanningPossible => false;

		private Scp079ScannerTrackedPlayer[] TrackedPlayers => null;

		private TrackerMessage UpdateSequence()
		{
			return default(TrackerMessage);
		}

		public Scp079ScannerSequence(Scp079Role role)
		{
		}

		public void ServerUpdate(out bool rpcRequested)
		{
			rpcRequested = default(bool);
		}

		public void WriteRpc(NetworkWriter writer)
		{
		}

		public void ReadRpc(NetworkReader reader)
		{
		}
	}
}
