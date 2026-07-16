using System;

namespace RemoteAdmin.Generic
{
	[Serializable]
	public abstract class EnumSetting<T> : RaSetting<T> where T : struct, Enum
	{
		protected override void OnSave()
		{
		}

		protected override void OnLoad()
		{
		}
	}
}
