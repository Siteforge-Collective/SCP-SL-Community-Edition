namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079KeyAbilityGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _background;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _unavailableColor;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _readyColor;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _description;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _keyText;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _lmbObj;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _rmbObj;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _rescaleTransform;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _rescaleParams;

		private global::UnityEngine.KeyCode _prevKeycode;

		internal void Setup(bool isReady, string description, ActionName key, bool createSpace)
		{
			float y = (createSpace ? _rescaleParams.x : _rescaleParams.y);
			_rescaleTransform.sizeDelta = new global::UnityEngine.Vector2(_rescaleParams.z, y);
			base.gameObject.SetActive(value: true);
			SetupKeycode(NewInput.GetKey(key));
			global::UnityEngine.Color color = (isReady ? _readyColor : _unavailableColor);
			_description.text = description;
			_description.color = color;
			_background.color = color;
		}

		private void SetupKeycode(global::UnityEngine.KeyCode keycode)
		{
			if (keycode != _prevKeycode)
			{
				bool flag = keycode == global::UnityEngine.KeyCode.Mouse0;
				bool flag2 = keycode == global::UnityEngine.KeyCode.Mouse1;
				_prevKeycode = keycode;
				_lmbObj.SetActive(flag);
				_rmbObj.SetActive(flag2);
				bool flag3 = flag || flag2;
				_keyText.gameObject.SetActive(!flag3);
				if (!flag3)
				{
					_keyText.text = new ReadableKeyCode(keycode).ShortVersion;
				}
			}
		}
	}
}
