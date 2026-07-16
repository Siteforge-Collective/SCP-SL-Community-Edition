namespace InventorySystem.Items.Firearms.Modules
{
	public class PumpAction : global::InventorySystem.Items.Firearms.Modules.IActionModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private const float ServerToleranceBetweenShots = 0.8f;

		private const float ServerTolerancePumpSpeed = 0.9f;

		private const int PredictionOverrideMilliseconds = 400;

		private static bool _resetEventAssigned;

		private static readonly global::System.Collections.Generic.Dictionary<ushort, int> ChamberedRoundsBySerial = new global::System.Collections.Generic.Dictionary<ushort, int>();

		private static readonly global::System.Collections.Generic.Dictionary<ushort, int> CockedHammersBySerial = new global::System.Collections.Generic.Dictionary<ushort, int>();

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly global::CameraShaking.RecoilSettings _recoil;

		private readonly int _chambersNumber;

		private readonly float _timeBetweenShots;

		private readonly float _pumpingTime;

		private readonly int _pumpAnimHash;

		private readonly global::System.Diagnostics.Stopwatch _predictedStatusStopwatch;

		private readonly global::System.Diagnostics.Stopwatch _lastShotStopwatch;

		private readonly global::System.Diagnostics.Stopwatch _pumpStopwatch;

		private readonly ushort _serial;

		private readonly int _triggerClip;

		private readonly int _dryfireClip;

		private bool _isTriggerReady;

		private global::InventorySystem.Items.Firearms.FirearmStatus _predictedStatus;

		private float PumpSpeedMultiplier => global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.FireRateMultiplier) * global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ReloadSpeedMultiplier);

		private float TimeBetweenShots => (_firearm.IsLocalPlayer ? _timeBetweenShots : (_timeBetweenShots * 0.8f)) / global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.FireRateMultiplier);

		private float PumpingTime => (_firearm.IsLocalPlayer ? _pumpingTime : (_pumpingTime * 0.9f)) / PumpSpeedMultiplier;

		private bool ModulesReady
		{
			get
			{
				if (_firearm.AmmoManagerModule.Standby && _firearm.EquipperModule.Standby)
				{
					return _firearm.AdsModule.Standby;
				}
				return false;
			}
		}

		private byte ShotSoundId => (byte)global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ShotClipIdOverride);

		public global::InventorySystem.Items.Firearms.FirearmStatus PredictedStatus
		{
			get
			{
				if (global::Mirror.NetworkServer.active)
				{
					return _firearm.Status;
				}
				if (_predictedStatusStopwatch.ElapsedMilliseconds > 400)
				{
					_predictedStatus = _firearm.Status;
				}
				return _predictedStatus;
			}
			private set
			{
				_predictedStatus = value;
				_predictedStatusStopwatch.Restart();
			}
		}

		public int ChamberedRounds
		{
			get
			{
				if (!ChamberedRoundsBySerial.TryGetValue(_serial, out var value))
				{
					return 0;
				}
				return value;
			}
			set
			{
				ChamberedRoundsBySerial[_serial] = value;
			}
		}

		public int CockedHammers
		{
			get
			{
				if (!CockedHammersBySerial.TryGetValue(_serial, out var value))
				{
					return 0;
				}
				return value;
			}
			set
			{
				CockedHammersBySerial[_serial] = value;
			}
		}

		public int LastFiredAmount { get; private set; }

		public bool IsTriggerHeld { get; private set; }

		public float CyclicRate => 1f / (_pumpingTime / (float)_chambersNumber + _timeBetweenShots * (float)(_chambersNumber - 1));

		public bool Standby
		{
			get
			{
				if (_lastShotStopwatch.Elapsed.TotalSeconds >= (double)TimeBetweenShots)
				{
					return _pumpStopwatch.Elapsed.TotalSeconds >= (double)PumpingTime;
				}
				return false;
			}
		}

		public int AmmoUsage => global::UnityEngine.Mathf.RoundToInt(global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AmmoConsumptionMultiplier));

		public PumpAction(global::InventorySystem.Items.Firearms.Firearm selfRef, ushort serial, int numberOfChambers, float timeBetweenShots, float pumpingTime, global::CameraShaking.RecoilSettings recoil, string pumpTriggerName, int triggerClip, int dryfireClip)
		{
			_firearm = selfRef;
			_serial = serial;
			_chambersNumber = numberOfChambers;
			_timeBetweenShots = timeBetweenShots;
			_pumpingTime = pumpingTime;
			_recoil = recoil;
			_pumpAnimHash = global::UnityEngine.Animator.StringToHash(pumpTriggerName);
			_triggerClip = triggerClip;
			_dryfireClip = dryfireClip;
			_lastShotStopwatch = global::System.Diagnostics.Stopwatch.StartNew();
			_predictedStatusStopwatch = global::System.Diagnostics.Stopwatch.StartNew();
			_pumpStopwatch = global::System.Diagnostics.Stopwatch.StartNew();
			if (!ChamberedRoundsBySerial.ContainsKey(serial))
			{
				ChamberedRounds = 0;
			}
			if (global::Mirror.NetworkServer.active)
			{
				selfRef.OnEquippedCalled += ServerResync;
			}
			if (!_resetEventAssigned)
			{
				global::InventorySystem.Inventory.OnLocalClientStarted += ChamberedRoundsBySerial.Clear;
				_resetEventAssigned = true;
			}
		}

		public global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
		{
			return global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse.Idle;
		}

		public bool ServerAuthorizeDryFire()
		{
			if (ChamberedRounds > 0 || CockedHammers <= 0)
			{
				ServerResync();
				return false;
			}
			CockedHammers -= global::UnityEngine.Mathf.Min(CockedHammers, AmmoUsage);
			_firearm.ServerSendAudioMessage((byte)_dryfireClip);
			return true;
		}

		public bool ServerAuthorizeShot()
		{
			if (ChamberedRounds == 0 || _firearm.Status.Ammo == 0)
			{
				ServerResync();
				return false;
			}
			if (_lastShotStopwatch.Elapsed.TotalSeconds < (double)TimeBetweenShots || _pumpStopwatch.Elapsed.TotalSeconds < (double)PumpingTime)
			{
				return false;
			}
			LastFiredAmount = 0;
			int num = AmmoUsage;
			bool result = false;
			while (num > 0 && ChamberedRounds > 0 && _firearm.Status.Ammo > 0)
			{
				num--;
				ChamberedRounds--;
				CockedHammers--;
				LastFiredAmount++;
				if (ChamberedRounds > 0)
				{
					_lastShotStopwatch.Restart();
				}
				_firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)(_firearm.Status.Ammo - 1), _firearm.Status.Flags, _firearm.Status.Attachments);
				_firearm.ServerSendAudioMessage((byte)(ShotSoundId + ChamberedRounds));
				result = true;
				if (ChamberedRounds == 0 && _firearm.Status.Ammo > 0 && !_firearm.IsLocalPlayer)
				{
					_pumpStopwatch.Restart();
					_firearm.AnimSetTrigger(_pumpAnimHash);
					break;
				}
			}
			return result;
		}

		public void Pump(bool sendToClients)
		{
			int num = PredictedStatus.Ammo;
			if (ChamberedRounds > 0)
			{
				num = _firearm.Status.Ammo - ChamberedRounds;
				PredictedStatus = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)num, PredictedStatus.Flags, PredictedStatus.Attachments);
				if (global::Mirror.NetworkServer.active)
				{
					_firearm.OwnerInventory.ServerAddAmmo(_firearm.AmmoType, ChamberedRounds);
				}
			}
			CockedHammers = _chambersNumber;
			ChamberedRounds = global::UnityEngine.Mathf.Min(num, _chambersNumber);
			if (global::Mirror.NetworkServer.active)
			{
				_firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)num, _firearm.Status.Flags | global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked, _firearm.Status.Attachments);
				if (sendToClients)
				{
					ServerResync();
				}
			}
		}

		public void ClientProcessMessage(byte msgCode)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				ChamberedRounds = msgCode;
			}
		}

		private void ServerResync()
		{
			_firearm.OwnerInventory.connectionToClient.Send(new global::InventorySystem.Items.Firearms.Modules.ShotgunResyncMessage(_serial, ChamberedRounds, CockedHammers));
		}
	}
}
