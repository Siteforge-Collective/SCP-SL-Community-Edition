internal class CentralAuthInterface : ICentralAuth
{
	private readonly ReferenceHub _hub;

	private readonly bool _is;

	public ReferenceHub GetHub()
	{
		return _hub;
	}

	public CentralAuthInterface(ReferenceHub hub, bool server)
	{
		_hub = hub;
		_is = server;
	}

	public void TokenGenerated(string token)
	{
		global::GameCore.Console.AddLog("Authentication token obtained from central server.", global::UnityEngine.Color.green);
		_hub.characterClassManager.CmdSendToken(token);
	}

	public void RequestBadge(string token)
	{
		_hub.serverRoles.RequestBadge(token);
	}

	public void RequestPublicPart(string token)
	{
		_hub.serverRoles.SetPublicPart(token);
	}

	public void Fail()
	{
		if (_is)
		{
			ServerConsole.AddLog("Failed to validate authentication token.");
			ServerConsole.Disconnect(_hub.connectionToClient, "Failed to validate authentication token.");
		}
		else
		{
			global::GameCore.Console.AddLog("Failed to obtain authentication token from central server.", global::UnityEngine.Color.red);
			_hub.connectionToServer.Disconnect();
		}
	}

	public void Ok(string userId, string userId2, string ban, string server, bool bypass, bool bypassWl, bool DNT, string serial, string vacSession, string rqIp, string Asn, bool BypassIpCheck)
	{
		switch (ban)
		{
		case "NO":
			ServerConsole.AddLog("Accepted authentication token of user " + userId + " signed by " + server + ". No active global bans.");
			break;
		case "M1":
			ServerConsole.AddLog("Accepted authentication token of user " + userId + " signed by " + server + ". Player is globally muted.");
			break;
		case "M2":
			ServerConsole.AddLog("Accepted authentication token of user " + userId + " signed by " + server + ". Player is globally muted on intercom.");
			break;
		default:
			ServerConsole.AddLog("Accepted authentication token of user " + userId + " signed by " + server + ". Active global ban present in the token.");
			break;
		}
		_hub.gameConsoleTransmission.SendToClient("Accepted your authentication token (your user id is " + userId + ") " + ((ban == "NO") ? "without any global bans present" : ("with global ban status " + ban)) + " signed by " + server + " server.", "green");
		CharacterClassManager characterClassManager = _hub.characterClassManager;
		ServerRoles serverRoles = _hub.serverRoles;
		characterClassManager.AuthTokenSerial = serial;
		characterClassManager.RequestIp = rqIp;
		characterClassManager.VacSession = vacSession;
		characterClassManager.Asn = Asn;
		if (DNT)
		{
			serverRoles.SetDoNotTrack();
		}
		string userId3 = (userId.Contains("$") ? userId.Substring(0, userId.IndexOf("$", global::System.StringComparison.Ordinal)) : userId);
		if (((!bypass || !ServerStatic.GetPermissionsHandler().IsVerified) && BanHandler.QueryBan(userId3, null).Key != null) || (userId2 != null && (!bypass || !ServerStatic.GetPermissionsHandler().IsVerified) && BanHandler.QueryBan(userId2, null).Key != null))
		{
			TargetConsolePrint("You are banned from this server.", "red");
			ServerConsole.AddLog("Player kicked due to local User ID ban.");
			ServerConsole.Disconnect(_hub.connectionToClient, "You are banned from this server.");
			return;
		}
		if ((global::GameCore.ConfigFile.ServerConfig.GetBool("use_global_bans", def: true) || ServerStatic.PermissionsHandler.IsVerified) && ban != "NO" && ban != "M1" && ban != "M2")
		{
			TargetConsolePrint(ban, "red");
			ServerConsole.AddLog("Player with ID " + userId + " kicked due to an active global ban: " + ban + ".");
			ServerConsole.Disconnect(_hub.connectionToClient, ban);
			return;
		}
		if (!BypassIpCheck && rqIp != "N/A" && ServerConsole.EnforceSameIp)
		{
			string address = _hub.connectionToClient.address;
			if ((address.Contains(".") && rqIp.Contains(".")) || (address.Contains(":") && rqIp.Contains(":")))
			{
				bool flag = false;
				if (ServerConsole.SkipEnforcementForLocalAddresses)
				{
					flag = address == "127.0.0.1" || address.StartsWith("10.") || address.StartsWith("192.168.");
					if (!flag && address.StartsWith("172."))
					{
						string[] array = address.Split('.');
						if (array.Length == 4 && byte.TryParse(array[1], out var result) && result >= 16 && result <= 31)
						{
							flag = true;
						}
					}
				}
				if (!flag && address != rqIp)
				{
					TargetConsolePrint("Authentication token has been issued to a different IP address.", "red");
					TargetConsolePrint("Your IP address: " + address, "red");
					TargetConsolePrint("Issued to: " + rqIp, "red");
					ServerConsole.AddLog("Player kicked due to IP addresses mismatch.");
					ServerConsole.Disconnect(_hub.connectionToClient, "Authentication token has been issued to a different IP address.");
					return;
				}
			}
		}
		global::VoiceChat.VcMuteFlags vcMuteFlags = global::VoiceChat.VcMuteFlags.None;
		if ((global::VoiceChat.VoiceChatMutes.QueryLocalMute(userId3) || (userId2 != null && global::VoiceChat.VoiceChatMutes.QueryLocalMute(userId2))) && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerMuted, _hub, false))
		{
			vcMuteFlags |= global::VoiceChat.VcMuteFlags.LocalRegular;
			TargetConsolePrint("You are muted on the voice chat by the server administrator.", "red");
		}
		if ((global::GameCore.ConfigFile.ServerConfig.GetBool("global_mutes_voicechat", def: true) || ServerStatic.PermissionsHandler.IsVerified) && ban == "M1" && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerMuted, _hub, false))
		{
			vcMuteFlags |= global::VoiceChat.VcMuteFlags.GlobalRegular;
			TargetConsolePrint("You are globally muted on the voice chat.", "red");
		}
		if (global::VoiceChat.VoiceChatMutes.QueryLocalMute(userId3, intercom: true) || (userId2 != null && global::VoiceChat.VoiceChatMutes.QueryLocalMute(userId2, intercom: true)))
		{
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerMuted, _hub, true))
			{
				vcMuteFlags |= global::VoiceChat.VcMuteFlags.LocalIntercom;
				TargetConsolePrint("You are muted on the intercom by the server administrator.", "red");
			}
		}
		else if ((global::GameCore.ConfigFile.ServerConfig.GetBool("global_mutes_intercom", def: true) || ServerStatic.PermissionsHandler.IsVerified) && ban == "M2" && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerMuted, _hub, true))
		{
			vcMuteFlags |= global::VoiceChat.VcMuteFlags.GlobalIntercom;
			TargetConsolePrint("You are globally muted on the intercom.", "red");
		}
		serverRoles.BypassStaff |= bypass;
		if (serverRoles.BypassStaff)
		{
			vcMuteFlags = global::VoiceChat.VcMuteFlags.None;
		}
		global::VoiceChat.VoiceChatMutes.SetFlags(_hub, vcMuteFlags);
		if (serverRoles.BypassStaff)
		{
			TargetConsolePrint("You have the ban bypass flag, so you can't be banned from this server.", "cyan");
		}
		serverRoles.StartServerChallenge(0);
	}

	public void FailToken(string reason)
	{
		TargetConsolePrint("Your authentication token is invalid - " + reason, "red");
		ServerConsole.AddLog("Rejected invalid authentication token.");
		ServerConsole.Disconnect(_hub.connectionToClient, reason);
	}

	private void TargetConsolePrint(string txt, string color)
	{
		_hub.gameConsoleTransmission.SendToClient(txt, color);
	}
}
