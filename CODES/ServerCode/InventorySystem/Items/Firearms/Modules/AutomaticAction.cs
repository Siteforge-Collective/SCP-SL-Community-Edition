namespace InventorySystem.Items.Firearms.Modules
{
	public class AutomaticAction : global::InventorySystem.Items.Firearms.Modules.IActionModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
		public struct RefusedShotMessage : global::Mirror.NetworkMessage
		{
		}

		private const float ServerFirerateTolerance = 0.03f;

		private const float StatusUpdateTime = 0.4f;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly global::System.Diagnostics.Stopwatch _lastUpdateStopwatch;

		private readonly bool _semiAuto;

		private readonly bool _hasBoltLock;

		private readonly float _boltTravelTime;

		private readonly float _defaultTimeBetweenShots;

		private readonly byte _dryfireClip;

		private readonly byte _triggerClip;

		private readonly global::CameraShaking.RecoilSettings _recoilSettings;

		private readonly global::InventorySystem.Items.Firearms.FirearmRecoilPattern _recoilPattern;

		private readonly bool _usesRecoilPattern;

		private readonly float _gunshotRandomVal;

		private readonly global::System.Collections.Generic.Queue<float> _queuedShots;

		private readonly int _ammoConsumption;

		private global::InventorySystem.Items.Firearms.FirearmStatus _predictedStatus;

		private bool _hammerReady;

		private double _lastShotTime;

		private float _serverLastSuccessfulShot;

		public global::InventorySystem.Items.Firearms.FirearmStatus PredictedStatus
		{
			get
			{
				if (_lastShotTime >= 0.4000000059604645 || !_firearm.IsLocalPlayer)
				{
					_predictedStatus = _firearm.Status;
				}
				return _predictedStatus;
			}
			private set
			{
				_predictedStatus = value;
			}
		}

		private float CooldownBetweenShots => _defaultTimeBetweenShots / FireRateMultiplier;

		private float FireRateMultiplier => global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.FireRateMultiplier);

		private byte ShotClipId => (byte)global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ShotClipIdOverride);

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

		public bool Standby
		{
			get
			{
				if (_firearm.IsLocalPlayer)
				{
					if (_lastShotTime > (double)CooldownBetweenShots)
					{
						return _queuedShots.Count == 0;
					}
					return false;
				}
				return true;
			}
		}

		public float CyclicRate => 1f / _defaultTimeBetweenShots * FireRateMultiplier * (float)_ammoConsumption;

		public bool IsTriggerHeld { get; private set; }

		public AutomaticAction(global::InventorySystem.Items.Firearms.Firearm selfRef, bool semiAuto, float boltTravelTime, float cooldownBetweenShots, byte dryfireClip, byte triggerClip, float gunshotPitchRandomization, global::CameraShaking.RecoilSettings recoilSettings, global::InventorySystem.Items.Firearms.FirearmRecoilPattern recoilPattern, bool hasBoltLock, int consumption)
		{
			_firearm = selfRef;
			_semiAuto = semiAuto;
			_boltTravelTime = boltTravelTime;
			_defaultTimeBetweenShots = cooldownBetweenShots;
			_dryfireClip = dryfireClip;
			_triggerClip = triggerClip;
			_recoilSettings = recoilSettings;
			_recoilPattern = recoilPattern;
			_usesRecoilPattern = recoilPattern != null;
			_gunshotRandomVal = gunshotPitchRandomization;
			_hasBoltLock = hasBoltLock;
			_ammoConsumption = consumption;
			_lastShotTime = 0.4000000059604645;
			_queuedShots = new global::System.Collections.Generic.Queue<float>();
			_lastUpdateStopwatch = global::System.Diagnostics.Stopwatch.StartNew();
		}

		public global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
		{
			return global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse.Idle;
		}

		public bool ServerAuthorizeShot()
		{
			if (_firearm.Status.Ammo < _ammoConsumption)
			{
				return false;
			}
			if (!ServerCheckFirerate())
			{
				return false;
			}
			if (!ModulesReady)
			{
				_firearm.Owner.gameConsoleTransmission.SendToClient($"Shot rejected, ammoManager={_firearm.AmmoManagerModule.Standby}, equipperModule={_firearm.EquipperModule.Standby}, adsModule={_firearm.AdsModule.Standby}", "gray");
				return false;
			}
			global::InventorySystem.Items.Firearms.FirearmStatusFlags firearmStatusFlags = _firearm.Status.Flags;
			if (_firearm.Status.Ammo - _ammoConsumption < _ammoConsumption && _boltTravelTime == 0f)
			{
				firearmStatusFlags &= ~global::InventorySystem.Items.Firearms.FirearmStatusFlags.Chambered;
			}
			_firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)(_firearm.Status.Ammo - _ammoConsumption), firearmStatusFlags, _firearm.Status.Attachments);
			_firearm.ServerSendAudioMessage(ShotClipId);
			return true;
		}

		public bool ServerAuthorizeDryFire()
		{
			if ((!ServerCheckFirerate() || _firearm.Status.Ammo != 0 || !ModulesReady) && !_firearm.IsLocalPlayer)
			{
				return false;
			}
			global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = _firearm.Status.Flags;
			if (!global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked))
			{
				return false;
			}
			flags &= ~global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked;
			_firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus(0, flags, _firearm.Status.Attachments);
			_firearm.ServerSendAudioMessage(_dryfireClip);
			return true;
		}

		private bool ServerCheckFirerate()
		{
			float timeSinceLevelLoad = global::UnityEngine.Time.timeSinceLevelLoad;
			float num = timeSinceLevelLoad - _serverLastSuccessfulShot;
			if (num < -0.03f * CooldownBetweenShots)
			{
				_firearm.OwnerInventory.connectionToClient.Send(default(global::InventorySystem.Items.Firearms.Modules.AutomaticAction.RefusedShotMessage));
				return false;
			}
			_serverLastSuccessfulShot = timeSinceLevelLoad + CooldownBetweenShots - global::UnityEngine.Mathf.Min(num, CooldownBetweenShots);
			return true;
		}
	}
}
