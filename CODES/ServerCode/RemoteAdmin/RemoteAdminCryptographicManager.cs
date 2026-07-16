namespace RemoteAdmin
{
	public class RemoteAdminCryptographicManager : global::Mirror.NetworkBehaviour
	{
		private global::Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair _ecdhKeys;

		private global::Org.BouncyCastle.Crypto.Agreement.ECDHBasicAgreement _exchange;

		private byte[] _ecdhPublicKeySignature;

		internal bool ExchangeRequested;

		internal byte[] EncryptionKey;

		private void Init()
		{
			_ecdhKeys = global::Cryptography.ECDH.GenerateKeys();
			_exchange = global::Cryptography.ECDH.Init(_ecdhKeys);
			ExchangeRequested = true;
		}

		[global::Mirror.Server]
		public void StartExchange()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void RemoteAdmin.RemoteAdminCryptographicManager::StartExchange()' called when server was not active");
				return;
			}
			if (_exchange == null || _ecdhKeys == null)
			{
				Init();
			}
			TargetDiffieHellmanExchange(base.connectionToClient, global::Cryptography.ECDSA.KeyToString(_ecdhKeys.Public));
		}

		[global::Mirror.TargetRpc]
		private void TargetDiffieHellmanExchange(global::Mirror.NetworkConnection conn, string publicKey)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteString(writer, publicKey);
			SendTargetRPCInternal(conn, typeof(global::RemoteAdmin.RemoteAdminCryptographicManager), "TargetDiffieHellmanExchange", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Command(channel = 4)]
		private void CmdDiffieHellmanExchange(string publicKey, byte[] signature)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteString(writer, publicKey);
			global::Mirror.NetworkWriterExtensions.WriteBytesAndSize(writer, signature);
			SendCommandInternal(typeof(global::RemoteAdmin.RemoteAdminCryptographicManager), "CmdDiffieHellmanExchange", writer, 4);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_TargetDiffieHellmanExchange(global::Mirror.NetworkConnection conn, string publicKey)
		{
		}

		protected static void InvokeUserCode_TargetDiffieHellmanExchange(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetDiffieHellmanExchange called on server.");
			}
			else
			{
				((global::RemoteAdmin.RemoteAdminCryptographicManager)obj).UserCode_TargetDiffieHellmanExchange(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader));
			}
		}

		private void UserCode_CmdDiffieHellmanExchange(string publicKey, byte[] signature)
		{
			if (EncryptionKey != null || _exchange == null || _ecdhKeys == null)
			{
				return;
			}
			ReferenceHub hub = ReferenceHub.GetHub(base.gameObject);
			global::Org.BouncyCastle.Crypto.AsymmetricKeyParameter publicKey2 = hub.serverRoles.PublicKey;
			string authToken = hub.characterClassManager.AuthToken;
			if (CharacterClassManager.OnlineMode)
			{
				if (publicKey == null || authToken == null)
				{
					hub.gameConsoleTransmission.SendToClient("Please complete authentication before requesting ECDHE exchange.", "magenta");
					return;
				}
				if (publicKey2 != null && !global::Cryptography.ECDSA.VerifyBytes(publicKey, signature, publicKey2))
				{
					hub.gameConsoleTransmission.SendToClient("Exchange parameters signature is invalid!", "magenta");
					return;
				}
			}
			EncryptionKey = global::Cryptography.ECDH.DeriveKey(_exchange, global::Cryptography.ECDSA.PublicKeyFromString(publicKey));
		}

		protected static void InvokeUserCode_CmdDiffieHellmanExchange(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogError("Command CmdDiffieHellmanExchange called on client.");
			}
			else
			{
				((global::RemoteAdmin.RemoteAdminCryptographicManager)obj).UserCode_CmdDiffieHellmanExchange(global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadBytesAndSize(reader));
			}
		}

		static RemoteAdminCryptographicManager()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(global::RemoteAdmin.RemoteAdminCryptographicManager), "CmdDiffieHellmanExchange", InvokeUserCode_CmdDiffieHellmanExchange, requiresAuthority: true);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::RemoteAdmin.RemoteAdminCryptographicManager), "TargetDiffieHellmanExchange", InvokeUserCode_TargetDiffieHellmanExchange);
		}
	}
}
