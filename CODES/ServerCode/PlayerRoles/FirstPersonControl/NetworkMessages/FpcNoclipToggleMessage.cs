namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
	public struct FpcNoclipToggleMessage : global::Mirror.NetworkMessage
	{
		public void ProcessMessage(global::Mirror.NetworkConnection sender)
		{
			if (ReferenceHub.TryGetHubNetID(sender.identity.netId, out var hub) && global::PlayerRoles.FirstPersonControl.FpcNoclip.IsPermitted(hub))
			{
				if (hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole)
				{
					hub.playerStats.GetModule<global::PlayerStatsSystem.AdminFlagsStat>().InvertFlag(global::PlayerStatsSystem.AdminFlags.Noclip);
				}
				else
				{
					hub.gameConsoleTransmission.SendToClient("Noclip is not supported for this class.", "yellow");
				}
			}
		}
	}
}
