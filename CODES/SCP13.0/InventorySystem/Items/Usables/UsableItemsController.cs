using CustomPlayerEffects;
using Mirror;
using PlayerRoles;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utils.Networking;

namespace InventorySystem.Items.Usables
{
	public static class UsableItemsController
	{
        public static readonly Dictionary<ReferenceHub, PlayerHandler> Handlers = new Dictionary<ReferenceHub, PlayerHandler>();

        public static readonly Dictionary<ushort, float> GlobalItemCooldowns = new Dictionary<ushort, float>();

        public static readonly Dictionary<ushort, float> StartTimes;

        private static readonly Dictionary<ushort, AudioSource> CurrentlyPlayingSources = new Dictionary<ushort, AudioSource>();

        public static event Action<ReferenceHub, UsableItem> ServerOnUsingCompleted;

        public static event Action<StatusMessage> OnClientStatusReceived
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

        [RuntimeInitializeOnLoadMethod]
        private static void InitOnLoad()
        {
            CustomNetworkManager.OnClientReady += OnClientReady;
            PlayerRoleManager.OnRoleChanged += ResetPlayerOnRoleChange;
            StaticUnityMethods.OnUpdate += Update;
            ReferenceHub.OnPlayerRemoved = (Action<ReferenceHub>)Delegate.Combine(ReferenceHub.OnPlayerRemoved, (Action<ReferenceHub>)delegate (ReferenceHub hub)
            {
                Handlers.Remove(hub);
            });
        }

        public static void OnClientReady()
        {
            NetworkServer.ReplaceHandler<StatusMessage>(ServerReceivedStatus);
            NetworkClient.ReplaceHandler<StatusMessage>(ClientReceivedStatus);
            NetworkClient.ReplaceHandler<ItemCooldownMessage>(ClientReceivedCooldown);
            GlobalItemCooldowns.Clear();
            Handlers.Clear();
        }

        public static PlayerHandler GetHandler(ReferenceHub ply)
        {
            if (!Handlers.TryGetValue(ply, out var value))
            {
                value = new PlayerHandler();
                Handlers.Add(ply, value);
            }
            return value;
        }

        public static float GetCooldown(ushort itemSerial, ItemBase item, PlayerHandler ply)
        {
            float num = 0f;
            if (GlobalItemCooldowns.TryGetValue(itemSerial, out var value))
            {
                num = value;
            }
            if (ply.PersonalCooldowns.TryGetValue(item.ItemTypeId, out var value2) && value2 > num)
            {
                num = value2;
            }
            return num - Time.timeSinceLevelLoad;
        }


        public static void PlaySoundOnPlayer(ReferenceHub ply, AudioClip clip)
		{
		}

        private static void Update()
        {
            if (!StaticUnityMethods.IsPlaying || !NetworkServer.active)
            {
                return;
            }
            foreach (KeyValuePair<ReferenceHub, PlayerHandler> handler in Handlers)
            {
                handler.Value.DoUpdate(handler.Key);
                CurrentlyUsedItem currentUsable = handler.Value.CurrentUsable;
                if (currentUsable.ItemSerial == 0)
                {
                    continue;
                }
                float speedMultiplier = currentUsable.Item.ItemTypeId.GetSpeedMultiplier(handler.Key);
                if (currentUsable.ItemSerial != handler.Key.inventory.CurItem.SerialNumber)
                {
                    if (currentUsable.Item != null)
                    {
                        currentUsable.Item.OnUsingCancelled();
                    }
                    handler.Value.CurrentUsable = CurrentlyUsedItem.None;
                    handler.Key.inventory.connectionToClient.Send(new StatusMessage(StatusMessage.StatusType.Cancel, currentUsable.ItemSerial));
                }
                else if (Time.timeSinceLevelLoad >= currentUsable.StartTime + currentUsable.Item.UseTime / speedMultiplier)
                {
                    if (!EventManager.ExecuteEvent(new PlayerUsedItemEvent(handler.Key, currentUsable.Item)))
                    {
                        break;
                    }
                    currentUsable.Item.ServerOnUsingCompleted();
                    UsableItemsController.ServerOnUsingCompleted?.Invoke(handler.Key, currentUsable.Item);
                    handler.Value.CurrentUsable = CurrentlyUsedItem.None;
                }
            }
        }


        private static void ServerReceivedStatus(NetworkConnection conn, StatusMessage msg)
        {
            if (!ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) || !(hub.inventory.CurInstance is UsableItem usableItem) || usableItem.ItemSerial != msg.ItemSerial)
            {
                return;
            }
            PlayerHandler handler = GetHandler(hub);
            switch (msg.Status)
            {
                case StatusMessage.StatusType.Start:
                    if (usableItem.ServerValidateStartRequest(handler) && handler.CurrentUsable.ItemSerial == 0 && usableItem.CanStartUsing)
                    {
                        float cooldown = GetCooldown(msg.ItemSerial, usableItem, handler);
                        if (cooldown > 0f)
                        {
                            conn.Send(new ItemCooldownMessage(msg.ItemSerial, cooldown));
                        }
                        else if (usableItem.ItemTypeId.GetSpeedMultiplier(hub) > 0f && EventManager.ExecuteEvent(new PlayerUseItemEvent(hub, usableItem)))
                        {
                            handler.CurrentUsable = new CurrentlyUsedItem(usableItem, msg.ItemSerial, Time.timeSinceLevelLoad);
                            handler.CurrentUsable.Item.OnUsingStarted();
                            new StatusMessage(StatusMessage.StatusType.Start, msg.ItemSerial).SendToAuthenticated();
                        }
                    }
                    break;
                case StatusMessage.StatusType.Cancel:
                    if (usableItem.ServerValidateCancelRequest(handler) && handler.CurrentUsable.ItemSerial != 0)
                    {
                        float speedMultiplier = handler.CurrentUsable.Item.ItemTypeId.GetSpeedMultiplier(hub);
                        if (handler.CurrentUsable.StartTime + handler.CurrentUsable.Item.MaxCancellableTime / speedMultiplier > Time.timeSinceLevelLoad && EventManager.ExecuteEvent(new PlayerCancelUsingItemEvent(hub, handler.CurrentUsable.Item)))
                        {
                            handler.CurrentUsable.Item.OnUsingCancelled();
                            handler.CurrentUsable = CurrentlyUsedItem.None;
                            new StatusMessage(StatusMessage.StatusType.Cancel, msg.ItemSerial).SendToAuthenticated();
                        }
                    }
                    break;
            }
        }

        private static void ClientReceivedStatus(StatusMessage msg)
		{
		}

		private static void ClientReceivedCooldown(ItemCooldownMessage msg)
		{
		}

        private static void ResetPlayerOnRoleChange(ReferenceHub ply, PlayerRoleBase r1, PlayerRoleBase r2)
        {
            GetHandler(ply).ResetAll();
        }
    }
}
