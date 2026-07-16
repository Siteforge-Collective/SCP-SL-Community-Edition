namespace RemoteAdmin.Communication
{
	public class RaPlayer : global::RemoteAdmin.Interfaces.IServerCommunication, global::RemoteAdmin.Interfaces.IClientCommunication
	{
		public int DataId => 1;

		public void ReceiveData(CommandSender sender, string data)
		{
			string[] array = data.Split(' ');
			if (array.Length != 2 || !int.TryParse(array[0], out var result))
			{
				return;
			}
			bool flag = result == 1;
			global::RemoteAdmin.PlayerCommandSender playerCommandSender = sender as global::RemoteAdmin.PlayerCommandSender;
			if (!flag && playerCommandSender != null && !playerCommandSender.ServerRoles.Staff && !global::RemoteAdmin.CommandProcessor.CheckPermissions(sender, PlayerPermissions.PlayerSensitiveDataAccess))
			{
				return;
			}
			string[] newargs;
			global::System.Collections.Generic.List<ReferenceHub> list = global::Utils.RAUtils.ProcessPlayerIdOrNamesList(new global::System.ArraySegment<string>(global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1))), 0, out newargs);
			if (list.Count == 0)
			{
				return;
			}
			bool flag2 = PermissionsHandler.IsPermitted(sender.Permissions, 18007046uL);
			if (playerCommandSender != null && (playerCommandSender.ServerRoles.Staff || playerCommandSender.ServerRoles.RaEverywhere))
			{
				flag2 = true;
			}
			if (list.Count > 1)
			{
				global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent("<color=white>");
				stringBuilder.Append("Selecting multiple players:");
				stringBuilder.Append("\nPlayer ID: <color=green><link=CP_ID>\uf0c5</link></color>");
				stringBuilder.Append("\nIP Address: " + ((!flag) ? "<color=green><link=CP_IP>\uf0c5</link></color>" : "[REDACTED]"));
				stringBuilder.Append("\nUser ID: " + (flag2 ? "<color=green><link=CP_USERID>\uf0c5</link></color>" : "[REDACTED]"));
				stringBuilder.Append("</color>");
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				foreach (ReferenceHub item in list)
				{
					text = text + item.PlayerId + ".";
					if (!flag)
					{
						text2 = text2 + ((item.networkIdentity.connectionToClient.IpOverride != null) ? item.networkIdentity.connectionToClient.OriginalIpAddress : item.networkIdentity.connectionToClient.address) + ",";
					}
					if (flag2)
					{
						text3 = text3 + item.characterClassManager.UserId + ".";
					}
				}
				if (text.Length > 0)
				{
					global::RemoteAdmin.Communication.RaClipboard.Send(sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType.PlayerId, text);
				}
				if (text2.Length > 0)
				{
					global::RemoteAdmin.Communication.RaClipboard.Send(sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType.Ip, text2);
				}
				if (text3.Length > 0)
				{
					global::RemoteAdmin.Communication.RaClipboard.Send(sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType.UserId, text3);
				}
				sender.RaReply($"${DataId} {stringBuilder}", success: true, logToConsole: true, string.Empty);
				global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
				return;
			}
			ReferenceHub referenceHub = list[0];
			ServerLogs.AddLog(ServerLogs.Modules.DataAccess, $"{sender.LogName} accessed IP address of player {referenceHub.PlayerId} ({referenceHub.nicknameSync.MyNick}).", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			bool flag3 = PermissionsHandler.IsPermitted(sender.Permissions, PlayerPermissions.GameplayData);
			CharacterClassManager characterClassManager = referenceHub.characterClassManager;
			NicknameSync nicknameSync = referenceHub.nicknameSync;
			global::Mirror.NetworkConnectionToClient connectionToClient = referenceHub.networkIdentity.connectionToClient;
			ServerRoles serverRoles = referenceHub.serverRoles;
			if (sender is global::RemoteAdmin.PlayerCommandSender playerCommandSender2)
			{
				playerCommandSender2.ReferenceHub.queryProcessor.GameplayData = flag3;
			}
			global::System.Text.StringBuilder stringBuilder2 = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent("<color=white>");
			stringBuilder2.Append("Nickname: " + nicknameSync.CombinedName);
			stringBuilder2.Append($"\nPlayer ID: {referenceHub.PlayerId} <color=green><link=CP_ID>\uf0c5</link></color>");
			global::RemoteAdmin.Communication.RaClipboard.Send(sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType.PlayerId, $"{referenceHub.PlayerId}");
			if (connectionToClient == null)
			{
				stringBuilder2.Append("\nIP Address: null");
			}
			else if (!flag)
			{
				stringBuilder2.Append("\nIP Address: " + connectionToClient.address + " ");
				if (connectionToClient.IpOverride != null)
				{
					global::RemoteAdmin.Communication.RaClipboard.Send(sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType.Ip, connectionToClient.OriginalIpAddress ?? "");
					stringBuilder2.Append(" [routed via " + connectionToClient.OriginalIpAddress + "]");
				}
				else
				{
					global::RemoteAdmin.Communication.RaClipboard.Send(sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType.Ip, connectionToClient.address ?? "");
				}
				stringBuilder2.Append(" <color=green><link=CP_IP>\uf0c5</link></color>");
			}
			else
			{
				stringBuilder2.Append("\nIP Address: [REDACTED]");
			}
			stringBuilder2.Append("\nUser ID: " + ((!flag2) ? "<color=#D4AF37>INSUFFICIENT PERMISSIONS</color>" : (string.IsNullOrEmpty(characterClassManager.UserId) ? "(none)" : (characterClassManager.UserId + " <color=green><link=CP_USERID>\uf0c5</link></color>"))));
			if (flag2)
			{
				global::RemoteAdmin.Communication.RaClipboard.Send(sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType.UserId, characterClassManager.UserId ?? "");
				if (characterClassManager.SaltedUserId != null && characterClassManager.SaltedUserId.Contains("$"))
				{
					stringBuilder2.Append("\nSalted User ID: " + characterClassManager.SaltedUserId);
				}
				if (!string.IsNullOrEmpty(characterClassManager.UserId2))
				{
					stringBuilder2.Append("\nUser ID 2: " + characterClassManager.UserId2);
				}
			}
			stringBuilder2.Append("\nServer role: " + serverRoles.GetColoredRoleString());
			bool flag4 = global::RemoteAdmin.CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenBadges);
			bool flag5 = global::RemoteAdmin.CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenGlobalBadges);
			if (playerCommandSender != null && playerCommandSender.ServerRoles.Staff)
			{
				flag4 = true;
				flag5 = true;
			}
			bool flag6 = !string.IsNullOrEmpty(serverRoles.HiddenBadge);
			bool flag7 = !flag6 || (serverRoles.GlobalHidden && flag5) || (!serverRoles.GlobalHidden && flag4);
			if (flag7)
			{
				if (flag6)
				{
					stringBuilder2.Append("\n<color=#DC143C>Hidden role: </color>" + serverRoles.HiddenBadge);
					stringBuilder2.Append("\n<color=#DC143C>Hidden role type: </color>" + (serverRoles.GlobalHidden ? "GLOBAL" : "LOCAL"));
				}
				if (serverRoles.RaEverywhere)
				{
					stringBuilder2.Append("\nStudio Status: <color=#BCC6CC>Studio GLOBAL Staff (management or global moderation)</color>");
				}
				else if (serverRoles.Staff)
				{
					stringBuilder2.Append("\nStudio Status: <color=#94B9CF>Studio Staff</color>");
				}
			}
			int flags = (int)global::VoiceChat.VoiceChatMutes.GetFlags(list[0]);
			if (flags != 0)
			{
				stringBuilder2.Append("\nMUTE STATUS:");
				foreach (int value2 in global::System.Enum.GetValues(typeof(global::VoiceChat.VcMuteFlags)))
				{
					if (value2 != 0 && (flags & value2) == value2)
					{
						stringBuilder2.Append(" <color=#F70D1A>");
						stringBuilder2.Append((global::VoiceChat.VcMuteFlags)value2);
						stringBuilder2.Append("</color>");
					}
				}
			}
			stringBuilder2.Append("\nActive flag(s):");
			if (characterClassManager.GodMode)
			{
				stringBuilder2.Append(" <color=#659EC7>[GOD MODE]</color>");
			}
			if (referenceHub.playerStats.GetModule<global::PlayerStatsSystem.AdminFlagsStat>().HasFlag(global::PlayerStatsSystem.AdminFlags.Noclip))
			{
				stringBuilder2.Append(" <color=#DC143C>[NOCLIP ENABLED]</color>");
			}
			else if (global::PlayerRoles.FirstPersonControl.FpcNoclip.IsPermitted(referenceHub))
			{
				stringBuilder2.Append(" <color=#E52B50>[NOCLIP UNLOCKED]</color>");
			}
			if (serverRoles.DoNotTrack)
			{
				stringBuilder2.Append(" <color=#BFFF00>[DO NOT TRACK]</color>");
			}
			if (serverRoles.BypassMode)
			{
				stringBuilder2.Append(" <color=#BFFF00>[BYPASS MODE]</color>");
			}
			if (flag7 && serverRoles.RemoteAdmin)
			{
				stringBuilder2.Append(" <color=#43C6DB>[RA AUTHENTICATED]</color>");
			}
			if (serverRoles.IsInOverwatch)
			{
				stringBuilder2.Append(" <color=#008080>[OVERWATCH MODE]</color>");
			}
			else if (flag3)
			{
				stringBuilder2.Append("\nClass: ").Append(global::PlayerRoles.PlayerRoleLoader.AllRoles.TryGetValue(global::PlayerRoles.PlayerRolesUtils.GetRoleId(referenceHub), out var value) ? value.RoleName : "None");
				stringBuilder2.Append(" <color=#fcff99>[HP: ").Append(global::RemoteAdmin.CommandProcessor.GetRoundedStat<global::PlayerStatsSystem.HealthStat>(referenceHub)).Append("]</color>");
				stringBuilder2.Append(" <color=green>[AHP: ").Append(global::RemoteAdmin.CommandProcessor.GetRoundedStat<global::PlayerStatsSystem.AhpStat>(referenceHub)).Append("]</color>");
				stringBuilder2.Append(" <color=#977dff>[HS: ").Append(global::RemoteAdmin.CommandProcessor.GetRoundedStat<global::PlayerStatsSystem.HumeShieldStat>(referenceHub)).Append("]</color>");
				stringBuilder2.Append("\nPosition: ").Append(referenceHub.transform.position.ToPreciseString());
			}
			else
			{
				stringBuilder2.Append("\n<color=#D4AF37>Some fields were hidden. GameplayData permission required.</color>");
			}
			stringBuilder2.Append("</color>");
			sender.RaReply($"${DataId} {(global::NorthwoodLib.Pools.StringBuilderPool.Shared.ToStringReturn(stringBuilder2))}", success: true, logToConsole: true, string.Empty);
			global::RemoteAdmin.Communication.RaPlayerQR.Send(sender, isBig: false, string.IsNullOrEmpty(characterClassManager.UserId) ? "(no User ID)" : characterClassManager.UserId);
		}

		public void ReceiveData(string data, bool secure)
		{
		}
	}
}
