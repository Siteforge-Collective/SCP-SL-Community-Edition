using System;
using RemoteAdmin.Generic;

namespace RemoteAdmin.Settings
{
	[Serializable]
	public class BandwidthCooldownSetting : CustomSliderSetting
	{
		public override string Path => null;

		public override float DefaultValue => 0f;
	}
}
