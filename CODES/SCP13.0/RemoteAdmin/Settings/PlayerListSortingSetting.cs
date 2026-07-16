using System;
using RemoteAdmin.Communication;
using RemoteAdmin.Generic;

namespace RemoteAdmin.Settings
{
	[Serializable]
	public class PlayerListSortingSetting : EnumSetting<RaPlayerList.PlayerSorting>
	{
		public override string Path { get; }

		public override RaPlayerList.PlayerSorting Value
		{
			get
			{
				return default(RaPlayerList.PlayerSorting);
			}
			set
			{
			}
		}

		protected override void OnLoad()
		{
		}
	}
}
