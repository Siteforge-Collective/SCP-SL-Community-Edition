using System.Collections.Generic;
using Mirror;
using Security;

public class CheaterReport : NetworkBehaviour
{
	private static readonly Dictionary<char, string> CharacterReplacements;

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

	private HashSet<uint> _reportedPlayers;

	private RateLimit _commandRateLimit;

	private ReferenceHub _hub;

	private void Start()
	{
	}

	internal void Report(uint playerNetId, string reason, bool notifyGm)
	{
	}

	[Command(channel = 4)]
	private void CmdReport(uint playerNetId, string reason, byte[] signature, bool notifyGm)
	{
	}

	[Server]
	private void IssueReport(GameConsoleTransmission reporter, string reporterUserId, string reportedUserId, string reportedAuth, string reportedIp, string reporterAuth, string reporterIp, ref string reason, ref byte[] signature, string reporterPublicKey, uint reportedNetId, string reporterNickname, string reportedNickname)
	{
	}

	[Server]
	private void LogReport(GameConsoleTransmission reporter, string reporterUserId, string reportedUserId, ref string reason, uint reportedNetId, bool notifyGm, string reporterNickname, string reportedNickname)
	{
	}

	[Server]
	private void SendStaffChatNotification(string reporterUserId, string reportedUserId, string reason, string reporterNickname, string reportedNickname)
	{
	}

	internal static bool SubmitReport(string reporterUserId, string reportedUserId, string reason, uint reportedId, string reporterNickname, string reportedNickname, bool friendlyFire)
	{
		return false;
	}

	private static string ConvertToLatin(string str)
	{
		return null;
	}

	private static string AsDiscordCode(string text)
	{
		return null;
	}

	private static string DiscordSanitize(string text)
	{
		return null;
	}

	static CheaterReport()
	{
	}

	protected void UserCode_CmdReport__UInt32__String__Byte_005B_005D__Boolean(uint playerNetId, string reason, byte[] signature, bool notifyGm)
	{
	}

	protected static void InvokeUserCode_CmdReport__UInt32__String__Byte_005B_005D__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}
}
