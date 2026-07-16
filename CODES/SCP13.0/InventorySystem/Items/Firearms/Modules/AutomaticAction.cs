using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CameraShaking;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.BasicMessages;
using Mirror;
using UnityEngine;


namespace InventorySystem.Items.Firearms.Modules
{
	public class AutomaticAction : IActionModule, IFirearmModuleBase
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct RefusedShotMessage : NetworkMessage
		{
		}

		private const float ServerFirerateTolerance = 0.03f;

		private const float StatusUpdateTime = 0.4f;

		private readonly Firearm _firearm;

		private readonly Stopwatch _lastUpdateStopwatch;

		private readonly bool _semiAuto;

		private readonly bool _hasBoltLock;

		private readonly float _boltTravelTime;

		private readonly float _defaultTimeBetweenShots;

		private readonly byte _dryfireClip;

		private readonly byte _triggerClip;

		private readonly RecoilSettings _recoilSettings;

		private readonly FirearmRecoilPattern _recoilPattern;

		private readonly bool _usesRecoilPattern;

		private readonly float _gunshotRandomVal;

		private readonly Queue<float> _queuedShots;

		private readonly int _ammoConsumption;

		private FirearmStatus _predictedStatus;

		private bool _hammerReady;

		private double _lastShotTime;

		private float _serverLastSuccessfulShot;


        public FirearmStatus PredictedStatus
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

        private float CooldownBetweenShots => 0f;

		private float FireRateMultiplier => 0f;

		private byte ShotClipId => 0;

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

		public float FullautoInaccuracy => 0f;

        public AutomaticAction(Firearm selfRef, bool semiAuto, float boltTravelTime, float cooldownBetweenShots, byte dryfireClip, byte triggerClip, float gunshotPitchRandomization, RecoilSettings recoilSettings, FirearmRecoilPattern recoilPattern, bool hasBoltLock, int consumption)
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
            _queuedShots = new Queue<float>();
            _lastUpdateStopwatch = Stopwatch.StartNew();
        }


        private void ClientPlaySound(int index, bool isGunshot)
		{
		}

		private void ClientProcessReceivedStatus(FirearmStatus oldStatus, FirearmStatus newStatus)
		{
		}

		private void ClientModifyPredictedAmmo(int amount)
		{
		}

		private static void ClientShotRefused(RefusedShotMessage msg)
		{
		}

        public ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
        {
            return ActionModuleResponse.Idle;
        }

        public bool ServerAuthorizeShot()
        {
            if (_firearm.Owner.HasBlock(BlockedInteraction.ItemPrimaryAction))
            {
                return false;
            }
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
            FirearmStatusFlags firearmStatusFlags = _firearm.Status.Flags;
            if (_firearm.Status.Ammo - _ammoConsumption < _ammoConsumption && _boltTravelTime == 0f)
            {
                firearmStatusFlags &= ~FirearmStatusFlags.Chambered;
            }
            _firearm.Status = new FirearmStatus((byte)(_firearm.Status.Ammo - _ammoConsumption), firearmStatusFlags, _firearm.Status.Attachments);
            _firearm.ServerSendAudioMessage(ShotClipId);
            return true;
        }

        public bool ServerAuthorizeDryFire()
        {
            if ((!ServerCheckFirerate() || _firearm.Status.Ammo != 0 || !ModulesReady) && !_firearm.IsLocalPlayer)
            {
                return false;
            }
            FirearmStatusFlags flags = _firearm.Status.Flags;
            if (!flags.HasFlagFast(FirearmStatusFlags.Cocked))
            {
                return false;
            }
            flags &= ~FirearmStatusFlags.Cocked;
            _firearm.Status = new FirearmStatus(0, flags, _firearm.Status.Attachments);
            _firearm.ServerSendAudioMessage(_dryfireClip);
            return true;
        }

        private bool ServerCheckFirerate()
        {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            float num = timeSinceLevelLoad - _serverLastSuccessfulShot;
            if (num < -0.03f * CooldownBetweenShots)
            {
                _firearm.OwnerInventory.connectionToClient.Send(default(RefusedShotMessage));
                return false;
            }
            _serverLastSuccessfulShot = timeSinceLevelLoad + CooldownBetweenShots - Mathf.Min(num, CooldownBetweenShots);
            return true;
        }
    }
}
