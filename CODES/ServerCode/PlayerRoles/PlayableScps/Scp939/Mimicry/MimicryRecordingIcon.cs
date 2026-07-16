namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryRecordingIcon : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _nickname;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _rolename;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _removeProgress;

		[global::UnityEngine.SerializeField]
		private HoldableButton _removeButton;

		[global::UnityEngine.SerializeField]
		private HoldableButton _previewButton;

		[global::UnityEngine.SerializeField]
		private HoldableButton _useButton;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite _startPreviewSprite;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite _stopPreviewSprite;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecorder _assignedRecorder;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingsMenu _menu;

		private global::VoiceChat.Networking.PlaybackBuffer _buffer;

		private int _id;

		private bool _previewing;

		private bool Interactable
		{
			get
			{
				if (base.gameObject.activeInHierarchy && _menu.HighlightedSlot == _id)
				{
					return global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController.Singleton.CurrentGroup == _menu.Fader;
				}
				return false;
			}
		}

		public global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation CurDescription
		{
			get
			{
				if (!Interactable)
				{
					return global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge;
				}
				if (_previewing)
				{
					return global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.LocalPreviewHeader;
				}
				if (_previewButton.IsHovering)
				{
					return global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PreviewVoiceButton;
				}
				if (_removeButton.IsHovering)
				{
					return global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.RemoveVoiceButton;
				}
				if (_useButton.IsHovering)
				{
					return global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.TransmitVoiceButton;
				}
				return global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge;
			}
		}

		public void Setup(global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingsMenu menu, global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecorder recorder, int id)
		{
			global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecorder.MimicryRecording mimicryRecording = recorder.SavedVoices[id];
			_id = id;
			_menu = menu;
			_assignedRecorder = recorder;
			_buffer = mimicryRecording.Buffer;
			if (global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayerRoleBase>(mimicryRecording.Owner.Role, out var result))
			{
				_nickname.text = mimicryRecording.Owner.Nickname;
				_rolename.text = result.RoleName;
				_rolename.color = result.RoleColor;
			}
		}

		private void Update()
		{
			_removeProgress.fillAmount = _removeButton.HeldPercent;
			if (_previewing && (!Interactable || _assignedRecorder.PreviewPlayback.IsEmpty))
			{
				StopPreview();
			}
		}

		public void TogglePreview()
		{
			if (Interactable)
			{
				if (_previewing)
				{
					StopPreview();
				}
				else
				{
					StartPreview();
				}
			}
		}

		public void RemoveRecording()
		{
			_assignedRecorder.SavedVoices.RemoveAt(_id);
			_assignedRecorder.SavedVoicesModified = true;
			_assignedRecorder.PreviewPlayback.StopPreview();
		}

		public void SendRecording()
		{
			_assignedRecorder.Transmitter.SendVoice(_buffer);
		}

		private void StartPreview()
		{
			_previewing = true;
			_assignedRecorder.PreviewPlayback.StartPreview(_buffer);
			_previewButton.image.sprite = _stopPreviewSprite;
		}

		private void StopPreview()
		{
			_previewing = false;
			_assignedRecorder.PreviewPlayback.StopPreview();
			_previewButton.image.sprite = _startPreviewSprite;
		}
	}
}
