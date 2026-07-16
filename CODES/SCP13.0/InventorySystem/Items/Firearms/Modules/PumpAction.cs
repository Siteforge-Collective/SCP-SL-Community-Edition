using System.Collections.Generic;
using System.Diagnostics;
using CameraShaking;

namespace InventorySystem.Items.Firearms.Modules
{
	public class PumpAction : IActionModule, IFirearmModuleBase
	{
		private const float ServerToleranceBetweenShots = 0.8f;

		private const float ServerTolerancePumpSpeed = 0.9f;

		private const int PredictionOverrideMilliseconds = 400;

		private static bool _resetEventAssigned;

		private static readonly Dictionary<ushort, int> ChamberedRoundsBySerial;

		private static readonly Dictionary<ushort, int> CockedHammersBySerial;

		private readonly Firearm _firearm;

		private readonly RecoilSettings _recoil;

		private readonly int _chambersNumber;

		private readonly float _timeBetweenShots;

		private readonly float _pumpingTime;

		private readonly int _pumpAnimHash;

		private readonly Stopwatch _predictedStatusStopwatch;

		private readonly Stopwatch _lastShotStopwatch;

		private readonly Stopwatch _pumpStopwatch;

		private readonly ushort _serial;

		private readonly int _triggerClip;

		private readonly int _dryfireClip;

		private bool _isTriggerReady;

		private FirearmStatus _predictedStatus;

		private float PumpSpeedMultiplier => 0f;

		private float TimeBetweenShots => 0f;

		private float PumpingTime => 0f;

		private bool ModulesReady => false;

		private byte ShotSoundId => 0;

		public FirearmStatus PredictedStatus
		{
			get
			{
				return default(FirearmStatus);
			}
			private set
			{
			}
		}

		public int ChamberedRounds
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int CockedHammers
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int LastFiredAmount { get; private set; }

		public bool IsTriggerHeld { get; private set; }

		public float CyclicRate => 0f;

		public bool Standby => false;

		public int AmmoUsage => 0;

		public PumpAction(Firearm selfRef, ushort serial, int numberOfChambers, float timeBetweenShots, float pumpingTime, RecoilSettings recoil, string pumpTriggerName, int triggerClip, int dryfireClip)
		{
		}

		public ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
		{
			return default(ActionModuleResponse);
		}

		public bool ServerAuthorizeDryFire()
		{
			return false;
		}

		public bool ServerAuthorizeShot()
		{
			return false;
		}

		public void Pump(bool sendToClients)
		{
		}

		public void ClientProcessMessage(byte msgCode)
		{
		}

		private ActionModuleResponse ClientTryPerformShot()
		{
			return default(ActionModuleResponse);
		}

		private void ServerResync()
		{
		}
	}
}
