using System;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Generic
{
	[Serializable]
	public abstract class ColorSliderSetting : CustomSliderSetting
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

		public override float DefaultValue => 0f;

		[field: SerializeField]
		public RawImage[] RaUIElements { get; set; }

		protected override void OnSave()
		{
		}

		protected override void OnUpdateSlider(float value)
		{
		}

		protected virtual Color CreateColor(Color oldColor, float value)
		{
			return default(Color);
		}
	}
}
