using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Utils.Networking
{
    public static class NetworkUtils
    {
        private static readonly Dictionary<uint, NetworkIdentity> EmptyDict = new Dictionary<uint, NetworkIdentity>();

        public static Dictionary<uint, NetworkIdentity> SpawnedNetIds
        {
            get
            {
                if (NetworkServer.active) return NetworkServer.spawned;
                if (NetworkClient.active) return NetworkClient.spawned;

                return EmptyDict;
            }
        }

        public static void SendToAuthenticated<T>(this T message, int channelId = 0) where T : struct, NetworkMessage
        {
            message.SendToHubsConditionally((ReferenceHub x) => x != null && x.Mode != ClientInstanceMode.Unverified, channelId);
        }

        public static void SendToHubsConditionally<T>(this T msg, Func<ReferenceHub, bool> predicate, int channelId = 0) where T : struct, NetworkMessage
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning($"[NetworkUtils] SendToHubsConditionally: NetworkServer is not active! Message {typeof(T).Name} ignored.");
                return;
            }

            foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
            {
                if (referenceHub != null && predicate(referenceHub))
                {
                    NetworkConnectionToClient conn = referenceHub.connectionToClient;

                    if (conn != null && conn.isReady)
                    {
                        conn.Send(msg, channelId);
                    }
                }
            }
        }

        public static void DumpHandlers()
        {
            var field = typeof(NetworkServer).GetField("handlers", BindingFlags.Static | BindingFlags.NonPublic);
            var handlers = (Dictionary<ushort, NetworkMessageDelegate>)field.GetValue(null);

            Debug.Log($"<color=cyan>[NetworkTable]</color> ����� ���������: {handlers.Count}");

            foreach (var kvp in handlers)
            {
                string messageTypeName = "UnknownMessage";

                try
                {
                    var target = kvp.Value.Target;
                    if (target != null)
                    {
                        var method = target.GetType().GetMethod("Invoke");
                        var genericTypes = target.GetType().BaseType?.GetGenericArguments();

                        if (genericTypes != null && genericTypes.Length > 0)
                            messageTypeName = genericTypes[0].Name;
                        else
                            messageTypeName = target.GetType().Name;
                    }
                }
                catch { }

                Debug.Log($"<b>ID: {kvp.Key}</b> �> ���: <color=yellow>{messageTypeName}</color>");
            }
        }
    }
}