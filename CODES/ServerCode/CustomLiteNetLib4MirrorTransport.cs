public class CustomLiteNetLib4MirrorTransport : global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport
{
	private enum ClientType : byte
	{
		GameClient = 0,
		VerificationService = 1
	}

	private static readonly global::LiteNetLib.Utils.NetDataWriter RequestWriter;

	public static GeoblockingMode Geoblocking;

	public static ChallengeType ChallengeMode;

	public static ushort ChallengeInitLen;

	public static ushort ChallengeSecretLen;

	public static readonly global::System.Collections.Generic.Dictionary<global::System.Net.IPEndPoint, PreauthItem> UserIds;

	public static readonly global::System.Collections.Generic.HashSet<string> UserIdFastReload;

	public static readonly global::System.Collections.Generic.Dictionary<string, PreauthChallengeItem> Challenges;

	public static bool UserRateLimiting;

	public static bool IpRateLimiting;

	public static bool UseGlobalBans;

	public static bool GeoblockIgnoreWhitelisted;

	public static bool UseChallenge;

	public static bool DisplayPreauthLogs;

	private static bool _delayConnections;

	public static bool SuppressRejections;

	public static bool SuppressIssued;

	public static uint Rejected;

	public static uint ChallengeIssued;

	public static byte DelayTime;

	internal static byte DelayVolume;

	internal static byte DelayVolumeThreshold;

	public static readonly global::System.Collections.Generic.HashSet<string> UserRateLimit;

	public static readonly global::System.Collections.Generic.HashSet<global::System.Net.IPAddress> IpRateLimit;

	public static readonly global::System.Collections.Generic.HashSet<string> GeoblockingList;

	public static RejectionReason LastRejectionReason;

	public static string LastCustomReason;

	public static string VerificationChallenge;

	public static string VerificationResponse;

	public static long LastBanExpiration;

	public static bool IpPassthroughEnabled;

	public static global::System.Collections.Generic.HashSet<global::System.Net.IPAddress> TrustedProxies;

	public static global::System.Collections.Generic.Dictionary<int, string> RealIpAddresses;

	public static uint RejectionThreshold;

	public static uint IssuedThreshold;

	public static bool DelayConnections
	{
		get
		{
			return _delayConnections;
		}
		set
		{
			if (_delayConnections != value)
			{
				if (!value)
				{
					UserIds.Clear();
				}
				_delayConnections = value;
				ServerConsole.AddLog(value ? $"Incoming connections will be now delayed by {DelayTime} seconds." : "Incoming connections will be no longer delayed.");
			}
		}
	}

	static CustomLiteNetLib4MirrorTransport()
	{
		RequestWriter = new global::LiteNetLib.Utils.NetDataWriter();
		Geoblocking = GeoblockingMode.None;
		ChallengeMode = ChallengeType.Reply;
		UserIds = new global::System.Collections.Generic.Dictionary<global::System.Net.IPEndPoint, PreauthItem>();
		UserIdFastReload = new global::System.Collections.Generic.HashSet<string>();
		Challenges = new global::System.Collections.Generic.Dictionary<string, PreauthChallengeItem>();
		_delayConnections = true;
		DelayTime = 3;
		UserRateLimit = new global::System.Collections.Generic.HashSet<string>();
		IpRateLimit = new global::System.Collections.Generic.HashSet<global::System.Net.IPAddress>();
		GeoblockingList = new global::System.Collections.Generic.HashSet<string>();
		RejectionThreshold = 60u;
		IssuedThreshold = 50u;
	}

	protected override void ProcessConnectionRequest(global::LiteNetLib.ConnectionRequest request)
	{
		try
		{
			if (!request.Data.TryGetByte(out var result) || result >= 2)
			{
				RequestWriter.Reset();
				RequestWriter.Put((byte)2);
				request.RejectForce(RequestWriter);
				return;
			}
			if (result == 1)
			{
				if (VerificationChallenge != null && request.Data.TryGetString(out var result2) && result2 == VerificationChallenge)
				{
					RequestWriter.Reset();
					RequestWriter.Put((byte)18);
					RequestWriter.Put(VerificationResponse);
					request.Reject(RequestWriter);
					VerificationChallenge = null;
					VerificationResponse = null;
					ServerConsole.AddLog("Verification challenge and response have been sent.", global::System.ConsoleColor.Green);
					return;
				}
				Rejected++;
				if (Rejected > RejectionThreshold)
				{
					SuppressRejections = true;
				}
				if (!SuppressRejections && DisplayPreauthLogs)
				{
					ServerConsole.AddLog($"Invalid verification challenge has been received from endpoint {request.RemoteEndPoint}.");
				}
				RequestWriter.Reset();
				RequestWriter.Put((byte)19);
				request.RejectForce(RequestWriter);
				return;
			}
			byte result3 = 0;
			if (!request.Data.TryGetByte(out var result4) || !request.Data.TryGetByte(out var result5) || !request.Data.TryGetByte(out var result6) || !request.Data.TryGetBool(out var result7) || (result7 && !request.Data.TryGetByte(out result3)))
			{
				RequestWriter.Reset();
				RequestWriter.Put((byte)3);
				request.RejectForce(RequestWriter);
				return;
			}
			if (!global::GameCore.Version.CompatibilityCheck(global::GameCore.Version.Major, global::GameCore.Version.Minor, global::GameCore.Version.Revision, result4, result5, result6, result7, result3))
			{
				RequestWriter.Reset();
				RequestWriter.Put((byte)3);
				request.RejectForce(RequestWriter);
				return;
			}
			int result8;
			bool flag = request.Data.TryGetInt(out result8);
			if (!request.Data.TryGetBytesWithLength(out var result9))
			{
				flag = false;
			}
			if (!flag)
			{
				RequestWriter.Reset();
				RequestWriter.Put((byte)15);
				request.RejectForce(RequestWriter);
				return;
			}
			if (DelayConnections)
			{
				PreauthDisableIdleMode();
				RequestWriter.Reset();
				RequestWriter.Put((byte)17);
				RequestWriter.Put(DelayTime);
				if (DelayVolume < byte.MaxValue)
				{
					DelayVolume++;
				}
				if (DelayVolume < DelayVolumeThreshold)
				{
					if (DisplayPreauthLogs)
					{
						ServerConsole.AddLog($"Delayed connection incoming from endpoint {request.RemoteEndPoint} by {DelayTime} seconds.");
					}
					request.Reject(RequestWriter);
				}
				else
				{
					if (DisplayPreauthLogs)
					{
						ServerConsole.AddLog($"Force delayed connection incoming from endpoint {request.RemoteEndPoint} by {DelayTime} seconds.");
					}
					request.RejectForce(RequestWriter);
				}
				return;
			}
			if (UseChallenge)
			{
				if (result8 == 0 || result9 == null || result9.Length == 0)
				{
					if (!CheckIpRateLimit(request))
					{
						return;
					}
					int num = 0;
					string key = string.Empty;
					for (byte b = 0; b < 3; b++)
					{
						num = RandomGenerator.GetInt32();
						if (num == 0)
						{
							num = 1;
						}
						key = string.Concat(request.RemoteEndPoint.Address, "-", num);
						if (!Challenges.ContainsKey(key))
						{
							break;
						}
						if (b == 2)
						{
							RequestWriter.Reset();
							RequestWriter.Put((byte)4);
							request.RejectForce(RequestWriter);
							if (DisplayPreauthLogs)
							{
								ServerConsole.AddLog($"Failed to generate ID for challenge for incoming connection from endpoint {request.RemoteEndPoint}.");
							}
							return;
						}
					}
					byte[] bytes = RandomGenerator.GetBytes(ChallengeInitLen + ChallengeSecretLen, secure: true);
					ChallengeIssued++;
					if (ChallengeIssued > IssuedThreshold)
					{
						SuppressIssued = true;
					}
					if (!SuppressIssued && DisplayPreauthLogs)
					{
						ServerConsole.AddLog($"Requested challenge for incoming connection from endpoint {request.RemoteEndPoint}.");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)13);
					RequestWriter.Put((byte)ChallengeMode);
					RequestWriter.Put(num);
					switch (ChallengeMode)
					{
					case ChallengeType.MD5:
						RequestWriter.PutBytesWithLength(bytes, 0, ChallengeInitLen);
						RequestWriter.Put(ChallengeSecretLen);
						RequestWriter.PutBytesWithLength(global::Cryptography.Md.Md5(bytes));
						Challenges.Add(key, new PreauthChallengeItem(new global::System.ArraySegment<byte>(bytes, ChallengeInitLen, ChallengeSecretLen)));
						break;
					case ChallengeType.SHA1:
						RequestWriter.PutBytesWithLength(bytes, 0, ChallengeInitLen);
						RequestWriter.Put(ChallengeSecretLen);
						RequestWriter.PutBytesWithLength(global::Cryptography.Sha.Sha1(bytes));
						Challenges.Add(key, new PreauthChallengeItem(new global::System.ArraySegment<byte>(bytes, ChallengeInitLen, ChallengeSecretLen)));
						break;
					default:
						RequestWriter.PutBytesWithLength(bytes);
						Challenges.Add(key, new PreauthChallengeItem(new global::System.ArraySegment<byte>(bytes)));
						break;
					}
					request.Reject(RequestWriter);
					PreauthDisableIdleMode();
					return;
				}
				string key2 = string.Concat(request.RemoteEndPoint.Address, "-", result8);
				if (!Challenges.ContainsKey(key2))
				{
					Rejected++;
					if (Rejected > RejectionThreshold)
					{
						SuppressRejections = true;
					}
					if (!SuppressRejections && DisplayPreauthLogs)
					{
						ServerConsole.AddLog($"Security challenge response of incoming connection from endpoint {request.RemoteEndPoint} has been REJECTED (invalid Challenge ID).");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)14);
					request.RejectForce(RequestWriter);
					return;
				}
				global::System.ArraySegment<byte> validResponse = Challenges[key2].ValidResponse;
				if (!global::System.Linq.Enumerable.SequenceEqual(result9, validResponse))
				{
					Rejected++;
					if (Rejected > RejectionThreshold)
					{
						SuppressRejections = true;
					}
					if (!SuppressRejections && DisplayPreauthLogs)
					{
						ServerConsole.AddLog($"Security challenge response of incoming connection from endpoint {request.RemoteEndPoint} has been REJECTED (invalid response).");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)15);
					request.RejectForce(RequestWriter);
					return;
				}
				Challenges.Remove(key2);
				PreauthDisableIdleMode();
				if (DisplayPreauthLogs)
				{
					ServerConsole.AddLog($"Security challenge response of incoming connection from endpoint {request.RemoteEndPoint} has been accepted.");
				}
			}
			else if (!CheckIpRateLimit(request))
			{
				return;
			}
			int position = request.Data.Position;
			if (!CharacterClassManager.OnlineMode)
			{
				global::System.Collections.Generic.KeyValuePair<BanDetails, BanDetails> keyValuePair = BanHandler.QueryBan(null, request.RemoteEndPoint.Address.ToString());
				if (keyValuePair.Value != null)
				{
					if (DisplayPreauthLogs)
					{
						ServerConsole.AddLog($"Player tried to connect from banned endpoint {request.RemoteEndPoint}.");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)6);
					RequestWriter.Put(keyValuePair.Value.Expires);
					RequestWriter.Put(keyValuePair.Value?.Reason ?? string.Empty);
					request.RejectForce(RequestWriter);
					ResetIdleMode();
				}
				else if (ProcessCancellationData(request, global::PluginAPI.Events.EventManager.ExecuteEvent<global::PluginAPI.Events.PreauthCancellationData>(global::PluginAPI.Enums.ServerEventType.PlayerPreauth, new object[8]
				{
					null,
					request.RemoteEndPoint.Address.ToString(),
					0,
					CentralAuthPreauthFlags.None,
					null,
					null,
					request,
					position
				})))
				{
					request.Accept();
					PreauthDisableIdleMode();
				}
				return;
			}
			if (!request.Data.TryGetString(out var result10) || result10 == string.Empty)
			{
				RequestWriter.Reset();
				RequestWriter.Put((byte)5);
				request.RejectForce(RequestWriter);
				return;
			}
			if (!request.Data.TryGetLong(out var result11) || !request.Data.TryGetByte(out var result12) || !request.Data.TryGetString(out var result13) || !request.Data.TryGetBytesWithLength(out var result14))
			{
				RequestWriter.Reset();
				RequestWriter.Put((byte)4);
				request.RejectForce(RequestWriter);
				return;
			}
			string result15 = null;
			string text = ((IpPassthroughEnabled && TrustedProxies.Contains(request.RemoteEndPoint.Address) && request.Data.TryGetString(out result15)) ? $"{result15} [routed via {request.RemoteEndPoint}]" : request.RemoteEndPoint.ToString());
			CentralAuthPreauthFlags centralAuthPreauthFlags = (CentralAuthPreauthFlags)result12;
			try
			{
				if (!global::Cryptography.ECDSA.VerifyBytes($"{result10};{result12};{result13};{result11}", result14, ServerConsole.PublicKey))
				{
					Rejected++;
					if (Rejected > RejectionThreshold)
					{
						SuppressRejections = true;
					}
					if (!SuppressRejections && DisplayPreauthLogs)
					{
						ServerConsole.AddLog("Player from endpoint " + text + " sent preauthentication token with invalid digital signature.");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)2);
					request.RejectForce(RequestWriter);
					ResetIdleMode();
					return;
				}
				if (TimeBehaviour.CurrentUnixTimestamp > result11)
				{
					Rejected++;
					if (Rejected > RejectionThreshold)
					{
						SuppressRejections = true;
					}
					if (!SuppressRejections && DisplayPreauthLogs)
					{
						ServerConsole.AddLog("Player from endpoint " + text + " sent expired preauthentication token.");
						ServerConsole.AddLog("Make sure that time and timezone set on server is correct. We recommend synchronizing the time.");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)11);
					request.RejectForce(RequestWriter);
					ResetIdleMode();
					return;
				}
				if (UserRateLimiting)
				{
					if (UserRateLimit.Contains(result10))
					{
						Rejected++;
						if (Rejected > RejectionThreshold)
						{
							SuppressRejections = true;
						}
						if (!SuppressRejections && DisplayPreauthLogs)
						{
							ServerConsole.AddLog("Incoming connection from " + result10 + " (" + text + ") rejected due to exceeding the rate limit.");
						}
						RequestWriter.Reset();
						RequestWriter.Put((byte)12);
						request.RejectForce(RequestWriter);
						ResetIdleMode();
						return;
					}
					UserRateLimit.Add(result10);
				}
				if (!centralAuthPreauthFlags.HasFlagFast(CentralAuthPreauthFlags.IgnoreBans) || !ServerStatic.GetPermissionsHandler().IsVerified)
				{
					global::System.Collections.Generic.KeyValuePair<BanDetails, BanDetails> keyValuePair2 = BanHandler.QueryBan(result10, result15 ?? request.RemoteEndPoint.Address.ToString());
					if (keyValuePair2.Key != null || keyValuePair2.Value != null)
					{
						Rejected++;
						if (Rejected > RejectionThreshold)
						{
							SuppressRejections = true;
						}
						if (!SuppressRejections && DisplayPreauthLogs)
						{
							ServerConsole.AddLog(((keyValuePair2.Key == null) ? "Player" : "Banned player") + " " + result10 + " tried to connect from " + ((keyValuePair2.Value == null) ? "" : "banned ") + " endpoint " + text + ".");
							ServerLogs.AddLog(ServerLogs.Modules.Networking, ((keyValuePair2.Key == null) ? "Player" : "Banned player") + " " + result10 + " tried to connect from " + ((keyValuePair2.Value == null) ? "" : "banned ") + " endpoint " + text + ".", ServerLogs.ServerLogType.ConnectionUpdate);
						}
						RequestWriter.Reset();
						RequestWriter.Put((byte)6);
						RequestWriter.Put(keyValuePair2.Key?.Expires ?? keyValuePair2.Value.Expires);
						RequestWriter.Put(keyValuePair2.Key?.Reason ?? keyValuePair2.Value?.Reason ?? string.Empty);
						request.Reject(RequestWriter);
						ResetIdleMode();
						return;
					}
				}
				if (centralAuthPreauthFlags.HasFlagFast(CentralAuthPreauthFlags.AuthRejected))
				{
					if (DisplayPreauthLogs)
					{
						ServerConsole.AddLog("Player " + result10 + " (" + text + ") kicked due to auth rejection by central server.");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)20);
					request.Reject(RequestWriter);
					ResetIdleMode();
					return;
				}
				if (centralAuthPreauthFlags.HasFlagFast(CentralAuthPreauthFlags.GloballyBanned) && (ServerStatic.PermissionsHandler.IsVerified || UseGlobalBans))
				{
					if (DisplayPreauthLogs)
					{
						ServerConsole.AddLog("Player " + result10 + " (" + text + ") kicked due to an active global ban.");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)8);
					request.Reject(RequestWriter);
					ResetIdleMode();
					return;
				}
				if ((!centralAuthPreauthFlags.HasFlagFast(CentralAuthPreauthFlags.IgnoreWhitelist) || !ServerStatic.GetPermissionsHandler().IsVerified) && !WhiteList.IsWhitelisted(result10))
				{
					if (DisplayPreauthLogs)
					{
						ServerConsole.AddLog("Player " + result10 + " tried joined from endpoint " + text + ", but is not whitelisted.");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)7);
					request.Reject(RequestWriter);
					ResetIdleMode();
					return;
				}
				if (Geoblocking != GeoblockingMode.None && (!centralAuthPreauthFlags.HasFlagFast(CentralAuthPreauthFlags.IgnoreGeoblock) || !ServerStatic.PermissionsHandler.BanTeamBypassGeo) && (!GeoblockIgnoreWhitelisted || !WhiteList.IsOnWhitelist(result10)) && ((Geoblocking == GeoblockingMode.Whitelist && !GeoblockingList.Contains(result13.ToUpper())) || (Geoblocking == GeoblockingMode.Blacklist && GeoblockingList.Contains(result13.ToUpper()))))
				{
					Rejected++;
					if (Rejected > RejectionThreshold)
					{
						SuppressRejections = true;
					}
					if (!SuppressRejections && DisplayPreauthLogs)
					{
						ServerConsole.AddLog("Player " + result10 + " (" + text + ") tried joined from blocked country " + result13.ToUpper() + ".");
					}
					RequestWriter.Reset();
					RequestWriter.Put((byte)9);
					request.RejectForce(RequestWriter);
					ResetIdleMode();
					return;
				}
				if (UserIdFastReload.Contains(result10))
				{
					UserIdFastReload.Remove(result10);
				}
				if (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.Host.ConnectedPeersCount < CustomNetworkManager.slots || (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.Host.ConnectedPeersCount != global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorNetworkManager.singleton.maxConnections && ((centralAuthPreauthFlags.HasFlagFast(CentralAuthPreauthFlags.ReservedSlot) && ServerStatic.PermissionsHandler.BanTeamSlots) || (global::GameCore.ConfigFile.ServerConfig.GetBool("use_reserved_slots", def: true) && ReservedSlot.HasReservedSlot(result10, out var bypass) && (bypass || global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.Host.ConnectedPeersCount < CustomNetworkManager.slots + CustomNetworkManager.reservedSlots)))))
				{
					if (!ProcessCancellationData(request, global::PluginAPI.Events.EventManager.ExecuteEvent<global::PluginAPI.Events.PreauthCancellationData>(global::PluginAPI.Enums.ServerEventType.PlayerPreauth, new object[8]
					{
						result10,
						request.RemoteEndPoint.Address.ToString(),
						result11,
						centralAuthPreauthFlags,
						result13,
						result14,
						request,
						position
					})))
					{
						return;
					}
					if (UserIds.ContainsKey(request.RemoteEndPoint))
					{
						UserIds[request.RemoteEndPoint].SetUserId(result10);
					}
					else
					{
						UserIds.Add(request.RemoteEndPoint, new PreauthItem(result10));
					}
					global::LiteNetLib.NetPeer netPeer = request.Accept();
					if (result15 != null)
					{
						if (RealIpAddresses.ContainsKey(netPeer.Id))
						{
							RealIpAddresses[netPeer.Id] = result15;
						}
						else
						{
							RealIpAddresses.Add(netPeer.Id, result15);
						}
					}
					if (global::PluginAPI.Core.Statistics.PeakPlayers.Total < global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.Host.ConnectedPeersCount)
					{
						global::PluginAPI.Core.Statistics.PeakPlayers = new global::PluginAPI.Core.Statistics.Peak(global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.Host.ConnectedPeersCount, global::System.DateTime.Now);
					}
					ServerConsole.AddLog("Player " + result10 + " preauthenticated from endpoint " + text + ".");
					ServerLogs.AddLog(ServerLogs.Modules.Networking, result10 + " preauthenticated from endpoint " + text + ".", ServerLogs.ServerLogType.ConnectionUpdate);
					PreauthDisableIdleMode();
				}
				else
				{
					RequestWriter.Reset();
					RequestWriter.Put((byte)1);
					request.Reject(RequestWriter);
					ResetIdleMode();
				}
			}
			catch (global::System.Exception ex)
			{
				Rejected++;
				if (Rejected > RejectionThreshold)
				{
					SuppressRejections = true;
				}
				if (!SuppressRejections && DisplayPreauthLogs)
				{
					ServerConsole.AddLog("Player from endpoint " + text + " sent an invalid preauthentication token. " + ex.Message);
				}
				RequestWriter.Reset();
				RequestWriter.Put((byte)2);
				request.RejectForce(RequestWriter);
				ResetIdleMode();
			}
		}
		catch (global::System.Exception ex2)
		{
			Rejected++;
			if (Rejected > RejectionThreshold)
			{
				SuppressRejections = true;
			}
			if (!SuppressRejections)
			{
				ServerConsole.AddLog($"Player from endpoint {request.RemoteEndPoint} failed to preauthenticate: {ex2.Message}");
			}
			RequestWriter.Reset();
			RequestWriter.Put((byte)4);
			request.RejectForce(RequestWriter);
		}
	}

	private static bool ProcessCancellationData(global::LiteNetLib.ConnectionRequest request, global::PluginAPI.Events.PreauthCancellationData pcd)
	{
		if (!pcd.IsCancelled)
		{
			return true;
		}
		if (DisplayPreauthLogs)
		{
			ServerConsole.AddLog($"Incoming connection from {request.RemoteEndPoint} rejected by a plugin.");
		}
		bool forced;
		global::LiteNetLib.Utils.NetDataWriter netDataWriter = pcd.GenerateWriter(out forced);
		if (netDataWriter == null)
		{
			return false;
		}
		if (forced)
		{
			request.RejectForce(netDataWriter);
		}
		else
		{
			request.Reject(netDataWriter);
		}
		return false;
	}

	private static bool CheckIpRateLimit(global::LiteNetLib.ConnectionRequest request)
	{
		if (!IpRateLimiting)
		{
			return true;
		}
		if (IpRateLimit.Contains(request.RemoteEndPoint.Address))
		{
			Rejected++;
			if (Rejected > RejectionThreshold)
			{
				SuppressRejections = true;
			}
			if (!SuppressRejections)
			{
				ServerConsole.AddLog($"Incoming connection from endpoint {request.RemoteEndPoint} rejected due to exceeding the rate limit.");
				ServerLogs.AddLog(ServerLogs.Modules.Networking, $"Incoming connection from endpoint {request.RemoteEndPoint} rejected due to exceeding the rate limit.", ServerLogs.ServerLogType.RateLimit);
			}
			RequestWriter.Reset();
			RequestWriter.Put((byte)12);
			request.RejectForce(RequestWriter);
			return false;
		}
		IpRateLimit.Add(request.RemoteEndPoint.Address);
		return true;
	}

	protected override void GetConnectData(global::LiteNetLib.Utils.NetDataWriter writer)
	{
	}

	protected override void OnConncetionRefused(global::LiteNetLib.DisconnectInfo disconnectinfo)
	{
		if (disconnectinfo.AdditionalData.TryGetByte(out var result))
		{
			LastRejectionReason = (RejectionReason)result;
		}
		else
		{
			LastRejectionReason = RejectionReason.NotSpecified;
		}
	}

	private static void ResetIdleMode()
	{
		if (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.Host.ConnectedPeersCount == 0)
		{
			IdleMode.SetIdleMode(state: true);
		}
	}

	public static void PreauthDisableIdleMode()
	{
		if (ServerStatic.IsDedicated && IdleMode.IdleModeActive)
		{
			IdleMode.PreauthStopwatch.Restart();
			IdleMode.SetIdleMode(state: false);
		}
	}

	public static void ReloadChallengeOptions()
	{
		UseChallenge = global::GameCore.ConfigFile.ServerConfig.GetBool("preauth_challenge", def: true);
		ChallengeInitLen = global::GameCore.ConfigFile.ServerConfig.GetUShort("preauth_challenge_base_length", 16);
		ChallengeSecretLen = global::GameCore.ConfigFile.ServerConfig.GetUShort("preauth_challenge_secret_length", 5);
		string text = global::GameCore.ConfigFile.ServerConfig.GetString("preauth_challenge_mode", "reply").ToLower();
		if (!(text == "md5"))
		{
			if (text == "sha1")
			{
				ChallengeMode = ChallengeType.SHA1;
				return;
			}
			ChallengeMode = ChallengeType.Reply;
			ChallengeSecretLen = 0;
		}
		else
		{
			ChallengeMode = ChallengeType.MD5;
		}
	}
}
