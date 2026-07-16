namespace InventorySystem.Items.Firearms.Modules
{
	public class StandardAds : global::InventorySystem.Items.Firearms.Modules.IAdsModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private bool _prevState;

		private bool _serverAds;

		private bool _deAdsAnimaiton;

		private bool _state;

		private float _curAds;

		private float _curAnimation;

		private float _extraDeltaTime;

		private global::UnityEngine.AudioSource _adsSoundSource;

		protected readonly global::InventorySystem.Items.Firearms.Firearm Firearm;

		private readonly ushort _serial;

		private readonly float _defaultAdsTime;

		private readonly int _adsLayer;

		private readonly byte _adsInClip;

		private readonly byte _adsOutClip;

		public bool Standby => true;

		public bool ServerAds
		{
			get
			{
				if (_serverAds && Firearm.AmmoManagerModule.Standby && Firearm.EquipperModule.Standby)
				{
					return AdsSpeed > 0f;
				}
				return false;
			}
			set
			{
				_serverAds = value;
			}
		}

		private float AdsSpeed => global::UnityEngine.Mathf.Max(0f, 1f / _defaultAdsTime * global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(Firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.AdsSpeedMultiplier));

		public StandardAds(global::InventorySystem.Items.Firearms.Firearm selfRef, ushort serial, float defaultAdsTime, int adsLayer, byte adsInClip, byte adsOutClip)
		{
			Firearm = selfRef;
			_serial = serial;
			_defaultAdsTime = defaultAdsTime;
			_adsLayer = adsLayer;
			_adsInClip = adsInClip;
			_adsOutClip = adsOutClip;
			selfRef.OnHolsteredCalled += ResetAds;
			if (selfRef.IsSpectated)
			{
				selfRef.OnEquipUpdateCalled += UpdateSpectator;
				_extraDeltaTime = selfRef.OwnerInventory.LastItemSwitch;
				UpdateSpectator();
			}
		}

		private void ResetAds()
		{
			_curAds = 0f;
			ServerAds = false;
		}

		private void UpdateSpectator()
		{
		}

		public void ClientUpdateAds(bool newState)
		{
		}
	}
}
