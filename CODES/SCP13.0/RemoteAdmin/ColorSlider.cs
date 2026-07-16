using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin
{
	public class ColorSlider : CustomSlider
	{
		public enum ModifiedColorValue
		{
			R = 0,
			G = 1,
			B = 2,
			A = 3
		}

		[Tooltip("The images this slider is gonna change the color of.")]
		public RawImage[] Images;

		[Tooltip("Which color value will be modified by this slider.")]
		public ModifiedColorValue ValueToModify;

		protected override void OnValueChanged(float newValue)
		{
		}
	}
}
