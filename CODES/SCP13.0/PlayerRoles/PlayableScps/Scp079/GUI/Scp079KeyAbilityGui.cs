using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079KeyAbilityGui : Scp079GuiElementBase
	{
		[SerializeField]
		private Image _background;

		[SerializeField]
		private Color _unavailableColor;

		[SerializeField]
		private Color _readyColor;

		[SerializeField]
		private TextMeshProUGUI _description;

		[SerializeField]
		private TextMeshProUGUI _keyText;

		[SerializeField]
		private GameObject _lmbObj;

		[SerializeField]
		private GameObject _rmbObj;

		[SerializeField]
		private RectTransform _rescaleTransform;

		[SerializeField]
		private Vector3 _rescaleParams;

		private KeyCode _prevKeycode;

		internal void Setup(bool isReady, string description, ActionName key, bool createSpace)
		{
		}

		private void SetupKeycode(KeyCode keycode)
		{
		}
	}
}
