using System;
using RemoteAdmin.Generic;

namespace RemoteAdmin.Settings
{
	[Serializable]
	public class TooltipHideDelaySetting : TooltipSetting
	{
		public override string Path => null;

		public override float DefaultValue => 0f;

		protected override void ChangeTooltip(float value)
		{
		}
	}
}
