using System.Collections.Generic;

using UnityEngine;

namespace InventorySystem.Items.Firearms.BasicMessages
{
    public static class FirearmClientsideStateDatabase
    {
        private static readonly Dictionary<ushort, FirearmStatus> PreReloadStatuses = new Dictionary<ushort, FirearmStatus>();

		private static readonly Dictionary<ushort, float> ReloadTimes = new Dictionary<ushort, float>();

        private static readonly Dictionary<ushort, RequestType> ReloadTracker = new Dictionary<ushort, RequestType>();

		private static readonly Dictionary<ushort, bool> AdsTracker = new Dictionary<ushort, bool>();

        [global::UnityEngine.RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            global::InventorySystem.Inventory.OnCurrentItemChanged += (ReferenceHub hub, global::InventorySystem.Items.ItemIdentifier old, global::InventorySystem.Items.ItemIdentifier cur) =>
            {
                HandleItemChange(cur);
            };
            global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.OnClientConfirmationReceived += HandleMessage;
        }

        private static void HandleMessage(global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage msg)
        {
            global::InventorySystem.Items.Firearms.BasicMessages.RequestType request = msg.Request;
            if (request == global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Reload
                || request == global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Unload
                || request == global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ReloadStop)
            {
                ReloadTracker[msg.Serial] = msg.Request;
                ReloadTimes[msg.Serial] = global::UnityEngine.Time.timeSinceLevelLoad;
                if (global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.ReceivedStatuses.TryGetValue(msg.Serial, out var value))
                {
                    PreReloadStatuses[msg.Serial] = value;
                }
            }
        }

        private static void HandleItemChange(global::InventorySystem.Items.ItemIdentifier newItem)
        {
            AdsTracker[newItem.SerialNumber] = false;
            ReloadTracker[newItem.SerialNumber] = global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ReloadStop;
        }


        public static global::InventorySystem.Items.Firearms.BasicMessages.RequestType GetReloadStateRaw(ushort serial)
        {
            if (!ReloadTracker.TryGetValue(serial, out var value))
            {
                return global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ReloadStop;
            }
            return value;
        }

        public static bool IsAdsing(ushort serial)
        {
            bool value;
            return AdsTracker.TryGetValue(serial, out value) && value;
        }

        public static bool IsReloading(ushort serial)
        {
            return GetReloadStateRaw(serial) == global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Reload;
        }

        public static bool IsUnloading(ushort serial)
        {
            return GetReloadStateRaw(serial) == global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Unload;
        }

        public static bool IsIdle(ushort serial)
        {
            return GetReloadStateRaw(serial) == global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ReloadStop;
        }

        public static float ElapsedReloadState(ushort serial)
        {
            return global::UnityEngine.Time.timeSinceLevelLoad - (ReloadTimes.TryGetValue(serial, out float value) ? value : 0f);
        }

        public static global::InventorySystem.Items.Firearms.FirearmStatus GetPreReloadStatus(ushort serial)
        {
            if (!PreReloadStatuses.TryGetValue(serial, out var value))
            {
                global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.ReceivedStatuses.TryGetValue(serial, out value);
            }
            return value;
        }
    }
}