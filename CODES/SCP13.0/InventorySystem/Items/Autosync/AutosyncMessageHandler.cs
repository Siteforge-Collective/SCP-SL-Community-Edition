using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Autosync
{
    public static class AutosyncMessageHandler
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += delegate
            {
                NetworkServer.ReplaceHandler<AutosyncMessage>(ServerHandleMessage);
                NetworkClient.ReplaceHandler(delegate (AutosyncMessage msg)
                {
                    msg.ProcessRpc();
                });
            };
        }

        private static void ServerHandleMessage(NetworkConnection conn, AutosyncMessage msg)
        {
            if (!(conn.identity == null) && ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub))
            {
                msg.ProcessCmd(hub);
            }
        }

        public static AutosyncMessage ReadAutosyncMessage(this NetworkReader reader)
        {
            return new AutosyncMessage(reader);
        }

        public static void WriteAutosyncMessage(this NetworkWriter writer, AutosyncMessage msg)
        {
            msg.Serialize(writer);
        }
    }
}
