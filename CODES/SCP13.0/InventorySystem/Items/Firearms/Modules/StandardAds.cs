using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public class StandardAds : IAdsModule, IFirearmModuleBase
	{
		private static readonly int AdsInAnimation;

		private static readonly int AdsOutAnimation;

		private static readonly int AdsCurrentHash;

		private bool _prevState;

		private bool _serverAds;

		private bool _deAdsAnimaiton;

		private bool _state;

		private float _curAds;

		private float _curAnimation;

		private float _extraDeltaTime;

		private AudioSource _adsSoundSource;

		protected readonly Firearm Firearm;

		private readonly ushort _serial;

		private readonly float _defaultAdsTime;

		private readonly int _adsLayer;

		private readonly byte _adsInClip;

		private readonly byte _adsOutClip;

		public bool Standby => false;

		public bool ServerAds
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float ClientAdsAmount => 0f;

		public bool ClientAllowAds => false;

		private float AdsSpeed => 0f;

		protected virtual bool ForceDisabled => false;

		protected virtual bool AllowChange => false;

		public StandardAds(Firearm selfRef, ushort serial, float defaultAdsTime, int adsLayer, byte adsInClip, byte adsOutClip)
		{
		}

		private void ResetAds()
		{
		}

		public void ClientUpdateAds(bool newState)
		{
		}

		private void PlayAdsClip(byte clipId)
		{
		}
	}
}
