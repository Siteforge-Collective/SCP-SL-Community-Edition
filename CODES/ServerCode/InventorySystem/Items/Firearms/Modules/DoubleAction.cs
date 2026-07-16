namespace InventorySystem.Items.Firearms.Modules
{
	public class DoubleAction : global::InventorySystem.Items.Firearms.Modules.IActionModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private enum TriggerState
		{
			Released = 0,
			Pulling = 1,
			SearLock = 2
		}

		private readonly global::System.Diagnostics.Stopwatch _triggerStopwatch;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly global::UnityEngine.KeyCode _cockKey;

		private readonly float _triggerPullTime;

		private readonly int _cockingTriggerHash;

		private readonly float _cooldownAfterShot;

		private readonly float _cockingTime;

		private readonly byte _dryfireClip;

		private readonly byte _mechaClip;

		private readonly byte _cockClip;

		private bool _isCocked;

		private global::InventorySystem.Items.Firearms.Modules.DoubleAction.TriggerState _triggerState;

		private float _nextAllowedShot;

		private static readonly global::CameraShaking.RecoilSettings Recoil = new global::CameraShaking.RecoilSettings(0.1f, 10f, 1.02f, 3f, -0.21f);

		private float FireRateMultiplier => global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.FireRateMultiplier);

		private bool ServerTriggerReady
		{
			get
			{
				if (_nextAllowedShot <= global::UnityEngine.Time.timeSinceLevelLoad && _firearm.EquipperModule.Standby)
				{
					return _firearm.AmmoManagerModule.Standby;
				}
				return false;
			}
		}

		private bool ReadyToFire
		{
			get
			{
				if (ServerTriggerReady)
				{
					return _firearm.AdsModule.Standby;
				}
				return false;
			}
		}

		public global::InventorySystem.Items.Firearms.FirearmStatus PredictedStatus => _firearm.Status;

		public bool Standby
		{
			get
			{
				if (_nextAllowedShot <= global::UnityEngine.Time.timeSinceLevelLoad)
				{
					return !IsTriggerHeld;
				}
				return false;
			}
		}

		public float CyclicRate => FireRateMultiplier / (_cooldownAfterShot + _triggerPullTime);

		public bool IsTriggerHeld => _triggerState != global::InventorySystem.Items.Firearms.Modules.DoubleAction.TriggerState.Released;

		public bool Cocked
		{
			get
			{
				return _isCocked;
			}
			set
			{
				_isCocked = value;
				if (global::Mirror.NetworkServer.active && global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(_firearm.Status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked) != value)
				{
					global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = _firearm.Status.Flags;
					flags = ((!value) ? (flags & ~global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked) : (flags | global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked));
					_firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus(_firearm.Status.Ammo, flags, _firearm.Status.Attachments);
				}
			}
		}

		public DoubleAction(global::InventorySystem.Items.Firearms.Firearm selfRef, float triggerPullTime, float cooldownAfterShot, float cockingTime, string cockingTriggerName, byte dryfireClipIndex, byte mechaClipIndex, byte cockingClipIndex)
		{
			_firearm = selfRef;
			_triggerPullTime = triggerPullTime;
			_cooldownAfterShot = cooldownAfterShot;
			_cockingTime = cockingTime;
			_cockingTriggerHash = global::UnityEngine.Animator.StringToHash(cockingTriggerName);
			_dryfireClip = dryfireClipIndex;
			_mechaClip = mechaClipIndex;
			_cockClip = cockingClipIndex;
			_triggerStopwatch = new global::System.Diagnostics.Stopwatch();
			if (global::Mirror.NetworkServer.active)
			{
				global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Firearms.Modules.CockMessage>(ServerMsgReceived);
			}
			_firearm.OnShotCalled += ResetCocked;
			_firearm.OnDryfired += ResetCocked;
			_firearm.OnEquippedCalled += OnEquipped;
		}

		private void ResetCocked()
		{
			Cocked = false;
		}

		private void OnEquipped()
		{
			Cocked = global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(_firearm.Status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked);
			_triggerState = global::InventorySystem.Items.Firearms.Modules.DoubleAction.TriggerState.Released;
		}

		private void ServerMsgReceived(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Firearms.Modules.CockMessage cock)
		{
			if (ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) && hub.inventory.CurInstance is global::InventorySystem.Items.Firearms.Firearm firearm && firearm.ActionModule is global::InventorySystem.Items.Firearms.Modules.DoubleAction doubleAction && !doubleAction.Cocked)
			{
				firearm.ServerSendAudioMessage(_cockClip);
				doubleAction.Cocked = true;
			}
		}

		public global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
		{
			return global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse.Idle;
		}

		public bool ServerAuthorizeShot()
		{
			if ((ServerTriggerReady || _firearm.IsLocalPlayer) && _firearm.Status.Ammo > 0)
			{
				_firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)(_firearm.Status.Ammo - 1), _firearm.Status.Flags, _firearm.Status.Attachments);
				_nextAllowedShot = global::UnityEngine.Time.timeSinceLevelLoad + _cooldownAfterShot;
				_firearm.ServerSendAudioMessage((byte)global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.ShotClipIdOverride));
				return true;
			}
			return false;
		}

		public bool ServerAuthorizeDryFire()
		{
			if (ServerTriggerReady && _firearm.Status.Ammo == 0)
			{
				_nextAllowedShot = global::UnityEngine.Time.timeSinceLevelLoad + _cooldownAfterShot;
				_firearm.ServerSendAudioMessage(_dryfireClip);
				return true;
			}
			return false;
		}
	}
}
