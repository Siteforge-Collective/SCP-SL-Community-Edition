using System.Runtime.InteropServices;
using InventorySystem.Items.Pickups;
using Mirror;
using Scp914;
using UnityEngine;
using VoiceChat.Playbacks;

namespace InventorySystem.Items.Radio
{
	public class RadioPickup : CollisionDetectionPickup, IUpgradeTrigger
	{
		[SyncVar]
		public bool SavedEnabled;

		[SyncVar]
		public byte SavedRange;

		public float SavedBattery;

		private static RadioItem _radioCache;

		private static bool _radioCacheSet;

		[SerializeField]
		private Material _enabledMat;

		[SerializeField]
		private Material _disabledMat;

		[SerializeField]
		private Renderer _targetRenderer;

		[SerializeField]
		private GameObject _activeObject;

		[SerializeField]
		private SpatializedRadioPlaybackBase _playback;

		private bool _prevEnabled;

		public bool NetworkSavedEnabled
		{
			get
			{
				return false;
			}
			[param: In]
			set
			{
			}
		}

		public byte NetworkSavedRange
		{
			get
			{
				return 0;
			}
			[param: In]
			set
			{
			}
		}

		private void Update()
		{
		}

		protected override void Awake()
		{
		}

		private void LateUpdate()
		{
		}

		public void ServerOnUpgraded(Scp914KnobSetting setting)
		{
		}
	}
}
