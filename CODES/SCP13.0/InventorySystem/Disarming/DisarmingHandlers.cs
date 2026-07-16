using System.Collections.Generic;
using InventorySystem.Items;
using Mirror;
using PlayerRoles;
using PluginAPI.Events;
using UnityEngine;
using Utils.Networking;

namespace InventorySystem.Disarming
{
    public static class DisarmingHandlers
    {
        public delegate void PlayerDisarmed(ReferenceHub disarmerHub, ReferenceHub targetHub);

        private static readonly Dictionary<uint, float> ServerCooldowns = new Dictionary<uint, float>();

        private const float ServerDisarmingDistanceSqrt = 20f;

        private const float ServerRequestCooldown = 0.8f;

        private static DisarmedPlayersListMessage NewDisarmedList => new DisarmedPlayersListMessage(DisarmedPlayers.Entries);

        public static event PlayerDisarmed OnPlayerDisarmed;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += ReplaceHandlers;
            Inventory.OnLocalClientStarted += delegate
            {
                NetworkClient.Send(new DisarmMessage(null, disarm: false, isNull: true));
            };
        }

        private static void ReplaceHandlers()
        {
            DisarmedPlayers.Entries.Clear();
            ServerCooldowns.Clear();
            NetworkServer.ReplaceHandler<DisarmMessage>(ServerProcessDisarmMessage);
            NetworkClient.ReplaceHandler<DisarmedPlayersListMessage>(ClientProcessListMessage);
        }

        private static void ServerProcessDisarmMessage(NetworkConnection conn, DisarmMessage msg)
        {
            if (!NetworkServer.active || !ServerCheckCooldown(conn) || !ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) || (!msg.PlayerIsNull && ((msg.PlayerToDisarm.transform.position - hub.transform.position).sqrMagnitude > 20f || (msg.PlayerToDisarm.inventory.CurInstance != null && msg.PlayerToDisarm.inventory.CurInstance.TierFlags != ItemTierFlags.Common))))
            {
                return;
            }
            bool flag = !msg.PlayerIsNull && msg.PlayerToDisarm.inventory.IsDisarmed();
            bool flag2 = !msg.PlayerIsNull && hub.CanStartDisarming(msg.PlayerToDisarm);
            if (flag && !msg.Disarm)
            {
                if (!hub.inventory.IsDisarmed())
                {
                    bool flag3 = hub.GetTeam() == Team.SCPs;
                    PlayerRemoveHandcuffsEvent playerRemoveHandcuffsEvent = new PlayerRemoveHandcuffsEvent(hub, msg.PlayerToDisarm);
                    if (!EventManager.ExecuteEvent(playerRemoveHandcuffsEvent))
                    {
                        return;
                    }
                    if (flag3 && playerRemoveHandcuffsEvent.CanRemoveHandcuffsAsScp)
                    {
                        flag3 = false;
                    }
                    if (flag3)
                    {
                        return;
                    }
                    msg.PlayerToDisarm.inventory.SetDisarmedStatus(null);
                }
            }
            else
            {
                if (!(!flag && flag2) || !msg.Disarm)
                {
                    hub.networkIdentity.connectionToClient.Send(NewDisarmedList);
                    return;
                }
                if (msg.PlayerToDisarm.inventory.CurInstance == null || msg.PlayerToDisarm.inventory.CurInstance.CanHolster())
                {
                    if (!EventManager.ExecuteEvent(new PlayerHandcuffEvent(hub, msg.PlayerToDisarm)))
                    {
                        return;
                    }
                    DisarmingHandlers.OnPlayerDisarmed?.Invoke(hub, msg.PlayerToDisarm);
                    msg.PlayerToDisarm.inventory.SetDisarmedStatus(hub.inventory);
                }
            }
            NewDisarmedList.SendToAuthenticated();
        }

        private static bool ServerCheckCooldown(NetworkConnection conn)
        {
            uint netId = conn.identity.netId;
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            if (!ServerCooldowns.TryGetValue(conn.identity.netId, out var value))
            {
                value = 0f;
            }
            if (timeSinceLevelLoad < value + 0.8f)
            {
                return false;
            }
            ServerCooldowns[netId] = timeSinceLevelLoad;
            return true;
        }

        private static void ClientProcessListMessage(DisarmedPlayersListMessage msg)
        {
            if (!NetworkServer.active)
            {
                DisarmedPlayers.Entries = msg.Entries;
            }
        }
    }
}
