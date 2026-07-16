using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin
{
	public class CustomSlider : Slider
	{
		[Tooltip("This text will be updated as the value of this slider changes.")]
		public TMP_Text ValueDisplay;

		[SerializeField]
		private string _valueFormat;

		protected override void Awake()
		{
		}

		protected void ChangeValue(float newValue)
		{
		}

		protected virtual void OnValueChanged(float newValue)
		{
		}

		public virtual void SetText(float newValue)
		{
		}
	}
}
