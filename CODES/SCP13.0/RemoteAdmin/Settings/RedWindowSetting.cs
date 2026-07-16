using System;
using RemoteAdmin.Generic;
using UnityEngine;

namespace RemoteAdmin.Settings
{
	[Serializable]
	public class RedWindowSetting : ColorSliderSetting
	{
		public override string Path => null;

		protected override Color CreateColor(Color oldColor, float value)
		{
			return default(Color);
		}
	}
}
