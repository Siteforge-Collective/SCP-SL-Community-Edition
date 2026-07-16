namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryExternalButton : global::UnityEngine.UI.Button
	{
		[global::UnityEngine.SerializeField]
		private bool _hideDescriptionOnClick;

		private static global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryExternalButton _highlighted;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuBase _assignedMenu;

		private bool _descriptionHidden;

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation Description { get; internal set; }

		private bool Current => global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController.Singleton.CurrentGroup == _assignedMenu.Fader;

		private void Update()
		{
			base.targetGraphic.raycastTarget = Current;
		}

		protected override void Awake()
		{
			base.Awake();
			_assignedMenu = GetComponentInParent<global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuBase>();
			if (_hideDescriptionOnClick)
			{
				base.onClick.AddListener(delegate
				{
					_descriptionHidden = true;
				});
			}
		}

		public override void OnPointerEnter(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			if (Current)
			{
				_highlighted = this;
				_descriptionHidden = false;
			}
		}

		public override void OnPointerExit(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			if (!(_highlighted != this))
			{
				_highlighted = null;
			}
		}

		public static bool TryGetHighlighted(out global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryExternalButton val)
		{
			val = _highlighted;
			if (val != null)
			{
				return val.Current;
			}
			return false;
		}

		public static bool TryGetDescription(out global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation desc)
		{
			desc = (TryGetHighlighted(out var val) ? val.Description : global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge);
			if (desc != global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge)
			{
				return !val._descriptionHidden;
			}
			return false;
		}
	}
}
