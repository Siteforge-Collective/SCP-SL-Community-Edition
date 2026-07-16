using System.Collections.Generic;
using InventorySystem.Items;
using Mirror;
using PlayerRoles;
using UnityEngine;
using Utils.Networking;

namespace InventorySystem.Disarming
{
    public static class DisarmedPlayers
    {
        public readonly struct DisarmedEntry
        {
            public readonly uint DisarmedPlayer;
            public readonly uint Disarmer;

            public DisarmedEntry(uint disarmedPlayer, uint disarmer)
            {
                DisarmedPlayer = disarmedPlayer;
                Disarmer = disarmer;
            }
        }

        public static List<DisarmedEntry> Entries = new List<DisarmedEntry>();

        private const float AutoDisarmDistanceSquared = 8100f;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            StaticUnityMethods.OnUpdate += Update;
            PlayerRoleManager.OnRoleChanged += (hub, prevRole, newRole) =>
            {
                if (!NetworkServer.active || !(prevRole is IInventoryRole)) return;

                for (int i = 0; i < Entries.Count; i++)
                {
                    if (Entries[i].DisarmedPlayer == hub.netId)
                    {
                        Entries.RemoveAt(i);
                        new DisarmedPlayersListMessage(Entries).SendToAuthenticated();
                        break;
                    }
                }
            };
        }

        private static void Update()
        {
            if (!NetworkServer.active || Entries.Count == 0) return;

            bool changed = false;
            for (int i = Entries.Count - 1; i >= 0; i--)
            {
                if (!ValidateEntry(Entries[i]))
                {
                    Entries.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
            {
                new DisarmedPlayersListMessage(Entries).SendToAuthenticated();
            }
        }

        private static bool ValidateEntry(DisarmedEntry entry)
        {
            if (!ReferenceHub.TryGetHubNetID(entry.DisarmedPlayer, out ReferenceHub targetHub) ||
                !ReferenceHub.TryGetHubNetID(entry.Disarmer, out ReferenceHub disarmerHub))
                return false;

            if (Vector3.SqrMagnitude(targetHub.transform.position - disarmerHub.transform.position) > AutoDisarmDistanceSquared)
                return false;

            if (!disarmerHub.IsAlive())
                return false;

            return true;
        }

        public static bool IsDisarmed(this Inventory inv)
        {
            if (inv == null) return false;
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].DisarmedPlayer == inv.netId) return true;
            }
            return false;
        }

        public static void SetDisarmedStatus(this Inventory inv, Inventory disarmer)
        {
            if (inv == null || disarmer == null || !NetworkServer.active) return;

            // Убираем старую запись, если была
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].DisarmedPlayer == inv.netId)
                {
                    Entries.RemoveAt(i);
                    break;
                }
            }

            Entries.Add(new DisarmedEntry(inv.netId, disarmer.netId));
            new DisarmedPlayersListMessage(Entries).SendToAuthenticated();
        }

        public static bool ValidateDisarmament(this ReferenceHub disarmerHub, ReferenceHub targetHub)
        {
            if (targetHub.roleManager.CurrentRole is IInventoryRole inventoryRole)
            {
                return inventoryRole.AllowDisarming(disarmerHub);
            }
            return false;
        }

        public static bool CanStartDisarming(this ReferenceHub disarmerHub, ReferenceHub targetHub)
        {
            if (!disarmerHub.ValidateDisarmament(targetHub)) return false;

            ItemBase curInstance = disarmerHub.inventory.CurInstance;
            if (curInstance is IDisarmingItem disarmingItem)
            {
                return disarmingItem.AllowDisarming;
            }
            return false;
        }

        public static bool CanUndisarm(this ReferenceHub disarmerHub, ReferenceHub targetHub)
        {
            if (!targetHub.inventory.IsDisarmed()) return false;

            if (targetHub.roleManager.CurrentRole is IInventoryRole inventoryRole)
            {
                return inventoryRole.AllowUndisarming(disarmerHub);
            }
            return true;
        }
    }
}