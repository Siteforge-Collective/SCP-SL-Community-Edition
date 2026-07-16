using System;
using Tooltips;
using UnityEngine;

namespace RemoteAdmin.Generic
{
	[Serializable]
	public abstract class TooltipSetting : CustomSliderSetting
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
		public TooltipManager TooltipManager { get; set; }

		protected abstract void ChangeTooltip(float value);
	}
}
