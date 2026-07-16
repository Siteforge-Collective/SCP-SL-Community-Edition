using System;
using System.Collections.Generic;
using Footprinting;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;
using Utils.Networking;

namespace InventorySystem.Items.Usables.Scp330
{
    public static class Scp330NetworkHandler
    {
        public static readonly Dictionary<ushort, CandyKindID> ReceivedSelectedCandies = new Dictionary<ushort, CandyKindID>();

        public static event Action<SelectScp330Message> OnClientSelectMessageReceived;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += RegisterHandlers;
        }

        private static void RegisterHandlers()
        {
            NetworkServer.ReplaceHandler<SelectScp330Message>(ServerSelectMessageReceived);
            NetworkClient.ReplaceHandler<SelectScp330Message>(ClientSelectMessageReceived);
            NetworkClient.ReplaceHandler<SyncScp330Message>(ClientSyncMessageReceived);
            ReceivedSelectedCandies.Clear();
        }

        private static void ServerSelectMessageReceived(NetworkConnection conn, SelectScp330Message msg)
        {
            if (!ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) || !(hub.inventory.CurInstance is Scp330Bag scp330Bag) || scp330Bag == null || scp330Bag.ItemSerial != msg.Serial || msg.CandyID >= scp330Bag.Candies.Count)
            {
                return;
            }
            if (msg.Drop)
            {
                if (InventoryExtensions.ServerCreatePickup(psi: new PickupSyncInfo(scp330Bag.ItemTypeId, scp330Bag.Weight, 0), inv: hub.inventory, item: scp330Bag) is Scp330Pickup scp330Pickup)
                {
                    scp330Pickup.PreviousOwner = new Footprint(hub);
                    CandyKindID candyKindID = scp330Bag.TryRemove(msg.CandyID);
                    if (candyKindID != CandyKindID.None)
                    {
                        scp330Pickup.ExposedCandy = candyKindID;
                        scp330Pickup.StoredCandies.Add(candyKindID);
                    }
                }
            }
            else if (msg.CandyID >= 0 && msg.CandyID < scp330Bag.Candies.Count)
            {
                scp330Bag.SelectedCandyId = msg.CandyID;
                msg.CandyID = (int)scp330Bag.Candies[msg.CandyID];
                PlayerHandler handler = UsableItemsController.GetHandler(hub);
                handler.CurrentUsable = new CurrentlyUsedItem(scp330Bag, msg.Serial, Time.timeSinceLevelLoad);
                handler.CurrentUsable.Item.OnUsingStarted();
                msg.SendToAuthenticated();
            }
        }

        private static void ClientSyncMessageReceived(SyncScp330Message msg)
        {
            if (ReferenceHub.TryGetLocalHub(out var hub) && hub.inventory.UserInventory.Items.TryGetValue(msg.Serial, out var value) && value is Scp330Bag scp330Bag)
            {
                scp330Bag.Candies = msg.Candies;
            }
        }

        private static void ClientSelectMessageReceived(SelectScp330Message msg)
        {
        }

        public static void SerializeSyncMessage(this NetworkWriter writer, SyncScp330Message value)
        {
            writer.WriteUShort(value.Serial);
            writer.WriteByte((byte)value.Candies.Count);
            foreach (CandyKindID candy in value.Candies)
            {
                writer.WriteByte((byte)candy);
            }
        }

        public static SyncScp330Message DeserializeSyncMessage(this NetworkReader reader)
        {
            ushort serial = reader.ReadUShort();
            byte b = reader.ReadByte();
            List<CandyKindID> list = new List<CandyKindID>();
            for (int i = 0; i < b; i++)
            {
                list.Add((CandyKindID)reader.ReadByte());
            }
            return new SyncScp330Message
            {
                Candies = list,
                Serial = serial
            };
        }

        public static void SerializeSelectMessage(this NetworkWriter writer, SelectScp330Message value)
        {
            int num = value.CandyID + 1;
            writer.WriteUShort(value.Serial);
            writer.WriteSByte((sbyte)(value.Drop ? (-num) : num));
        }

        public static SelectScp330Message DeserializeSelectMessage(this NetworkReader reader)
        {
            ushort serial = reader.ReadUShort();
            int num = reader.ReadSByte();
            return new SelectScp330Message
            {
                CandyID = Mathf.Abs(num) - 1,
                Serial = serial,
                Drop = (num < 0)
            };
        }
    }
}
