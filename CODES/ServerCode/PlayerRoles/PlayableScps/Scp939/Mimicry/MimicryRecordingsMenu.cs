namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryRecordingsMenu : global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuBase
	{
		public enum MimicryDesc
		{
			None = 0,
			PreviewStart = 1,
			PreviewTimeline = 2,
			Remove = 3,
			Use = 4,
			Return = 20
		}

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingIcon[] _icons;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecorder _recorder;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingsMenu.MimicryDesc _desc;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingIcon _iconTemplate;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _previewRoot;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _previewElapsed;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _previewDuration;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Slider _previewProgress;

		private bool HighlightValid
		{
			get
			{
				if (base.HighlightedSlot >= 0)
				{
					return base.HighlightedSlot < Slots;
				}
				return false;
			}
		}

		public override int Slots => _recorder.SavedVoices.Count;

		protected override void OnSlotsNumberChanged(int prev, int cur)
		{
			base.OnSlotsNumberChanged(prev, cur);
			int slots = Slots;
			for (int i = 0; i < _recorder.MaxRecordings; i++)
			{
				global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingIcon mimicryRecordingIcon = _icons[i];
				if (i < slots)
				{
					mimicryRecordingIcon.gameObject.SetActive(value: true);
					mimicryRecordingIcon.Setup(this, _recorder, i);
					mimicryRecordingIcon.transform.localPosition = GetSlotPosition(i);
				}
				else
				{
					mimicryRecordingIcon.gameObject.SetActive(value: false);
				}
			}
			_recorder.SavedVoicesModified = false;
		}

		protected override void Setup(global::PlayerRoles.PlayableScps.Scp939.Scp939Role role)
		{
			base.Setup(role);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecorder>(out _recorder);
			int maxRecordings = _recorder.MaxRecordings;
			_icons = new global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingIcon[maxRecordings];
			for (int i = 0; i < maxRecordings; i++)
			{
				_icons[i] = global::UnityEngine.Object.Instantiate(_iconTemplate, _iconTemplate.transform.parent);
			}
		}

		protected override void Update()
		{
			base.Update();
			int slots = Slots;
			if (_recorder.SavedVoicesModified)
			{
				OnSlotsNumberChanged(slots, slots);
			}
			if (global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryExternalButton.TryGetDescription(out var desc))
			{
				UpdateDescription(desc);
			}
			else if (HighlightValid)
			{
				UpdateDescription(_icons[base.HighlightedSlot].CurDescription);
			}
			else
			{
				UpdateDescription(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge);
			}
			if (_desc == global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingsMenu.MimicryDesc.PreviewTimeline && HighlightValid)
			{
				_recorder.PreviewPlayback.UpdateProgress(_previewProgress, _previewElapsed, _previewDuration);
			}
		}

		protected override void OnDescriptionUpdated(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation newDesc)
		{
			_desc = (global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingsMenu.MimicryDesc)newDesc;
			switch (_desc)
			{
			case global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingsMenu.MimicryDesc.None:
				_previewRoot.SetActive(value: false);
				base.DescriptionText = string.Empty;
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryRecordingsMenu.MimicryDesc.PreviewTimeline:
				_previewRoot.SetActive(value: true);
				base.DescriptionText = string.Empty;
				break;
			default:
				_previewRoot.SetActive(value: false);
				base.OnDescriptionUpdated(newDesc);
				break;
			}
		}
	}
}
