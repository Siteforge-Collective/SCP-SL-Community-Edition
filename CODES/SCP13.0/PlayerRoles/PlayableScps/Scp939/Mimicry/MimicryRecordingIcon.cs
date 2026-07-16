using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoiceChat.Networking;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryRecordingIcon : MonoBehaviour
	{
		private static readonly HashSet<MimicryRecordingIcon> FavoritedEntries;

		[SerializeField]
		private GameObject _contentRoot;

		[SerializeField]
		private TextMeshProUGUI _nickname;

		[SerializeField]
		private TextMeshProUGUI _rolename;

		[SerializeField]
		private Image _removeProgress;

		[SerializeField]
		private HoldableButton _removeButton;

		[SerializeField]
		private HoldableButton _previewButton;

		[SerializeField]
		private HoldableButton _useButton;

		[SerializeField]
		private HoldableButton _stopButton;

		[SerializeField]
		private MimicryWaveform _waveformVisualizer;

		[SerializeField]
		private Image _favoriteFill;

		[SerializeField]
		private TMP_Text _favoriteLabel;

		private const KeyCode FirstKeybind = KeyCode.Alpha1;

		private MimicryPreviewPlayback _previewPlayback;

		private MimicryTransmitter _transmitter;

		private MimicryRecorder _assignedRecorder;

		private RectTransform _cachedRt;

		private bool _previewing;

		private bool _isEmpty;

		private bool _isFavorite;

		private KeyCode _assignedHotkey;

		public PlaybackBuffer VoiceRecord { get; private set; }

		public bool IsFavorite
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public bool IsEmpty
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float Height
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public void Setup(MimicryRecorder recorder, int id)
		{
		}

		private void Update()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void UpdateFavoriteHotkey()
		{
		}

		public void StartPreview()
		{
		}

		public void RemoveRecording()
		{
		}

		public void SendRecording()
		{
		}

		public void StopPlayback()
		{
		}

		public void ToggleFavorite()
		{
		}

		private void StopPreview()
		{
		}
	}
}
