namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079AuxGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _slider;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _textNormal;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _textInverted;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager _auxManager;

		private const string Format = "{0} / {1}";

		private string ExpText
		{
			set
			{
				_textNormal.text = value;
				_textInverted.text = value;
			}
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager>(out _auxManager);
		}

		private void Update()
		{
			_slider.fillAmount = _auxManager.CurrentAux / _auxManager.MaxAux;
			ExpText = $"{_auxManager.CurrentAuxFloored} / {_auxManager.MaxAux}";
		}
	}
}
