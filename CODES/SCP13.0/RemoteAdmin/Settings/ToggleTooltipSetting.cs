using System;
using RemoteAdmin.Generic;
using Tooltips;
using UnityEngine;

namespace RemoteAdmin.Settings
{
	[Serializable]
	public class ToggleTooltipSetting : ToggleableSetting
	{
		public override bool DefaultValue { get; }

		public override string Path { get; }

		public override bool Value
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		[field: SerializeField]
		public TooltipManager TooltipManager { get; set; }

		private void ChangeTooltip(bool value)
		{
		}
	}
}
