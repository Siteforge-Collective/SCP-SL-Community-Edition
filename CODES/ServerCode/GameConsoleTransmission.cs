public class GameConsoleTransmission : global::Mirror.NetworkBehaviour
{
	public global::RemoteAdmin.RemoteAdminCryptographicManager CryptoManager;

	public global::RemoteAdmin.QueryProcessor Processor;

	private static global::Org.BouncyCastle.Security.SecureRandom _secureRandom;

	private global::Security.RateLimit _cmdRateLimit;

	private void Start()
	{
		ReferenceHub hub = ReferenceHub.GetHub(base.gameObject);
		_cmdRateLimit = hub.playerRateLimitHandler.RateLimits[2];
		CryptoManager = GetComponent<global::RemoteAdmin.RemoteAdminCryptographicManager>();
		Processor = hub.queryProcessor;
		if (_secureRandom == null)
		{
			_secureRandom = new global::Org.BouncyCastle.Security.SecureRandom();
		}
	}

	[global::Mirror.Server]
	public void SendToClient(string text, string color)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameConsoleTransmission::SendToClient(System.String,System.String)' called when server was not active");
		}
		else
		{
			SendToClient(base.connectionToClient, text, color);
		}
	}

	[global::Mirror.Server]
	public void SendToClient(global::Mirror.NetworkConnection connection, string text, string color)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameConsoleTransmission::SendToClient(Mirror.NetworkConnection,System.String,System.String)' called when server was not active");
			return;
		}
		byte[] bytes = Utf8.GetBytes(color + "#" + text);
		if (CryptoManager.EncryptionKey == null)
		{
			TargetPrintOnConsole(connection, bytes, encrypted: false);
		}
		else
		{
			TargetPrintOnConsole(connection, global::Cryptography.AES.AesGcmEncrypt(bytes, CryptoManager.EncryptionKey, _secureRandom), encrypted: true);
		}
	}

	[global::Mirror.TargetRpc]
	public void TargetPrintOnConsole(global::Mirror.NetworkConnection connection, byte[] data, bool encrypted)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, data);
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, encrypted);
		SendTargetRPCInternal(connection, typeof(GameConsoleTransmission), "TargetPrintOnConsole", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Client]
	public void SendToServer(string command)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogWarning("[Client] function 'System.Void GameConsoleTransmission::SendToServer(System.String)' called when client was not active");
			return;
		}
		byte[] bytes = Utf8.GetBytes(command);
		if (CryptoManager.EncryptionKey == null)
		{
			CmdCommandToServer(bytes, encrypted: false);
		}
		else
		{
			CmdCommandToServer(global::Cryptography.AES.AesGcmEncrypt(bytes, CryptoManager.EncryptionKey, _secureRandom), encrypted: true);
		}
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdCommandToServer(byte[] data, bool encrypted)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, data);
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, encrypted);
		SendCommandInternal(typeof(GameConsoleTransmission), "CmdCommandToServer", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private global::UnityEngine.Color ProcessColor(string name)
	{
		switch (name)
		{
		case "red":
			return global::UnityEngine.Color.red;
		case "cyan":
			return global::UnityEngine.Color.cyan;
		case "blue":
			return global::UnityEngine.Color.blue;
		case "magenta":
			return global::UnityEngine.Color.magenta;
		case "white":
			return global::UnityEngine.Color.white;
		case "green":
			return global::UnityEngine.Color.green;
		case "yellow":
			return global::UnityEngine.Color.yellow;
		case "black":
			return global::UnityEngine.Color.black;
		default:
			return global::UnityEngine.Color.grey;
		}
	}

	private void MirrorProcessed()
	{
	}

	public void UserCode_TargetPrintOnConsole(global::Mirror.NetworkConnection connection, byte[] data, bool encrypted)
	{
		if (data == null)
		{
			return;
		}
		string empty = string.Empty;
		if (!encrypted)
		{
			empty = Utf8.GetString(data);
		}
		else
		{
			if (CryptoManager.EncryptionKey == null)
			{
				global::GameCore.Console.AddLog("Can't process encrypted message from server before completing ECDHE exchange.", global::UnityEngine.Color.magenta);
				return;
			}
			try
			{
				empty = Utf8.GetString(global::Cryptography.AES.AesGcmDecrypt(data, CryptoManager.EncryptionKey));
			}
			catch
			{
				SendToClient("Decryption or verification of encrypted message failed.", "magenta");
				return;
			}
		}
		string text = empty.Remove(empty.IndexOf("#", global::System.StringComparison.Ordinal));
		empty = empty.Remove(0, empty.IndexOf("#", global::System.StringComparison.Ordinal) + 1);
		global::GameCore.Console.AddLog((encrypted ? "[FROM SERVER] " : "[UNENCRYPTED FROM SERVER] ") + empty, ProcessColor(text));
	}

	protected static void InvokeUserCode_TargetPrintOnConsole(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetPrintOnConsole called on server.");
		}
		else
		{
			((GameConsoleTransmission)obj).UserCode_TargetPrintOnConsole(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	public void UserCode_CmdCommandToServer(byte[] data, bool encrypted)
	{
		if (!_cmdRateLimit.CanExecute() || data == null)
		{
			return;
		}
		string empty = string.Empty;
		if (!encrypted)
		{
			if (CryptoManager.EncryptionKey != null || CryptoManager.ExchangeRequested)
			{
				SendToClient(base.connectionToClient, "Please use encrypted connection to send commands.", "magenta");
				return;
			}
			empty = Utf8.GetString(data);
		}
		else
		{
			if (CryptoManager.EncryptionKey == null)
			{
				SendToClient(base.connectionToClient, "Can't process encrypted message from server before completing ECDHE exchange.", "magenta");
				return;
			}
			try
			{
				empty = Utf8.GetString(global::Cryptography.AES.AesGcmDecrypt(data, CryptoManager.EncryptionKey));
			}
			catch
			{
				SendToClient(base.connectionToClient, "Decryption or verification of encrypted message failed.", "magenta");
				return;
			}
		}
		Processor.ProcessGameConsoleQuery(empty);
	}

	protected static void InvokeUserCode_CmdCommandToServer(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdCommandToServer called on client.");
		}
		else
		{
			((GameConsoleTransmission)obj).UserCode_CmdCommandToServer(global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	static GameConsoleTransmission()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(GameConsoleTransmission), "CmdCommandToServer", InvokeUserCode_CmdCommandToServer, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(GameConsoleTransmission), "TargetPrintOnConsole", InvokeUserCode_TargetPrintOnConsole);
	}
}
