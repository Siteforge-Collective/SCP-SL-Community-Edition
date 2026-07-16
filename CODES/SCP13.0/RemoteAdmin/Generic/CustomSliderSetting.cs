using System;
using UnityEngine;

namespace RemoteAdmin.Generic
{
	[Serializable]
	public abstract class CustomSliderSetting : RaSetting<float>
	{
		public override float Value
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		[field: SerializeField]
		public CustomSlider RepresentingSlider { get; set; }

		public void SliderToValue()
		{
		}

		protected override void OnSave()
		{
		}

		protected override void OnLoad()
		{
		}

		protected virtual void OnUpdateSlider(float value)
		{
		}
	}
}
