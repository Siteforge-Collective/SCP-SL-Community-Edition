namespace PlayerRoles.RoleHelp
{
	public class KeyActionIcon : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private ActionName _action;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _keycodeBg;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TMP_Text _keycodeText;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TMP_Text _mouseText;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject[] _mouseIcons;

		private const global::UnityEngine.KeyCode MouseMin = global::UnityEngine.KeyCode.Mouse0;

		private const global::UnityEngine.KeyCode MouseMax = global::UnityEngine.KeyCode.Mouse6;

		private void Awake()
		{
			Refresh();
		}

		private void Refresh()
		{
			global::UnityEngine.KeyCode key = NewInput.GetKey(_action);
			_mouseIcons.ForEach(delegate(global::UnityEngine.GameObject x)
			{
				x.SetActive(value: false);
			});
			if (key >= global::UnityEngine.KeyCode.Mouse0 && key <= global::UnityEngine.KeyCode.Mouse6)
			{
				HandleMouse((int)(key - 323));
			}
			else
			{
				HandleKeycode(new ReadableKeyCode(key));
			}
		}

		private void HandleMouse(int buttonId)
		{
			_keycodeText.text = string.Empty;
			_mouseText.text = buttonId.ToString();
			int b = _mouseIcons.Length - 1;
			int num = global::UnityEngine.Mathf.Min(buttonId, b);
			_mouseIcons[num].SetActive(value: true);
			_keycodeBg.enabled = false;
		}

		private void HandleKeycode(ReadableKeyCode rkc)
		{
			_keycodeText.text = rkc.NormalVersion;
			_keycodeBg.enabled = true;
		}
	}
}
