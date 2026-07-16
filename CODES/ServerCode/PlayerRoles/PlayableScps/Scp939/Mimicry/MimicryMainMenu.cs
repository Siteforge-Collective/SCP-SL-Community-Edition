namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryMainMenu : global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuBase
	{
		[global::System.Serializable]
		private class MainMenuOption
		{
			public global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation Title;

			public global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation Description;

			public global::UnityEngine.Events.UnityEvent OnSelect;
		}

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMainMenu.MainMenuOption _stolenVoicesOption;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMainMenu.MainMenuOption _envSoundsOption;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMainMenu.MainMenuOption _placeMimicPointOption;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMainMenu.MainMenuOption _removeMimicPointOption;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TMP_Text _titleTemplate;

		private int _mimicPointIndex;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMainMenu.MainMenuOption[] _options;

		private global::TMPro.TMP_Text[] _textInstances;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController _mimicPointController;

		public override int Slots => _options.Length;

		protected override void Setup(global::PlayerRoles.PlayableScps.Scp939.Scp939Role role)
		{
			base.Setup(role);
			_options = new global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMainMenu.MainMenuOption[3] { _stolenVoicesOption, _placeMimicPointOption, _envSoundsOption };
			_mimicPointIndex = _options.IndexOf(_placeMimicPointOption);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController>(out _mimicPointController);
			_mimicPointController.OnMessageReceived += HandleRpc;
			RegisterMimicPointButton(_placeMimicPointOption);
			RegisterMimicPointButton(_removeMimicPointOption);
		}

		protected override void OnSlotsNumberChanged(int prev, int cur)
		{
			base.OnSlotsNumberChanged(prev, cur);
			_textInstances = new global::TMPro.TMP_Text[cur];
			for (int i = 0; i < cur; i++)
			{
				global::UnityEngine.RectTransform rectTransform = Highlights[i].rectTransform;
				global::TMPro.TMP_Text tMP_Text = global::UnityEngine.Object.Instantiate(_titleTemplate, rectTransform.parent);
				tMP_Text.rectTransform.localPosition = GetSlotPosition(i);
				_textInstances[i] = tMP_Text;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (base.HighlightedSlot >= 0)
			{
				UpdateDescription(_options[base.HighlightedSlot].Description);
			}
			else
			{
				UpdateDescription(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge);
			}
			_options[_mimicPointIndex] = (_mimicPointController.Active ? _removeMimicPointOption : _placeMimicPointOption);
			for (int i = 0; i < Slots; i++)
			{
				_textInstances[i].text = Translations.Get(_options[i].Title);
			}
		}

		protected override void OnSelected()
		{
			base.OnSelected();
			if (base.HighlightedSlot >= 0 && base.HighlightedSlot < Slots)
			{
				_options[base.HighlightedSlot].OnSelect?.Invoke();
			}
		}

		private void RegisterMimicPointButton(global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMainMenu.MainMenuOption option)
		{
			option.OnSelect.AddListener(_mimicPointController.ClientToggle);
		}

		private void HandleRpc(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation msg)
		{
			if (msg != global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge)
			{
				global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController.Singleton.CurrentGroup = null;
			}
		}

		private void OnDestroy()
		{
			_mimicPointController.OnMessageReceived -= HandleRpc;
		}
	}
}
