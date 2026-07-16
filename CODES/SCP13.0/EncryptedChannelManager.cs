using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CentralAuth;
using Cryptography;
using GameCore;
using Mirror;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Security;
using UnityEngine;

public class EncryptedChannelManager : NetworkBehaviour
{
    public delegate void EncryptedMessageHandler(string message, SecurityLevel level);

    public delegate void EncryptedMessageServerHandler(ReferenceHub hub, string message, SecurityLevel level);

    public enum EncryptedChannel : byte
    {
        RemoteAdmin = 0,
        GameConsole = 1,
        AdminChat = 2
    }

    public enum SecurityLevel : byte
    {
        Unsecured = 0,
        EncryptedAndAuthenticated = 1
    }

    private readonly struct EncryptedMessage
    {
        internal readonly EncryptedChannel Channel;

        internal readonly string Content;

        internal readonly uint Counter;

        internal int GetLength => Misc.Utf8Encoding.GetByteCount(Content) + 5;

        internal EncryptedMessage(EncryptedChannel channel, string content, uint counter)
        {
            Channel = channel;
            Content = content;
            Counter = counter;
        }

        internal void Serialize(byte[] array)
        {
            uint value = (BitConverter.IsLittleEndian ? Counter : BinaryPrimitives.ReverseEndianness(Counter));
            array[0] = (byte)Channel;
            BitConverter.GetBytes(value).CopyTo(array, 1);
            Misc.Utf8Encoding.GetBytes(Content, 0, Content.Length, array, 5);
        }

        internal static EncryptedMessage Deserialize(byte[] array)
        {
            uint num = BitConverter.ToUInt32(array, 1);
            if (!BitConverter.IsLittleEndian)
            {
                num = BinaryPrimitives.ReverseEndianness(num);
            }
            return new EncryptedMessage((EncryptedChannel)array[0], Misc.Utf8Encoding.GetString(array, 5, array.Length - 5), num);
        }
    }

    public readonly struct EncryptedMessageOutside : NetworkMessage
    {
        internal readonly SecurityLevel Level;

        internal readonly byte[] Data;

        internal EncryptedMessageOutside(SecurityLevel level, byte[] data)
        {
            Level = level;
            Data = data;
        }
    }

    internal byte[] EncryptionKey;

    internal static bool CryptographyDebug;

    [SyncVar(hook = nameof(ReceivedSaltHook))]
    [SerializeField]
    [HideInInspector]
    internal string ServerRandom;

    private uint _txCounter;

    private uint _rxCounter;

    private ECDHBasicAgreement _exchange;

    private static readonly SecureRandom SecureRandom = new SecureRandom();

    private static readonly Dictionary<EncryptedChannel, EncryptedMessageHandler> ClientChannelHandlers = new Dictionary<EncryptedChannel, EncryptedMessageHandler>();

    private static readonly Dictionary<EncryptedChannel, EncryptedMessageServerHandler> ServerChannelHandlers = new Dictionary<EncryptedChannel, EncryptedMessageServerHandler>();

    private static readonly Dictionary<EncryptedChannel, SecurityLevel> RequiredSecurityLevels = new Dictionary<EncryptedChannel, SecurityLevel>
    {
        {
            EncryptedChannel.RemoteAdmin,
            SecurityLevel.EncryptedAndAuthenticated
        },
        {
            EncryptedChannel.GameConsole,
            SecurityLevel.Unsecured
        },
        {
            EncryptedChannel.AdminChat,
            SecurityLevel.EncryptedAndAuthenticated
        }
    };

    internal AsymmetricCipherKeyPair EcdhKeys { get; private set; }

    internal void PrepareExchange()
    {
        if (_exchange == null)
        {
            EcdhKeys = ECDH.GenerateKeys();
            _exchange = ECDH.Init(EcdhKeys);
        }
    }

    internal void ServerProcessExchange(string publicKey)
    {
        if (EncryptionKey == null)
        {
            if (CryptographyDebug)
            {
                ServerConsole.AddLog("Received ECDH parameters from " + (ReferenceHub.TryGetHub(base.gameObject, out var hub) ? hub.LoggedNameFromRefHub() : "(unknown)") + ".", ConsoleColor.Gray);
            }
            EncryptionKey = ECDH.DeriveKey(_exchange, ECDSA.PublicKeyFromString(publicKey));
            if (CryptographyDebug)
            {
                ServerConsole.AddLog("ECDH key exchange successful. Derived encryption key.", ConsoleColor.Green);
            }
            _exchange = null;
            ResetCounters();
        }
    }

    internal void ClientProcessExchange(string publicKey, AsymmetricKeyParameter clientPublicKey, byte[] signature)
    {
        if (EncryptionKey == null)
        {
            if (CryptographyDebug)
            {
                GameCore.Console.AddLog("Received ECDH parameters from server.", UnityEngine.Color.gray, false, GameCore.Console.ConsoleLogType.Log);
            }
            if (signature != null && clientPublicKey != null)
            {
                if (!ECDSA.VerifyBytes(publicKey, signature, clientPublicKey))
                {
                    GameCore.Console.AddLog("Invalid ECDH signature from server.", UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
                    return;
                }
            }
            EncryptionKey = ECDH.DeriveKey(_exchange, ECDSA.PublicKeyFromString(publicKey));
            if (CryptographyDebug)
            {
                GameCore.Console.AddLog("ECDH key exchange successful. Derived encryption key.", UnityEngine.Color.green, false, GameCore.Console.ConsoleLogType.Log);
            }
            _exchange = null;
            ResetCounters();
        }
    }

    private void ResetCounters()
    {
        _txCounter = 0;
        _rxCounter = 0;
    }

    public override void OnStartLocalPlayer()
    {
        cachedClientReceiveEncryptedMessage ??= ClientReceiveEncryptedMessage;
        NetworkClient.ReplaceHandler<EncryptedMessageOutside>(cachedClientReceiveEncryptedMessage, true);
    }

    public override void OnStartServer()
    {
        ServerRandom = RandomGenerator.GetStringSecure(32);
        cachedServerReceiveEncryptedMessage ??= ServerReceiveEncryptedMessage;
        NetworkServer.ReplaceHandler<EncryptedMessageOutside>(cachedServerReceiveEncryptedMessage, true);
    }


    private static Action<NetworkConnectionToClient, EncryptedMessageOutside> cachedServerReceiveEncryptedMessage;

    public static void ServerReceiveEncryptedMessage(NetworkConnectionToClient conn, EncryptedMessageOutside msg)
    {
        if (!NetworkServer.active)
        {
            return;
        }
        if (ReferenceHub.TryGetHub(conn.connectionId, out ReferenceHub hub))
        {
            hub.encryptedChannelManager.ServerProcessEncryptedMessage(msg);
        }
    }

    private void ServerProcessEncryptedMessage(EncryptedMessageOutside msg)
    {
        if (TryUnpack(msg, out EncryptedMessage unpacked, out SecurityLevel level, base.isLocalPlayer))
        {
            if (CryptographyDebug)
            {
                ServerConsole.AddLog($"Received encrypted message on channel {unpacked.Channel} from {ReferenceHub.GetHub(gameObject).LoggedNameFromRefHub()}.", ConsoleColor.Gray);
            }
            if (ServerChannelHandlers.TryGetValue(unpacked.Channel, out EncryptedMessageServerHandler handler))
            {
                try
                {
                    handler?.Invoke(ReferenceHub.GetHub(gameObject), unpacked.Content, level);
                }
                catch (Exception ex)
                {
                    ServerConsole.AddLog($"Error handling encrypted message on channel {unpacked.Channel} (server). Exception: {ex.Message}", ConsoleColor.Red);
                    ServerConsole.AddLog(ex.StackTrace, ConsoleColor.Red);
                }
            }
            else
            {
                ServerConsole.AddLog($"No handler registered for channel {unpacked.Channel} on server.", ConsoleColor.Yellow);
            }
        }
    }

    private static Action<EncryptedMessageOutside> cachedClientReceiveEncryptedMessage;

    public static void ClientReceiveEncryptedMessage(EncryptedMessageOutside msg)
    {
        if (!NetworkClient.active)
        {
            return;
        }
        if (ReferenceHub.TryGetLocalHub(out ReferenceHub localHub))
        {
            localHub.encryptedChannelManager.ClientProcessEncryptedMessage(msg);
        }
    }

    private void ClientProcessEncryptedMessage(EncryptedMessageOutside msg)
    {
        if (TryUnpack(msg, out EncryptedMessage unpacked, out SecurityLevel level, true))
        {
            if (CryptographyDebug)
            {
                GameCore.Console.AddLog($"Received encrypted message on channel {unpacked.Channel} from server.", UnityEngine.Color.gray, false, GameCore.Console.ConsoleLogType.Log);
            }
            if (ClientChannelHandlers.TryGetValue(unpacked.Channel, out EncryptedMessageHandler handler))
            {
                try
                {
                    handler?.Invoke(unpacked.Content, level);
                }
                catch (Exception ex)
                {
                    GameCore.Console.AddLog($"Error handling encrypted message on channel {unpacked.Channel} (client). Exception: {ex.Message}", UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
                    GameCore.Console.AddLog(ex.StackTrace, UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
                }
            }
            else
            {
                GameCore.Console.AddLog($"No handler registered for channel {unpacked.Channel} on client.", UnityEngine.Color.yellow, false, GameCore.Console.ConsoleLogType.Warning);
            }
        }
    }

    public bool TrySendMessageToServer(string content, EncryptedChannel channel)
    {
        if (_txCounter == uint.MaxValue)
        {
            _txCounter = 0u;
        }
        if (!TryPack(new EncryptedMessage(channel, content, ++_txCounter), out var packed, NetworkServer.active))
        {
            return false;
        }
        NetworkClient.Send(packed);
        return true;
    }

    public static bool TrySendMessageToClient(ReferenceHub hub, string content, EncryptedChannel channel)
    {
        return hub.encryptedChannelManager.TrySendMessageToClient(content, channel);
    }

    [Server]
    public bool TrySendMessageToClient(string content, EncryptedChannel channel)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("[Server] function 'System.Boolean EncryptedChannelManager::TrySendMessageToClient(System.String,EncryptedChannelManager/EncryptedChannel)' called when server was not active");
            return false;
        }
        if (_txCounter == uint.MaxValue)
        {
            _txCounter = 0u;
        }
        if (!TryPack(new EncryptedMessage(channel, content, ++_txCounter), out var packed, base.isLocalPlayer))
        {
            return false;
        }
        base.connectionToClient.Send(packed);
        return true;
    }

    private bool TryPack(EncryptedMessage msg, out EncryptedMessageOutside packed, bool localClient = false)
    {
        bool flag = EncryptionKey != null && !localClient;
        if (!localClient && RequiredSecurityLevels[msg.Channel] == SecurityLevel.EncryptedAndAuthenticated && !flag)
        {
            GameCore.Console.AddLog($"Failed to send encrypted message to {msg.Channel} channel. Encryption key is not available.", UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
            packed = default(EncryptedMessageOutside);
            return false;
        }
        byte[] array = new byte[msg.GetLength];
        msg.Serialize(array);
        if (!flag)
        {
            packed = new EncryptedMessageOutside(SecurityLevel.Unsecured, array);
            return true;
        }
        packed = new EncryptedMessageOutside(SecurityLevel.EncryptedAndAuthenticated, AES.AesGcmEncrypt(array, EncryptionKey, SecureRandom));
        return true;
    }

    private bool TryUnpack(EncryptedMessageOutside packed, out EncryptedMessage msg, out SecurityLevel level, bool localClient)
    {
        level = packed.Level;
        bool flag = packed.Level == SecurityLevel.EncryptedAndAuthenticated;
        if (flag && EncryptionKey == null)
        {
            GameCore.Console.AddLog("Failed to decrypt received message. Encryption key is not available.", UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
            msg = default(EncryptedMessage);
            return false;
        }
        if (!flag)
        {
            msg = EncryptedMessage.Deserialize(packed.Data);
            if (!localClient && RequiredSecurityLevels[msg.Channel] == SecurityLevel.EncryptedAndAuthenticated)
            {
                GameCore.Console.AddLog($"Message on channel {msg.Channel} was sent without encryption. Discarding message!", UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
                msg = default(EncryptedMessage);
                return false;
            }
            return true;
        }
        try
        {
            msg = EncryptedMessage.Deserialize(AES.AesGcmDecrypt(packed.Data, EncryptionKey));
        }
        catch (Exception ex)
        {
            GameCore.Console.AddLog("Failed to decrypt received message. Exception: " + ex.Message, UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
            GameCore.Console.AddLog(ex.StackTrace, UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
            msg = default(EncryptedMessage);
            return false;
        }
        if (_rxCounter == uint.MaxValue)
        {
            _rxCounter = 0u;
        }
        if (msg.Counter <= _rxCounter)
        {
            GameCore.Console.AddLog($"Received message with counter {msg.Counter}, which is lower or equal to last received message counter {_rxCounter}. Discarding message!", UnityEngine.Color.red, false, GameCore.Console.ConsoleLogType.Error);
            msg = default(EncryptedMessage);
            return false;
        }
        _rxCounter = msg.Counter;
        return true;
    }

    public void ReceivedSaltHook(string oldSalt, string newSalt)
    {
        if (newSalt != null && EncryptionKey == null && !base.isLocalPlayer)
        {
            PrepareExchange();
            string clientPublicKey = ECDSA.KeyToString(EcdhKeys.Public);
            AsymmetricKeyParameter serverPublicKey = ECDSA.PublicKeyFromString(CentralAuthManager.ServerEcdhPublicKey);
            byte[] signature = ECDSA.SignBytes(clientPublicKey, CentralAuthManager.SessionKeys.Private);
            ClientProcessExchange(clientPublicKey, serverPublicKey, signature);
        }
    }

    public static void RegisterClientHandler(EncryptedChannel channel, EncryptedMessageHandler handler)
    {
        ClientChannelHandlers[channel] = handler;
    }

    public static void RegisterServerHandler(EncryptedChannel channel, EncryptedMessageServerHandler handler)
    {
        ServerChannelHandlers[channel] = handler;
    }
}