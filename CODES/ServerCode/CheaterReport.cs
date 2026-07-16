public class CheaterReport : global::Mirror.NetworkBehaviour
{
	private static readonly global::System.Collections.Generic.Dictionary<char, string> CharacterReplacements;

	internal static bool SendReportsByWebhooks;

	internal static string WebhookUrl;

	internal static string WebhookUsername;

	internal static string WebhookAvatar;

	internal static string ServerName;

	internal static string ReportHeader;

	internal static string ReportContent;

	internal static int WebhookColor;

	private int _reportedPlayersAmount;

	private float _lastReport;

	private global::System.Collections.Generic.HashSet<uint> _reportedPlayers;

	private global::Security.RateLimit _commandRateLimit;

	private ReferenceHub _hub;

	private void Start()
	{
		_hub = ReferenceHub.GetHub(this);
		_commandRateLimit = _hub.playerRateLimitHandler.RateLimits[2];
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdReport(uint playerNetId, string reason, byte[] signature, bool notifyGm)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, playerNetId);
		global::Mirror.NetworkWriterExtensions.WriteString(writer, reason);
		global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, signature);
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, notifyGm);
		SendCommandInternal(typeof(CheaterReport), "CmdReport", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Server]
	private void IssueReport(GameConsoleTransmission reporter, string reporterUserId, string reportedUserId, string reportedAuth, string reportedIp, string reporterAuth, string reporterIp, ref string reason, ref byte[] signature, string reporterPublicKey, uint reportedNetId, string reporterNickname, string reportedNickname)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void CheaterReport::IssueReport(GameConsoleTransmission,System.String,System.String,System.String,System.String,System.String,System.String,System.String&,System.Byte[]&,System.String,System.UInt32,System.String,System.String)' called when server was not active");
			return;
		}
		try
		{
			string data = $"reporterAuth={(global::NorthwoodLib.StringUtils.Base64Encode(reporterAuth))}&reporterIp={reporterIp}&reportedAuth={(global::NorthwoodLib.StringUtils.Base64Encode(reportedAuth))}&reportedIp={reportedIp}&reason={(global::NorthwoodLib.StringUtils.Base64Encode(ConvertToLatin(reason)))}&signature={(global::System.Convert.ToBase64String(signature))}&reporterKey={(global::NorthwoodLib.StringUtils.Base64Encode(reporterPublicKey))}&token={ServerConsole.Password}&port={ServerConsole.PortToReport}&serverIp={ServerConsole.Ip}";
			string text = HttpQuery.Post(CentralServer.StandardUrl + "ingamereport.php", data);
			if (reporter == null)
			{
				return;
			}
			switch (text)
			{
			case "OK":
				_reportedPlayers.Add(reportedNetId);
				reporter.SendToClient(base.connectionToClient, "[REPORTING] Player report successfully sent.", "green");
				break;
			case "ReportedUserIDAlreadyReported":
				reporter.SendToClient(base.connectionToClient, "[REPORTING] A report for this User ID already exists!" + global::System.Environment.NewLine, "yellow");
				break;
			case "RateLimited":
				reporter.SendToClient(base.connectionToClient, "[REPORTING] You are Ratelimited! Try again tomorrow." + global::System.Environment.NewLine, "red");
				break;
			default:
				reporter.SendToClient(base.connectionToClient, "[REPORTING] Error during **PROCESSING** player report:" + global::System.Environment.NewLine + text, "red");
				break;
			}
		}
		catch (global::System.Exception ex)
		{
			global::GameCore.Console.AddLog("[HOST] Error during **SENDING** player report:" + global::System.Environment.NewLine + ex.Message, global::UnityEngine.Color.red);
			if (reporter == null)
			{
				return;
			}
			reporter.SendToClient(base.connectionToClient, "[REPORTING] Error during **SENDING** player report.", "yellow");
		}
		if (SendReportsByWebhooks)
		{
			LogReport(reporter, reporterUserId, reportedUserId, ref reason, reportedNetId, notifyGm: true, reporterNickname, reportedNickname);
		}
	}

	[global::Mirror.Server]
	private void LogReport(GameConsoleTransmission reporter, string reporterUserId, string reportedUserId, ref string reason, uint reportedNetId, bool notifyGm, string reporterNickname, string reportedNickname)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void CheaterReport::LogReport(GameConsoleTransmission,System.String,System.String,System.String&,System.UInt32,System.Boolean,System.String,System.String)' called when server was not active");
		}
		else if (SubmitReport(reporterUserId, reportedUserId, reason, reportedNetId, reporterNickname, reportedNickname, friendlyFire: false))
		{
			if (!notifyGm)
			{
				_reportedPlayers.Add(reportedNetId);
				reporter.SendToClient(base.connectionToClient, "[REPORTING] Player report successfully sent to local administrators by webhooks.", "green");
			}
		}
		else
		{
			reporter.SendToClient(base.connectionToClient, "[REPORTING] Failed to send report to local administrators by webhooks.", "red");
		}
	}

	internal static bool SubmitReport(string reporterUserId, string reportedUserId, string reason, uint reportedId, string reporterNickname, string reportedNickname, bool friendlyFire)
	{
		try
		{
			HttpQuery.Post(friendlyFire ? FriendlyFireConfig.WebhookUrl : WebhookUrl, "payload_json=" + global::Utf8Json.JsonSerializer.ToJsonString(new DiscordWebhook(string.Empty, WebhookUsername, WebhookAvatar, tts: false, new DiscordEmbed[1]
			{
				new DiscordEmbed(ReportHeader, "rich", ReportContent, WebhookColor, new DiscordEmbedField[10]
				{
					new DiscordEmbedField("Server Name", ServerName, inline: false),
					new DiscordEmbedField("Server Endpoint", $"{ServerConsole.Ip}:{ServerConsole.PortToReport}", inline: false),
					new DiscordEmbedField("Reporter UserID", AsDiscordCode(reporterUserId), inline: false),
					new DiscordEmbedField("Reporter Nickname", DiscordSanitize(reporterNickname), inline: false),
					new DiscordEmbedField("Reported UserID", AsDiscordCode(reportedUserId), inline: false),
					new DiscordEmbedField("Reported Nickname", DiscordSanitize(reportedNickname), inline: false),
					new DiscordEmbedField("Reported NetID", reportedId.ToString(), inline: false),
					new DiscordEmbedField("Reason", DiscordSanitize(reason), inline: false),
					new DiscordEmbedField("Timestamp", TimeBehaviour.Rfc3339Time(), inline: false),
					new DiscordEmbedField("UTC Timestamp", TimeBehaviour.Rfc3339Time(global::System.DateTimeOffset.UtcNow), inline: false)
				})
			})));
			return true;
		}
		catch (global::System.Exception ex)
		{
			ServerConsole.AddLog("Failed to send report by webhook: " + ex.Message);
			global::UnityEngine.Debug.LogException(ex);
			return false;
		}
	}

	private static string ConvertToLatin(string str)
	{
		foreach (global::System.Collections.Generic.KeyValuePair<char, string> characterReplacement in CharacterReplacements)
		{
			str = str.Replace(characterReplacement.Key.ToString(), characterReplacement.Value);
		}
		return str;
	}

	private static string AsDiscordCode(string text)
	{
		return "`" + text.Replace("`", "'") + "`";
	}

	private static string DiscordSanitize(string text)
	{
		return text.Replace("<", "(").Replace(">", ")").Replace("@", "@ ")
			.Replace("`", "'")
			.Replace("~~", "∼∼")
			.Replace("*", "★")
			.Replace("_", "\uff3f")
			.Replace("&", " [AMP] ")
			.Replace("?", " [QM] ");
	}

	static CheaterReport()
	{
		CharacterReplacements = new global::System.Collections.Generic.Dictionary<char, string>
		{
			{ 'а', "a" },
			{ 'б', "b" },
			{ 'в', "v" },
			{ 'г', "g" },
			{ 'д', "d" },
			{ 'е', "e" },
			{ 'ё', "yo" },
			{ 'ж', "zh" },
			{ 'з', "z" },
			{ 'и', "i" },
			{ 'й', "j" },
			{ 'к', "k" },
			{ 'л', "l" },
			{ 'м', "m" },
			{ 'н', "n" },
			{ 'о', "o" },
			{ 'п', "p" },
			{ 'р', "r" },
			{ 'с', "s" },
			{ 'т', "t" },
			{ 'у', "u" },
			{ 'ф', "f" },
			{ 'х', "h" },
			{ 'ц', "c" },
			{ 'ч', "ch" },
			{ 'ш', "sh" },
			{ 'щ', "sch" },
			{ 'ъ', "j" },
			{ 'ы', "i" },
			{ 'ь', "j" },
			{ 'э', "e" },
			{ 'ю', "yu" },
			{ 'я', "ya" },
			{ 'А', "A" },
			{ 'Б', "B" },
			{ 'В', "V" },
			{ 'Г', "G" },
			{ 'Д', "D" },
			{ 'Е', "E" },
			{ 'Ё', "Yo" },
			{ 'Ж', "Zh" },
			{ 'З', "Z" },
			{ 'И', "I" },
			{ 'Й', "J" },
			{ 'К', "K" },
			{ 'Л', "L" },
			{ 'М', "M" },
			{ 'Н', "N" },
			{ 'О', "O" },
			{ 'П', "P" },
			{ 'Р', "R" },
			{ 'С', "S" },
			{ 'Т', "T" },
			{ 'У', "U" },
			{ 'Ф', "F" },
			{ 'Х', "H" },
			{ 'Ц', "C" },
			{ 'Ч', "Ch" },
			{ 'Ш', "Sh" },
			{ 'Щ', "Sch" },
			{ 'Ъ', "J" },
			{ 'Ы', "I" },
			{ 'Ь', "J" },
			{ 'Э', "E" },
			{ 'Ю', "Yu" },
			{ 'Я', "Ya" }
		};
		SendReportsByWebhooks = false;
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CheaterReport), "CmdReport", InvokeUserCode_CmdReport, requiresAuthority: true);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_CmdReport(uint playerNetId, string reason, byte[] signature, bool notifyGm)
	{
		if (!_commandRateLimit.CanExecute() || reason == null)
		{
			return;
		}
		float num = global::UnityEngine.Time.time - _lastReport;
		if (num < 2f)
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Reporting rate limit exceeded (1).", "red");
			return;
		}
		if (num > 60f)
		{
			_reportedPlayersAmount = 0;
		}
		if (_reportedPlayersAmount > 5)
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Reporting rate limit exceeded (2).", "red");
			return;
		}
		if (notifyGm && (!ServerStatic.GetPermissionsHandler().IsVerified || string.IsNullOrEmpty(ServerConsole.Password)))
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Server is not verified - you can't use report feature on this server.", "red");
			return;
		}
		if (!ReferenceHub.TryGetHubNetID(playerNetId, out var hub))
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Can't find player with that PlayerID.", "red");
			return;
		}
		CharacterClassManager reportedCcm = hub.characterClassManager;
		if (_reportedPlayers == null)
		{
			_reportedPlayers = new global::System.Collections.Generic.HashSet<uint>();
		}
		if (_reportedPlayers.Contains(playerNetId))
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] You have already reported that player.", "red");
			return;
		}
		if (string.IsNullOrEmpty(reportedCcm.UserId))
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Failed: User ID of reported player is null.", "red");
			return;
		}
		if (string.IsNullOrEmpty(_hub.characterClassManager.UserId))
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Failed: your User ID of is null.", "red");
			return;
		}
		if (_hub.characterClassManager.UserId == reportedCcm.UserId)
		{
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] You can't report yourself!" + global::System.Environment.NewLine, "yellow");
			return;
		}
		string reportedNickname = hub.nicknameSync.MyNick;
		if (!notifyGm)
		{
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerReport, _hub, hub, reason))
			{
				return;
			}
			global::GameCore.Console.AddLog("Player " + _hub.LoggedNameFromRefHub() + " reported player " + hub.LoggedNameFromRefHub() + " with reason " + reason + ".", global::UnityEngine.Color.gray);
			_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Player report successfully sent to local administrators.", "green");
			if (SendReportsByWebhooks)
			{
				global::System.Threading.Thread thread = new global::System.Threading.Thread((global::System.Threading.ThreadStart)delegate
				{
					LogReport(_hub.gameConsoleTransmission, _hub.characterClassManager.UserId, reportedCcm.UserId, ref reason, playerNetId, notifyGm: false, _hub.nicknameSync.MyNick, reportedNickname);
				});
				thread.Priority = global::System.Threading.ThreadPriority.Lowest;
				thread.IsBackground = true;
				thread.Name = "Reporting player (locally) - " + reportedCcm.UserId + " by " + _hub.characterClassManager.UserId;
				thread.Start();
			}
		}
		else
		{
			if (signature == null)
			{
				return;
			}
			if (!global::Cryptography.ECDSA.VerifyBytes(reportedCcm.SyncedUserId + ";" + reason, signature, GetComponent<ServerRoles>().PublicKey))
			{
				_hub.gameConsoleTransmission.SendToClient(base.connectionToClient, "[REPORTING] Invalid report signature.", "red");
			}
			else if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerCheaterReport, _hub, hub, reason))
			{
				_lastReport = global::UnityEngine.Time.time;
				_reportedPlayersAmount++;
				global::GameCore.Console.AddLog("Player " + _hub.LoggedNameFromRefHub() + " reported player " + hub.LoggedNameFromRefHub() + " with reason " + reason + ". Sending report to Global Moderation.", global::UnityEngine.Color.gray);
				global::System.Threading.Thread thread2 = new global::System.Threading.Thread((global::System.Threading.ThreadStart)delegate
				{
					IssueReport(_hub.gameConsoleTransmission, _hub.characterClassManager.UserId, reportedCcm.UserId, reportedCcm.AuthToken, reportedCcm.connectionToClient.address, _hub.characterClassManager.AuthToken, _hub.connectionToClient.address, ref reason, ref signature, global::Cryptography.ECDSA.KeyToString(GetComponent<ServerRoles>().PublicKey), playerNetId, _hub.nicknameSync.MyNick, reportedNickname);
				});
				thread2.Priority = global::System.Threading.ThreadPriority.Lowest;
				thread2.IsBackground = true;
				thread2.Name = "Reporting player - " + reportedCcm.UserId + " by " + _hub.characterClassManager.UserId;
				thread2.Start();
			}
		}
	}

	protected static void InvokeUserCode_CmdReport(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdReport called on client.");
		}
		else
		{
			((CheaterReport)obj).UserCode_CmdReport(global::Mirror.NetworkReaderExtensions.ReadUInt32(reader), global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}
}
