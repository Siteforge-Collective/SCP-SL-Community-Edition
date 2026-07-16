public class CentralAuth : global::UnityEngine.MonoBehaviour
{
	public static bool GlobalBadgeIssued;

	private ICentralAuth _ica;

	private static bool _awaitingTicket;

	private static bool _awaitingRequest;

	public static CentralAuth singleton;

	private void Awake()
	{
		singleton = this;
	}

	private static void AuthDebug(string msg, string color = "blue")
	{
		global::GameCore.Console.AddDebugLog("SDAUTH", "<color=" + color + ">" + msg + "</color>", MessageImportance.Normal);
	}

	private void Update()
	{
	}

	internal void GenerateToken(ICentralAuth icaa)
	{
	}

	public void StartValidateToken(ICentralAuth icaa, string token, global::System.Net.IPEndPoint endpoint)
	{
		global::MEC.Timing.RunCoroutine(_ValidateToken(icaa, token, endpoint), global::MEC.Segment.FixedUpdate);
	}

	private global::System.Collections.Generic.IEnumerator<float> _ValidateToken(ICentralAuth icaa, string token, global::System.Net.IPEndPoint endpoint)
	{
		if (string.IsNullOrEmpty(token) || !token.Contains("<br>Signature: "))
		{
			icaa.FailToken("Malformed token.");
			yield break;
		}
		try
		{
			string text = token.Substring(0, token.IndexOf("<br>Signature: ", global::System.StringComparison.Ordinal));
			string text2 = token.Substring(token.IndexOf("<br>Signature: ", global::System.StringComparison.Ordinal) + 15);
			text2 = text2.Replace("<br>", "");
			if (!global::Cryptography.ECDSA.Verify(text, text2, ServerConsole.PublicKey))
			{
				ServerConsole.AddLog("Authentication token signature mismatch.");
				icaa.GetHub().gameConsoleTransmission.SendToClient("Authentication token rejected due to signature mismatch.", "red");
				icaa.FailToken("Failed to validate authentication token signature.");
			}
			else
			{
				global::System.Collections.Generic.Dictionary<string, string> dictionary = global::System.Linq.Enumerable.ToDictionary(global::System.Linq.Enumerable.Select(text.Split(new string[1] { "<br>" }, global::System.StringSplitOptions.None), (string rwr) => rwr.Split(new string[1] { ": " }, global::System.StringSplitOptions.None)), (string[] split) => split[0], (string[] split) => split[1]);
				string text3 = RemoveSalt(dictionary["User ID"]);
				if (dictionary["Usage"] != "Authentication")
				{
					ServerConsole.AddLog("Player tried to use token not issued to authentication purposes.");
					icaa.GetHub().gameConsoleTransmission.SendToClient("Authentication token rejected due to invalid purpose of signature.", "red");
					_ica.FailToken("Token supplied by your game can't be used for authentication purposes.");
				}
				else if (endpoint != null && (!CustomLiteNetLib4MirrorTransport.UserIds.ContainsKey(endpoint) || !CustomLiteNetLib4MirrorTransport.UserIds[endpoint].UserId.Equals(text3, global::System.StringComparison.Ordinal)) && !CustomLiteNetLib4MirrorTransport.UserIdFastReload.Contains(text3))
				{
					ServerConsole.AddLog("Player tried to use token issued to a different user than the preauthentication token.");
					icaa.GetHub().gameConsoleTransmission.SendToClient("UserID mismatch between authentication and preauthentication token.", "red");
					icaa.GetHub().gameConsoleTransmission.SendToClient("Preauth: " + (CustomLiteNetLib4MirrorTransport.UserIds.ContainsKey(endpoint) ? CustomLiteNetLib4MirrorTransport.UserIds[endpoint].UserId : "(null)"), "red");
					icaa.GetHub().gameConsoleTransmission.SendToClient("Auth: " + RemoveSalt(dictionary["User ID"]), "red");
					_ica.FailToken("UserID mismatch between authentication and preauthentication token. Check the game console for more details.");
				}
				else if (dictionary["Test signature"] != "NO" && !CentralServer.TestServer)
				{
					ServerConsole.AddLog("Player tried to use authentication token issued only for testing. Server: " + dictionary["Issued by"] + ".");
					icaa.GetHub().gameConsoleTransmission.SendToClient("Authentication token rejected due to testing signature.", "red");
					_ica.FailToken("Your authentication token is issued only for testing purposes.");
				}
				else if (!dictionary.ContainsKey("Auth Version") || dictionary["Auth Version"] != "2")
				{
					ServerConsole.AddLog("Player used invalid version of authentication token. Server: " + dictionary["Issued by"] + ".");
					icaa.GetHub().gameConsoleTransmission.SendToClient("Authentication token version mismatch. Required version: 2.", "red");
					_ica.FailToken("This version of game requires authentication token version 2.");
				}
				else
				{
					global::System.DateTime dateTime = global::System.DateTime.ParseExact(dictionary["Expiration time"], "yyyy-MM-dd HH:mm:ss", global::System.Globalization.CultureInfo.InvariantCulture);
					global::System.DateTime dateTime2 = global::System.DateTime.ParseExact(dictionary["Issuance time"], "yyyy-MM-dd HH:mm:ss", global::System.Globalization.CultureInfo.InvariantCulture);
					if (dateTime < global::System.DateTime.UtcNow)
					{
						ServerConsole.AddLog("Player tried to use expired authentication token. Server: " + dictionary["Issued by"] + ".");
						ServerConsole.AddLog("Make sure that time and timezone set on server is correct. We recommend synchronizing the time.");
						icaa.GetHub().gameConsoleTransmission.SendToClient("Authentication token rejected due to expired signature.", "red");
						_ica.FailToken("Your authentication token has expired.");
					}
					else if (dateTime2 > global::System.DateTime.UtcNow.AddMinutes(20.0))
					{
						ServerConsole.AddLog("Player tried to use non-issued authentication token. Server: " + dictionary["Issued by"] + ".");
						ServerConsole.AddLog("Make sure that time and timezone set on server is correct. We recommend synchronizing the time.");
						icaa.GetHub().gameConsoleTransmission.SendToClient("Authentication token rejected due to non-issued signature.", "red");
						_ica.FailToken("Your authentication token has invalid issuance date.");
					}
					else if (global::GameCore.Version.PrivateBeta && (!dictionary.ContainsKey("Private beta ownership") || dictionary["Private beta ownership"] != "YES"))
					{
						ServerConsole.AddLog("Player " + dictionary["User ID"] + " tried to join this server, but is not Private Beta DLC owner. Server: " + dictionary["Issued by"] + ".");
						icaa.GetHub().gameConsoleTransmission.SendToClient("Private Beta DLC ownership is required to join private beta server.", "red");
						_ica.FailToken("Private Beta DLC ownership is required to join private beta server.");
					}
					else
					{
						ReferenceHub hub = icaa.GetHub();
						ServerRoles serverRoles = hub.serverRoles;
						serverRoles.FirstVerResult = dictionary;
						if (dictionary.ContainsKey("Sync Hashed") && dictionary["Sync Hashed"] == "YES")
						{
							serverRoles.SyncHashed = true;
						}
						icaa.Ok(dictionary["User ID"], dictionary.ContainsKey("User ID 2") ? dictionary["User ID 2"] : null, dictionary["Global ban"], dictionary["Issued by"], dictionary["Bypass bans"] == "YES", dictionary["Bypass WL"] == "YES", dictionary.ContainsKey("Do Not Track") && dictionary["Do Not Track"] == "YES", dictionary.ContainsKey("Serial") ? dictionary["Serial"] : "N/A", dictionary.ContainsKey("VAC session") ? dictionary["VAC session"] : "N/A", dictionary.ContainsKey("Request IP") ? dictionary["Request IP"] : "N/A", dictionary.ContainsKey("ASN") ? dictionary["ASN"] : "N/A", dictionary.ContainsKey("Skip IP Check") && dictionary["Skip IP Check"] == "YES");
						string text4 = string.Format("{0} authenticated from endpoint {1}. Player ID assigned: {2}. Auth token serial number: {3}", RemoveSalt(dictionary["User ID"]), (endpoint == null) ? "(null)" : endpoint.ToString(), hub.PlayerId, dictionary.ContainsKey("Serial") ? dictionary["Serial"] : "N/A");
						ServerConsole.AddLog(text4);
						ServerLogs.AddLog(ServerLogs.Modules.Networking, text4, ServerLogs.ServerLogType.ConnectionUpdate);
					}
					if (endpoint != null && CustomLiteNetLib4MirrorTransport.UserIds.ContainsKey(endpoint))
					{
						CustomLiteNetLib4MirrorTransport.UserIds.Remove(endpoint);
					}
					if (CustomLiteNetLib4MirrorTransport.UserIdFastReload.Contains(text3))
					{
						CustomLiteNetLib4MirrorTransport.UserIdFastReload.Remove(text3);
					}
				}
			}
		}
		catch (global::System.Exception ex)
		{
			ServerConsole.AddLog("Error during authentication token verification: " + ex.Message);
			icaa.Fail();
		}
		yield return float.NegativeInfinity;
	}

	internal static global::System.Collections.Generic.Dictionary<string, string> ValidatePartialAuthToken(string token, string userId, string nickname, string authSerial, string usage)
	{
		if (string.IsNullOrEmpty(token) || !token.Contains("<br>Signature: "))
		{
			return null;
		}
		try
		{
			string text = token.Substring(0, token.IndexOf("<br>Signature: ", global::System.StringComparison.Ordinal));
			string text2 = token.Substring(token.IndexOf("<br>Signature: ", global::System.StringComparison.Ordinal) + 15);
			text2 = text2.Replace("<br>", "");
			if (!global::Cryptography.ECDSA.Verify(text, text2, ServerConsole.PublicKey))
			{
				ServerConsole.AddLog("Partial auth token signature mismatch.");
				return null;
			}
			global::System.Collections.Generic.Dictionary<string, string> dictionary = global::System.Linq.Enumerable.ToDictionary(global::System.Linq.Enumerable.Select(text.Split(new string[1] { "<br>" }, global::System.StringSplitOptions.None), (string rwr) => rwr.Split(new string[1] { ": " }, global::System.StringSplitOptions.None)), (string[] split) => split[0], (string[] split) => split[1]);
			if (dictionary["Usage"] != usage)
			{
				ServerConsole.AddLog("Player tried to use a partial auth token issued to a different purpose.");
				return null;
			}
			if (authSerial != null && dictionary["Serial"] != authSerial)
			{
				ServerConsole.AddLog("Partial auth token serial mismatch.");
				return null;
			}
			if (dictionary["Test signature"] != "NO")
			{
				ServerConsole.AddLog("Player tried to use a partial auth token issued only for testing. Server: " + dictionary["Issued by"] + ".");
				return null;
			}
			if (string.IsNullOrEmpty(userId))
			{
				ServerConsole.AddLog("Player tried to use a partial auth token issued for different user (User ID mismatch - empty). Server: " + dictionary["Issued by"] + ".");
				return null;
			}
			string text3 = global::Cryptography.Sha.HashToString(global::Cryptography.Sha.Sha512(userId));
			string text4 = global::Cryptography.Sha.HashToString(global::Cryptography.Sha.Sha512(dictionary["User ID"]));
			if (dictionary["User ID"] != userId && dictionary["User ID"] != text3 && text4 != userId && text4 != text3)
			{
				ServerConsole.AddLog("Player tried to use a partial auth token issued for different user (User ID mismatch). Server: " + dictionary["Issued by"] + ".");
				return null;
			}
			if (global::NorthwoodLib.StringUtils.Base64Decode(dictionary["Nickname"]) != nickname)
			{
				ServerConsole.AddLog("Player tried to use a partial auth token issued for different user (nickname mismatch). Server: " + dictionary["Issued by"] + ".");
				return null;
			}
			global::System.DateTime dateTime = global::System.DateTime.ParseExact(dictionary["Expiration time"], "yyyy-MM-dd HH:mm:ss", global::System.Globalization.CultureInfo.InvariantCulture);
			global::System.DateTime dateTime2 = global::System.DateTime.ParseExact(dictionary["Issuance time"], "yyyy-MM-dd HH:mm:ss", global::System.Globalization.CultureInfo.InvariantCulture);
			if (dateTime < global::System.DateTime.UtcNow)
			{
				ServerConsole.AddLog("Player tried to use expired partial auth request token. Server: " + dictionary["Issued by"] + ".");
				ServerConsole.AddLog("Make sure that time and timezone set on server is correct. We recommend synchronizing the time.");
				return null;
			}
			if (dateTime2 > global::System.DateTime.UtcNow.AddMinutes(20.0))
			{
				ServerConsole.AddLog("Player tried to use non-issued partial auth token. Server: " + dictionary["Issued by"] + ".");
				ServerConsole.AddLog("Make sure that time and timezone set on server is correct. We recommend synchronizing the time.");
				return null;
			}
			return dictionary;
		}
		catch (global::System.Exception ex)
		{
			ServerConsole.AddLog("Error during partial auth token verification: " + ex.Message);
			global::UnityEngine.Debug.Log("Error during partial auth token verification: " + ex.Message + " StackTrace: " + ex.StackTrace);
			return null;
		}
	}

	public static string RemoveSalt(string userId)
	{
		if (userId.Contains("$"))
		{
			return userId.Substring(0, userId.IndexOf("$", global::System.StringComparison.Ordinal));
		}
		return userId;
	}
}
