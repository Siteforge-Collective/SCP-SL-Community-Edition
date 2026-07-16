namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public abstract class MimicryMenuBase : global::RadialMenus.RadialMenuBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _descriptionFader;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TMP_Text _descriptionInfo;

		private float _angleStep;

		private float _halfStep;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation _displayedDesc;

		private const float DescriptionFadeSpeed = 15f;

		private const float IconOffset = 280f;

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.CanvasGroup Fader { get; private set; }

		protected string DescriptionText
		{
			get
			{
				return _descriptionInfo.text;
			}
			set
			{
				_descriptionInfo.text = value;
			}
		}

		protected virtual void Awake()
		{
			if (ReferenceHub.TryGetLocalHub(out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp939.Scp939Role role)
			{
				Setup(role);
			}
			else
			{
				base.gameObject.SetActive(value: false);
			}
		}

		protected virtual void Setup(global::PlayerRoles.PlayableScps.Scp939.Scp939Role role)
		{
		}

		protected override void Update()
		{
			base.Update();
			int slots = Slots;
			for (int i = 0; i < slots; i++)
			{
				global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController.UpdateHighlight(Highlights[i], i == base.HighlightedSlot);
			}
			if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Mouse0) && !(Fader.alpha < 1f))
			{
				OnSelected();
			}
		}

		protected void UpdateDescription(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation translation)
		{
			float num = 15f * global::UnityEngine.Time.deltaTime;
			if (_displayedDesc == translation)
			{
				_descriptionFader.alpha = global::UnityEngine.Mathf.MoveTowards(_descriptionFader.alpha, global::UnityEngine.Mathf.Min(1, (int)_displayedDesc), num);
				return;
			}
			if (_descriptionFader.alpha > 0f)
			{
				_descriptionFader.alpha -= num;
				return;
			}
			OnDescriptionUpdated(translation);
			_displayedDesc = translation;
		}

		protected virtual void OnDescriptionUpdated(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation newDesc)
		{
			DescriptionText = Translations.Get(newDesc);
		}

		protected global::UnityEngine.Vector3 GetSlotPosition(int slot)
		{
			return global::UnityEngine.Quaternion.Euler(0f, 0f, (0f - _angleStep) * (float)slot - _halfStep) * global::UnityEngine.Vector3.up * 280f;
		}

		protected virtual void OnSelected()
		{
			if (base.HighlightedSlot < 0 && !InRingRange(out var _) && !global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryExternalButton.TryGetHighlighted(out var _))
			{
				global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController.Singleton.IsEnabled = false;
			}
		}

		protected override void OnSlotsNumberChanged(int prev, int cur)
		{
			_angleStep = 360f / (float)global::UnityEngine.Mathf.Max(1, cur);
			_halfStep = ((cur == 1) ? 0f : (_angleStep / 2f));
			if (cur != 0)
			{
				base.OnSlotsNumberChanged(prev, cur);
			}
			global::UnityEngine.UI.Image[] highlights = Highlights;
			foreach (global::UnityEngine.UI.Image image in highlights)
			{
				if (image == null)
				{
					break;
				}
				image.color = global::UnityEngine.Color.clear;
			}
			base.OnSlotsNumberChanged(prev, cur);
		}

		public void Open()
		{
			global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController.Singleton.CurrentGroup = Fader;
			int num = Slots;
			while (--num >= 0)
			{
				global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController.UpdateHighlight(Highlights[num], isHighlighted: false, 1f);
			}
		}
	}
}
