using System;
using RemoteAdmin.Generic;

namespace RemoteAdmin.Settings
{
	[Serializable]
	public class ToggleSpawnpointSetting : ToggleableSetting
	{
		public override string Path { get; }

		public override bool DefaultValue => false;
	}
}
