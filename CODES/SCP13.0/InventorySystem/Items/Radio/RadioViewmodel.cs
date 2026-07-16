using InventorySystem.Items.SwayControllers;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace InventorySystem.Items.Radio
{
	public class RadioViewmodel : AnimatedViewmodelBase
	{
		[Header("World-space User Interface")]
		[SerializeField]
		private GameObject _panelNoBattery;

		[SerializeField]
		[Header("World-space User Interface")]
		private GameObject _panelMain;

		[Header("World-space User Interface")]
		[SerializeField]
		private GameObject _panelRoot;

		[SerializeField]
		private TMP_Text _textModeShort;

		[SerializeField]
		private TMP_Text _textModeFull;

		[SerializeField]
		private TMP_Text _textBatteryLevel;

		[SerializeField]
		private TMP_Text _textVolume;

		[SerializeField]
		private TMP_Text _textTime;

		[SerializeField]
		private GameObject _txOn;

		[SerializeField]
		private GameObject _txOff;

		[SerializeField]
		private GameObject _rxOn;

		[SerializeField]
		private GameObject _rxOff;

		[SerializeField]
		private RawImage _rangeIndicator;

		[SerializeField]
		private RawImage _noBatteryIndicator;

		[SerializeField]
		private Image[] _batteryLevels;

		[SerializeField]
		[Header("Audio")]
		private AudioMixer _voicechatMixer;

		[SerializeField]
		private AudioSource _audioSource;

		[SerializeField]
		private AudioClip _clipTurnOn;

		[SerializeField]
		private AudioClip _clipTurnOff;

		[SerializeField]
		private AudioClip _clipCircleRange;

		[Header("Tracker")]
		[SerializeField]
		private Transform _cameraTrackerSource;

		[SerializeField]
		private Vector3 _cameraTrackerOffset;

		[SerializeField]
		private float _cameraTrackerIntensity;

		[Header("Other")]
		[SerializeField]
		private Transform _swayPivot;

		[SerializeField]
		private MeshRenderer _radioRenderer;

		[SerializeField]
		private Material _enabledMat;

		[SerializeField]
		private Material _disabledMat;

		private static readonly int IsTransmittingHash;

		private const string RadioChannelName = "AudioSettings_VoiceChat";

		private const string KeypadEmissionChannelName = "_EmissionColor";

		private const float BatteryFlashRate = 2.5f;

		private GoopSway _goopSway;

		private float _batteryFlashTimer;

		private int _prevRange;

		public override IItemSwayController SwayController => null;

		public override float ViewmodelCameraFOV => 0f;

		public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
		{
		}

		internal override void OnEquipped()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void UpdateNetwork()
		{
		}

		private void GetTxRx(out bool tx, out bool rx)
		{
			tx = default(bool);
			rx = default(bool);
		}

		private void SetBattery(byte percent)
		{
		}

		private void SetRange(int rangeId)
		{
		}

		private void SetState(bool state)
		{
		}

		private void RefreshKeypadColor(bool state)
		{
		}
	}
}
