namespace RemoteAdmin
{
	public class QueryProcessor : global::Mirror.NetworkBehaviour
	{
		public struct CommandData
		{
			public string Command;

			public string[] Usage;

			public string Description;

			public string AliasOf;

			public bool Hidden;

			public override bool Equals(object obj)
			{
				if (obj is global::RemoteAdmin.QueryProcessor.CommandData other)
				{
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (Command, Usage, Description, AliasOf, Hidden).GetHashCode();
			}

			public bool Equals(global::RemoteAdmin.QueryProcessor.CommandData other)
			{
				if (Command == other.Command && Usage == other.Usage && Description == other.Description && AliasOf == other.AliasOf)
				{
					return Hidden == other.Hidden;
				}
				return false;
			}

			public static bool operator ==(global::RemoteAdmin.QueryProcessor.CommandData lhs, global::RemoteAdmin.QueryProcessor.CommandData rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(global::RemoteAdmin.QueryProcessor.CommandData lhs, global::RemoteAdmin.QueryProcessor.CommandData rhs)
			{
				return !lhs.Equals(rhs);
			}
		}

		public global::RemoteAdmin.RemoteAdminCryptographicManager CryptoManager;

		public GameConsoleTransmission GCT;

		private ServerRoles _roles;

		private global::RemoteAdmin.PlayerCommandSender _sender;

		private global::Security.RateLimit _commandRateLimit;

		private static global::Org.BouncyCastle.Security.SecureRandom _secureRandom;

		private static global::RemoteAdmin.QueryProcessor.CommandData[] _commands;

		public static bool Lockdown;

		private const int HashIterations = 250;

		internal int PasswordTries;

		internal int SignaturesCounter;

		private int _signaturesCounter;

		internal byte[] Key;

		internal byte[] Salt;

		internal byte[] ClientSalt;

		private byte[] _key;

		private byte[] _salt;

		private byte[] _clientSalt;

		private float _lastPlayerlistRequest;

		private bool _commandsSynced;

		[global::Mirror.SyncVar(hook = "SetServerRandom")]
		private string _syncServerRandom;

		private static string _serverStaticRandom;

		private static bool _eventsAssgined;

		[global::System.NonSerialized]
		[global::Mirror.SyncVar]
		public bool OverridePasswordEnabled;

		internal bool PasswordSent;

		private bool _gameplayData;

		private bool _gdDirty;

		private ReferenceHub _hub;

		private const int CommandDescriptionSyncMaxLength = 80;

		internal static readonly char[] SpaceArray;

		public static readonly global::CommandSystem.ClientCommandHandler DotCommandHandler;

		private string _ipAddress;

		private global::Mirror.NetworkConnection _conns;

		public string ServerRandom => ReferenceHub.HostHub.queryProcessor._syncServerRandom;

		public bool IsHost => !string.IsNullOrEmpty(_syncServerRandom);

		public bool GameplayData
		{
			get
			{
				return _gameplayData;
			}
			set
			{
				_gameplayData = value;
				_gdDirty = true;
			}
		}

		public string Network_syncServerRandom
		{
			get
			{
				return _syncServerRandom;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncServerRandom))
				{
					string syncServerRandom = _syncServerRandom;
					SetSyncVar(value, ref _syncServerRandom, 1uL);
					if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
					{
						setSyncVarHookGuard(1uL, value: true);
						SetServerRandom(syncServerRandom, value);
						setSyncVarHookGuard(1uL, value: false);
					}
				}
			}
		}

		public bool NetworkOverridePasswordEnabled
		{
			get
			{
				return OverridePasswordEnabled;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref OverridePasswordEnabled))
				{
					bool overridePasswordEnabled = OverridePasswordEnabled;
					SetSyncVar(value, ref OverridePasswordEnabled, 2uL);
				}
			}
		}

		private void Awake()
		{
			_hub = ReferenceHub.GetHub(base.gameObject);
			_roles = _hub.serverRoles;
			CryptoManager = GetComponent<global::RemoteAdmin.RemoteAdminCryptographicManager>();
			GCT = GetComponent<GameConsoleTransmission>();
		}

		private void Start()
		{
			_commandRateLimit = _hub.playerRateLimitHandler.RateLimits[2];
			if (_secureRandom == null)
			{
				_secureRandom = new global::Org.BouncyCastle.Security.SecureRandom();
			}
			SignaturesCounter = 0;
			_signaturesCounter = 0;
			if (global::Mirror.NetworkServer.active)
			{
				_conns = base.connectionToClient;
				_ipAddress = _conns.address;
				NetworkOverridePasswordEnabled = ServerStatic.PermissionsHandler.OverrideEnabled;
				if (string.IsNullOrEmpty(_serverStaticRandom))
				{
					byte[] array;
					using (global::System.Security.Cryptography.RandomNumberGenerator randomNumberGenerator = new global::System.Security.Cryptography.RNGCryptoServiceProvider())
					{
						array = new byte[32];
						randomNumberGenerator.GetBytes(array);
					}
					_serverStaticRandom = global::System.Convert.ToBase64String(array);
					ServerConsole.AddLog("Generated round random salt: " + _serverStaticRandom);
				}
				if (base.isLocalPlayer)
				{
					_commands = ParseCommandsToStruct(global::RemoteAdmin.CommandProcessor.GetAllCommands());
					if (string.IsNullOrEmpty(_syncServerRandom))
					{
						Network_syncServerRandom = _serverStaticRandom;
					}
				}
			}
			else if (base.isLocalPlayer)
			{
				_commands = null;
			}
			_sender = new global::RemoteAdmin.PlayerCommandSender(_hub);
			_ = base.isLocalPlayer;
		}

		public void SetServerRandom(string prev, string random)
		{
		}

		private void Update()
		{
			if (base.isLocalPlayer && _lastPlayerlistRequest < 1f)
			{
				_lastPlayerlistRequest += global::UnityEngine.Time.deltaTime;
			}
			if (_gdDirty)
			{
				_gdDirty = false;
				if (global::Mirror.NetworkServer.active)
				{
					TargetSyncGameplayData(base.connectionToClient, _gameplayData);
				}
			}
		}

		[global::Mirror.Command(channel = 4)]
		public void CmdRequestSalt(byte[] clSalt)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, clSalt);
			SendCommandInternal(typeof(global::RemoteAdmin.QueryProcessor), "CmdRequestSalt", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.TargetRpc]
		public void TargetSaltGenerated(global::Mirror.NetworkConnection conn, byte[] salt)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, salt);
			SendTargetRPCInternal(conn, typeof(global::RemoteAdmin.QueryProcessor), "TargetSaltGenerated", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		internal void SyncCommandsToClient()
		{
			if (!_commandsSynced)
			{
				_commandsSynced = true;
				TargetUpdateCommandList(_commands);
			}
		}

		[global::Mirror.Server]
		private static global::RemoteAdmin.QueryProcessor.CommandData[] ParseCommandsToStruct(global::System.Collections.Generic.List<global::CommandSystem.ICommand> list)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'RemoteAdmin.QueryProcessor/CommandData[] RemoteAdmin.QueryProcessor::ParseCommandsToStruct(System.Collections.Generic.List`1<CommandSystem.ICommand>)' called when server was not active");
				return null;
			}
			global::System.Collections.Generic.List<global::RemoteAdmin.QueryProcessor.CommandData> list2 = new global::System.Collections.Generic.List<global::RemoteAdmin.QueryProcessor.CommandData>();
			foreach (global::CommandSystem.ICommand item2 in list)
			{
				string text = item2.Description;
				if (string.IsNullOrWhiteSpace(text))
				{
					text = null;
				}
				else if (text.Length > 80)
				{
					text = text.Substring(0, 80) + "...";
				}
				global::RemoteAdmin.QueryProcessor.CommandData item = new global::RemoteAdmin.QueryProcessor.CommandData
				{
					Command = item2.Command,
					Usage = ((item2 is global::CommandSystem.IUsageProvider usageProvider) ? usageProvider.Usage : null),
					Description = text,
					AliasOf = null,
					Hidden = (item2 is global::CommandSystem.IHiddenCommand)
				};
				list2.Add(item);
				if (item2.Aliases != null)
				{
					string[] aliases = item2.Aliases;
					foreach (string command in aliases)
					{
						list2.Add(new global::RemoteAdmin.QueryProcessor.CommandData
						{
							Command = command,
							Usage = null,
							Description = null,
							AliasOf = item.Command,
							Hidden = item.Hidden
						});
					}
				}
			}
			return list2.ToArray();
		}

		[global::Mirror.TargetRpc]
		private void TargetUpdateCommandList(global::RemoteAdmin.QueryProcessor.CommandData[] commands)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.GeneratedNetworkCode._Write_RemoteAdmin_002EQueryProcessor_002FCommandData_005B_005D(writer, commands);
			SendTargetRPCInternal(null, typeof(global::RemoteAdmin.QueryProcessor), "TargetUpdateCommandList", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command(channel = 4)]
		public void CmdSendPassword(byte[] authSignature)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, authSignature);
			SendCommandInternal(typeof(global::RemoteAdmin.QueryProcessor), "CmdSendPassword", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.TargetRpc]
		private void TargetReplyPassword(global::Mirror.NetworkConnection conn, bool b)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, b);
			SendTargetRPCInternal(conn, typeof(global::RemoteAdmin.QueryProcessor), "TargetReplyPassword", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.TargetRpc]
		internal void TargetAdminChatAccessDenied(global::Mirror.NetworkConnection conn)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			SendTargetRPCInternal(conn, typeof(global::RemoteAdmin.QueryProcessor), "TargetAdminChatAccessDenied", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Server]
		internal void TargetReply(global::Mirror.NetworkConnection conn, string content, bool isSuccess, bool logInConsole, string overrideDisplay)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void RemoteAdmin.QueryProcessor::TargetReply(Mirror.NetworkConnection,System.String,System.Boolean,System.Boolean,System.String)' called when server was not active");
			}
			else if (CryptoManager.EncryptionKey == null)
			{
				if (ServerStatic.IsDedicated && base.isLocalPlayer)
				{
					ServerConsole.AddLog("[RA output] " + content);
				}
				else if (!CryptoManager.ExchangeRequested)
				{
					TargetReplyPlain(conn, content, isSuccess, logInConsole, overrideDisplay);
				}
				else
				{
					TargetReplyPlain(conn, "ERROR#ECDHE exchange was requested, please complete it on client side.", isSuccess: false, logInConsole: true, "");
				}
			}
			else
			{
				TargetReplyEncrypted(conn, global::Cryptography.AES.AesGcmEncrypt(Utf8.GetBytes(content), CryptoManager.EncryptionKey, _secureRandom), isSuccess, logInConsole, overrideDisplay);
			}
		}

		[global::Mirror.TargetRpc]
		private void TargetReplyPlain(global::Mirror.NetworkConnection conn, string content, bool isSuccess, bool logInConsole, string overrideDisplay)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteString(writer, content);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, isSuccess);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, logInConsole);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, overrideDisplay);
			SendTargetRPCInternal(conn, typeof(global::RemoteAdmin.QueryProcessor), "TargetReplyPlain", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.TargetRpc]
		private void TargetReplyEncrypted(global::Mirror.NetworkConnection conn, byte[] content, bool isSuccess, bool logInConsole, string overrideDisplay)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, content);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, isSuccess);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, logInConsole);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, overrideDisplay);
			SendTargetRPCInternal(conn, typeof(global::RemoteAdmin.QueryProcessor), "TargetReplyEncrypted", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command(channel = 4)]
		public void CmdSendEncryptedQuery(byte[] query)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, query);
			SendCommandInternal(typeof(global::RemoteAdmin.QueryProcessor), "CmdSendEncryptedQuery", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command(channel = 4)]
		public void CmdSendQuery(string query, int counter, byte[] signature)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteString(writer, query);
			global::Mirror.NetworkWriterExtensions.WriteInt32(writer, counter);
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, signature);
			SendCommandInternal(typeof(global::RemoteAdmin.QueryProcessor), "CmdSendQuery", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		internal void ProcessGameConsoleQuery(string query)
		{
			string[] array = query.Trim().Split(SpaceArray, 512, global::System.StringSplitOptions.RemoveEmptyEntries);
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerGameConsoleCommand, _hub, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1))))
			{
				return;
			}
			if (DotCommandHandler.TryGetCommand(array[0], out var command))
			{
				try
				{
					string response;
					bool flag = command.Execute(array.Segment(1), _sender, out response);
					if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerGameConsoleCommandExecuted, _hub, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1)), flag, response))
					{
						GCT.SendToClient(base.connectionToClient, array[0].ToUpperInvariant() + "#" + response, "");
					}
					return;
				}
				catch (global::System.Exception ex)
				{
					string text = "Command execution failed! Error: " + ex;
					if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerGameConsoleCommandExecuted, _hub, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1)), false, text))
					{
						GCT.SendToClient(base.connectionToClient, array[0].ToUpperInvariant() + "#" + text, "");
					}
					return;
				}
			}
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerGameConsoleCommandExecuted, _hub, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1)), false, "Command not found."))
			{
				GCT.SendToClient(base.connectionToClient, "Command not found.", "red");
			}
		}

		private bool VerifyRequestSignature(string message, int counter, byte[] signature, bool validateCounter = true)
		{
			if (!_roles.PublicKeyAccepted && _roles.RemoteAdminMode == ServerRoles.AccessMode.PasswordOverride)
			{
				return VerifyHmacSignature(message, counter, signature, validateCounter);
			}
			return VerifyEcdsaSignature(message, counter, signature, validateCounter);
		}

		private bool VerifyHmacSignature(string message, int counter, byte[] signature, bool validateCounter = true)
		{
			if (counter <= _signaturesCounter)
			{
				if (validateCounter)
				{
					return false;
				}
			}
			else
			{
				_signaturesCounter = counter;
			}
			if (!OverridePasswordEnabled)
			{
				return false;
			}
			string text = message + ":[:COUNTER:]:" + counter + ":[:SALT:]:" + ServerRandom;
			byte[] array = global::System.Buffers.ArrayPool<byte>.Shared.Rent(global::System.Text.Encoding.UTF8.GetMaxByteCount(text.Length));
			int bytes = Utf8.GetBytes(text, array);
			bool result = global::System.Linq.Enumerable.SequenceEqual(global::Cryptography.Sha.Sha512Hmac(array, 0, bytes, _key), signature);
			global::System.Buffers.ArrayPool<byte>.Shared.Return(array);
			return result;
		}

		private bool VerifyEcdsaSignature(string message, int counter, byte[] signature, bool validateCounter = true)
		{
			if (!_roles.PublicKeyAccepted || _roles.PublicKey == null)
			{
				global::GameCore.Console.AddLog("VerifyEcdsaSignature called with empty Public Key", global::UnityEngine.Color.red);
				global::UnityEngine.Debug.LogError("VerifyEcdsaSignature called with empty Public Key");
				return false;
			}
			if (counter <= _signaturesCounter)
			{
				if (validateCounter)
				{
					return false;
				}
			}
			else
			{
				_signaturesCounter = counter;
			}
			return global::Cryptography.ECDSA.VerifyBytes(message + ":[:COUNTER:]:" + counter + ":[:SALT:]:" + ServerRandom, signature, _roles.PublicKey);
		}

		public static byte[] DerivePassword(string password, byte[] serversalt, byte[] clientsalt)
		{
			byte[] salt = global::Cryptography.Sha.Sha512(global::System.Convert.ToBase64String(serversalt) + global::System.Convert.ToBase64String(clientsalt));
			return global::Cryptography.PBKDF2.Pbkdf2HashBytes(password, salt, 250, 512);
		}

		[global::Mirror.TargetRpc]
		public void TargetSyncGameplayData(global::Mirror.NetworkConnection conn, bool gd)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, gd);
			SendTargetRPCInternal(conn, typeof(global::RemoteAdmin.QueryProcessor), "TargetSyncGameplayData", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void OnDestroy()
		{
			if (global::Mirror.NetworkServer.active && (!base.isLocalPlayer || !ServerStatic.IsDedicated))
			{
				ServerLogs.AddLog(ServerLogs.Modules.Networking, string.Format("{0} {1} disconnected from IP address {2}. Last class: {3} ({4})", GetComponent<NicknameSync>().CombinedName, string.IsNullOrEmpty(_hub.characterClassManager.UserId) ? "(no ID)" : _hub.characterClassManager.UserId, _ipAddress, global::PlayerRoles.PlayerRolesUtils.GetRoleId(_hub), _hub.roleManager.CurrentRole.RoleName), ServerLogs.ServerLogType.ConnectionUpdate);
			}
		}

		static QueryProcessor()
		{
			SpaceArray = new char[1] { ' ' };
			DotCommandHandler = global::CommandSystem.ClientCommandHandler.Create();
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::RemoteAdmin.QueryProcessor), "CmdRequestSalt", InvokeUserCode_CmdRequestSalt, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::RemoteAdmin.QueryProcessor), "CmdSendPassword", InvokeUserCode_CmdSendPassword, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::RemoteAdmin.QueryProcessor), "CmdSendEncryptedQuery", InvokeUserCode_CmdSendEncryptedQuery, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::RemoteAdmin.QueryProcessor), "CmdSendQuery", InvokeUserCode_CmdSendQuery, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.QueryProcessor), "TargetSaltGenerated", InvokeUserCode_TargetSaltGenerated);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.QueryProcessor), "TargetUpdateCommandList", InvokeUserCode_TargetUpdateCommandList);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.QueryProcessor), "TargetReplyPassword", InvokeUserCode_TargetReplyPassword);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.QueryProcessor), "TargetAdminChatAccessDenied", InvokeUserCode_TargetAdminChatAccessDenied);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.QueryProcessor), "TargetReplyPlain", InvokeUserCode_TargetReplyPlain);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.QueryProcessor), "TargetReplyEncrypted", InvokeUserCode_TargetReplyEncrypted);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.QueryProcessor), "TargetSyncGameplayData", InvokeUserCode_TargetSyncGameplayData);
		}

		private void MirrorProcessed()
		{
		}

		public void UserCode_CmdRequestSalt(byte[] clSalt)
		{
			if (!_commandRateLimit.CanExecute())
			{
				return;
			}
			if (!ServerStatic.PermissionsHandler.OverrideEnabled)
			{
				_hub.gameConsoleTransmission.SendToClient("Password authentication is disabled on this server!", "magenta");
				return;
			}
			if (_clientSalt == null)
			{
				if (clSalt == null)
				{
					_hub.gameConsoleTransmission.SendToClient("Please generate and send your salt!", "red");
					return;
				}
				if (clSalt.Length < 32)
				{
					_hub.gameConsoleTransmission.SendToClient("Generated salt is too short. Please generate longer salt and try again!", "red");
					return;
				}
				_clientSalt = clSalt;
				if (_key == null && _salt != null)
				{
					_key = ServerStatic.PermissionsHandler.DerivePassword(_salt, _clientSalt);
				}
				_hub.gameConsoleTransmission.SendToClient("Your salt " + global::System.Convert.ToBase64String(clSalt) + " has been accepted by the server.", "cyan");
			}
			if (_salt != null)
			{
				TargetSaltGenerated(base.connectionToClient, _salt);
				return;
			}
			byte[] array;
			using (global::System.Security.Cryptography.RandomNumberGenerator randomNumberGenerator = new global::System.Security.Cryptography.RNGCryptoServiceProvider())
			{
				array = new byte[32];
				randomNumberGenerator.GetBytes(array);
			}
			_salt = array;
			_key = ServerStatic.PermissionsHandler.DerivePassword(_salt, _clientSalt);
			TargetSaltGenerated(base.connectionToClient, _salt);
		}

		protected static void InvokeUserCode_CmdRequestSalt(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdRequestSalt called on client.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_CmdRequestSalt(global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader));
			}
		}

		public void UserCode_TargetSaltGenerated(global::Mirror.NetworkConnection conn, byte[] salt)
		{
		}

		protected static void InvokeUserCode_TargetSaltGenerated(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetSaltGenerated called on server.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_TargetSaltGenerated(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader));
			}
		}

		private void UserCode_TargetUpdateCommandList(global::RemoteAdmin.QueryProcessor.CommandData[] commands)
		{
		}

		protected static void InvokeUserCode_TargetUpdateCommandList(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetUpdateCommandList called on server.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_TargetUpdateCommandList(global::Mirror.GeneratedNetworkCode._Read_RemoteAdmin_002EQueryProcessor_002FCommandData_005B_005D(reader));
			}
		}

		public void UserCode_CmdSendPassword(byte[] authSignature)
		{
			if (!_commandRateLimit.CanExecute())
			{
				return;
			}
			bool b = false;
			if (_roles.RemoteAdmin)
			{
				b = true;
				PasswordTries = 0;
			}
			else
			{
				if (_salt == null || _clientSalt == null)
				{
					_hub.gameConsoleTransmission.SendToClient("Can't verify your remote admin password - please generate salt first!", "red");
					return;
				}
				if (_clientSalt.Length < 16)
				{
					_hub.gameConsoleTransmission.SendToClient("Generated salt is too short. Please rejoin the server and try again!", "red");
					return;
				}
				if (VerifyHmacSignature("Login", -1, authSignature, validateCounter: false))
				{
					PasswordTries = 0;
					UserGroup overrideGroup = ServerStatic.PermissionsHandler.OverrideGroup;
					if (overrideGroup != null)
					{
						ServerConsole.AddLog("Assigned group " + overrideGroup.BadgeText + " to " + _hub.nicknameSync.CombinedName + " - override password.");
						_roles.SetGroup(overrideGroup, ovr: true);
						b = true;
					}
					else
					{
						_hub.gameConsoleTransmission.SendToClient("Non-existing group is assigned for override password!", "red");
					}
				}
				else
				{
					PasswordTries++;
					ServerConsole.AddLog("Rejected override password sent by " + _hub.LoggedNameFromRefHub() + ".");
					ServerLogs.AddLog(ServerLogs.Modules.Permissions, "Rejected override password sent by " + _hub.LoggedNameFromRefHub() + ".", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
				}
			}
			if (PasswordTries >= 3)
			{
				ServerLogs.AddLog(ServerLogs.Modules.Permissions, _hub.LoggedNameFromRefHub() + " has been kicked from the server for sending too many invalid override passwords.", ServerLogs.ServerLogType.RemoteAdminActivity_Misc);
				ServerConsole.Disconnect(base.connectionToClient, "You have been kicked for too many Remote Admin login attempts.");
			}
			else
			{
				TargetReplyPassword(base.connectionToClient, b);
			}
		}

		protected static void InvokeUserCode_CmdSendPassword(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdSendPassword called on client.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_CmdSendPassword(global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader));
			}
		}

		private void UserCode_TargetReplyPassword(global::Mirror.NetworkConnection conn, bool b)
		{
		}

		protected static void InvokeUserCode_TargetReplyPassword(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetReplyPassword called on server.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_TargetReplyPassword(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}

		internal void UserCode_TargetAdminChatAccessDenied(global::Mirror.NetworkConnection conn)
		{
		}

		protected static void InvokeUserCode_TargetAdminChatAccessDenied(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetAdminChatAccessDenied called on server.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_TargetAdminChatAccessDenied(global::Mirror.NetworkClient.readyConnection);
			}
		}

		private void UserCode_TargetReplyPlain(global::Mirror.NetworkConnection conn, string content, bool isSuccess, bool logInConsole, string overrideDisplay)
		{
		}

		protected static void InvokeUserCode_TargetReplyPlain(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetReplyPlain called on server.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_TargetReplyPlain(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadString(reader));
			}
		}

		private void UserCode_TargetReplyEncrypted(global::Mirror.NetworkConnection conn, byte[] content, bool isSuccess, bool logInConsole, string overrideDisplay)
		{
		}

		protected static void InvokeUserCode_TargetReplyEncrypted(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetReplyEncrypted called on server.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_TargetReplyEncrypted(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadString(reader));
			}
		}

		public void UserCode_CmdSendEncryptedQuery(byte[] query)
		{
			if (!_commandRateLimit.CanExecute() || query == null)
			{
				return;
			}
			if (!_roles.RemoteAdmin)
			{
				GCT.SendToClient(base.connectionToClient, "You are not logged in to remote admin panel!", "red");
				return;
			}
			if (CryptoManager.EncryptionKey == null)
			{
				GCT.SendToClient(base.connectionToClient, "Please complete ECDHE exchange before sending encrypted remote admin requests.", "magenta");
				return;
			}
			string text;
			try
			{
				text = Utf8.GetString(global::Cryptography.AES.AesGcmDecrypt(query, CryptoManager.EncryptionKey));
			}
			catch
			{
				GCT.SendToClient(base.connectionToClient, "Decryption or verification of remote admin request failed.", "magenta");
				return;
			}
			if (!text.Contains(":[:COUNTER:]:"))
			{
				GCT.SendToClient(base.connectionToClient, "Remote admin request doesn't contain a signatures counter.", "magenta");
				return;
			}
			int num = text.LastIndexOf(":[:COUNTER:]:", global::System.StringComparison.Ordinal);
			if (!int.TryParse(text.Substring(num + 13), out var result))
			{
				GCT.SendToClient(base.connectionToClient, "Remote admin request contains non-integer signatures counter.", "magenta");
				return;
			}
			if (result <= _signaturesCounter)
			{
				GCT.SendToClient(base.connectionToClient, "Remote admin request contains smaller signatures counter than previous request.", "magenta");
				return;
			}
			_signaturesCounter = result;
			global::RemoteAdmin.CommandProcessor.ProcessQuery(text.Substring(0, num), _sender);
		}

		protected static void InvokeUserCode_CmdSendEncryptedQuery(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdSendEncryptedQuery called on client.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_CmdSendEncryptedQuery(global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader));
			}
		}

		public void UserCode_CmdSendQuery(string query, int counter, byte[] signature)
		{
			if (_commandRateLimit.CanExecute() && query != null && signature != null)
			{
				if (string.IsNullOrEmpty(ServerRandom))
				{
					GCT.SendToClient(base.connectionToClient, "Remote Admin error - ServerRandom is empty or null.", "magenta");
				}
				else if (!_roles.RemoteAdmin)
				{
					GCT.SendToClient(base.connectionToClient, "You are not logged in to remote admin panel!", "red");
				}
				else if (CryptoManager.ExchangeRequested)
				{
					GCT.SendToClient(base.connectionToClient, "ECDHE exchange was requested, please use encrypted channel for remote admin commands.", "magenta");
				}
				else if (!VerifyRequestSignature(query, counter, signature))
				{
					GCT.SendToClient(base.connectionToClient, "Signature verification of request \"" + query + "\" failed!", "magenta");
				}
				else
				{
					global::RemoteAdmin.CommandProcessor.ProcessQuery(query, _sender);
				}
			}
		}

		protected static void InvokeUserCode_CmdSendQuery(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdSendQuery called on client.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_CmdSendQuery(global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadInt32(reader), global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader));
			}
		}

		public void UserCode_TargetSyncGameplayData(global::Mirror.NetworkConnection conn, bool gd)
		{
			_gameplayData = gd;
		}

		protected static void InvokeUserCode_TargetSyncGameplayData(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetSyncGameplayData called on server.");
			}
			else
			{
				((global::RemoteAdmin.QueryProcessor)obj).UserCode_TargetSyncGameplayData(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteString(writer, _syncServerRandom);
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, OverridePasswordEnabled);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteString(writer, _syncServerRandom);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, OverridePasswordEnabled);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				string syncServerRandom = _syncServerRandom;
				Network_syncServerRandom = global::Mirror.NetworkReaderExtensions.ReadString(reader);
				if (!SyncVarEqual(syncServerRandom, ref _syncServerRandom))
				{
					SetServerRandom(syncServerRandom, _syncServerRandom);
				}
				bool overridePasswordEnabled = OverridePasswordEnabled;
				NetworkOverridePasswordEnabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				string syncServerRandom2 = _syncServerRandom;
				Network_syncServerRandom = global::Mirror.NetworkReaderExtensions.ReadString(reader);
				if (!SyncVarEqual(syncServerRandom2, ref _syncServerRandom))
				{
					SetServerRandom(syncServerRandom2, _syncServerRandom);
				}
			}
			if ((num & 2L) != 0L)
			{
				bool overridePasswordEnabled2 = OverridePasswordEnabled;
				NetworkOverridePasswordEnabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			}
		}
	}
}
