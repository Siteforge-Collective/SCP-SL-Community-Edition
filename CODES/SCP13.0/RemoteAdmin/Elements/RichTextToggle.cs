using RemoteAdmin.Presets;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Elements
{
	public class RichTextToggle : CustomButton
	{
		[SerializeField]
		private Button _button;

		[SerializeField]
		private ToggleColorPreset _colorPreset;

		[Tooltip("If a value is specified, it will lookup the value inside the player's config")]
		[SerializeField]
		private string _configKey;

		public override void SetState(bool isSelected)
		{
		}

		public void Toggle()
		{
		}

		private void Start()
		{
		}
	}
}
