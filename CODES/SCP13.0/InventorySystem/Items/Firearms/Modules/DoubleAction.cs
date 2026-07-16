using System.Diagnostics;
using CameraShaking;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public class DoubleAction : IActionModule, IFirearmModuleBase
	{
		private enum TriggerState
		{
			Released = 0,
			Pulling = 1,
			SearLock = 2
		}

		private readonly Stopwatch _triggerStopwatch;

		private readonly Firearm _firearm;

		private readonly KeyCode _cockKey;

		private readonly float _triggerPullTime;

		private readonly int _cockingTriggerHash;

		private readonly float _cooldownAfterShot;

		private readonly float _cockingTime;

		private readonly byte _dryfireClip;

		private readonly byte _mechaClip;

		private readonly byte _cockClip;

		private bool _isCocked;

		private TriggerState _triggerState;

		private float _nextAllowedShot;

		private static readonly RecoilSettings Recoil;

		public float HammerPosition => 0f;

		private float FireRateMultiplier => 0f;

		private bool ServerTriggerReady => false;

		private bool ReadyToFire => false;

		public FirearmStatus PredictedStatus => default(FirearmStatus);

		public bool Standby => false;

		public float CyclicRate => 0f;

		public bool IsTriggerHeld => false;

		public bool Cocked
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public DoubleAction(Firearm selfRef, float triggerPullTime, float cooldownAfterShot, float cockingTime, string cockingTriggerName, byte dryfireClipIndex, byte mechaClipIndex, byte cockingClipIndex)
		{
		}

		private void ResetCocked()
		{
		}

		private void OnEquipped()
		{
		}

		private void ServerMsgReceived(NetworkConnection conn, CockMessage cock)
		{
		}

		private void CockHammer()
		{
		}

		private ActionModuleResponse UpdateHeldTrigger()
		{
			return default(ActionModuleResponse);
		}

		private AudioSource ClientPlaySound(int index)
		{
			return null;
		}

		public ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
		{
			return default(ActionModuleResponse);
		}

		public bool ServerAuthorizeShot()
		{
			return false;
		}

		public bool ServerAuthorizeDryFire()
		{
			return false;
		}
	}
}
