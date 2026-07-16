using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MapGeneration;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.GUI;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079CurrentCameraSync : StandardSubroutine<Scp079Role>, IScp079FailMessageProvider
	{
		public enum ClientSwitchState
		{
			None = 0,
			SwitchingRoom = 1,
			SwitchingZone = 2
		}

		public const float CostPerMeter = 0.16f;

		public const int CostPerFloor = 5;

		public const int CostPerZone = 10;

		public const int CostPerSkippedZone = 20;

		public static readonly FacilityZone[] ZoneQueue;

		private const float FloorHeight = 100f;

		private const int ErrorTranslationId = 2;

		private const float SameRoomSwitchDuration = 0.1f;

		private const float ZoneSwitchDuration = 0.98f;

		private const string DefaultCameraName = "079 CONT CHAMBER";

		private readonly Stopwatch _switchStopwatch;

		private Scp079Camera _lastCam;

		private Scp079Camera _switchTarget;

		private Scp079AuxManager _auxManager;

		private Scp079LostSignalHandler _lostSignalHandler;

		private bool _camSet;

		private bool _eventAssigned;

		private float _targetSwitchTime;

		private ushort _defaultCamId;

		private ushort _requestedCamId;

		private bool _initialized;

		private Scp079HudTranslation _errorCode;

		private ClientSwitchState _clientSwitchRequest;

		public string FailMessage { get; private set; }

		public ClientSwitchState CurClientSwitchState { get; private set; }

		public FacilityZone CurClientTargetZone { get; private set; }

		public Scp079Camera CurrentCamera
		{
			get
			{
				return null;
			}
			private set
			{
			}
		}

		public event Action OnCameraChanged
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

		private void Update()
		{
		}

		private bool TryGetDefaultCamera(out Scp079Camera cam)
		{
			cam = null;
			return false;
		}

		private void OnHubAdded(ReferenceHub hub)
		{
		}

		public int GetSwitchCost(Scp079Camera target)
		{
			return 0;
		}

		public void ClientSwitchTo(Scp079Camera target)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public bool TryGetCurrentCamera(out Scp079Camera cam)
		{
			cam = null;
			return false;
		}

		public void OnFailMessageAssigned()
		{
		}
	}
}
