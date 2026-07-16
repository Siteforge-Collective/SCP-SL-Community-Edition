namespace InventorySystem.Items.Usables
{
	public static class UsableItemsController
	{
		public static readonly global::System.Collections.Generic.Dictionary<ReferenceHub, global::InventorySystem.Items.Usables.PlayerHandler> Handlers = new global::System.Collections.Generic.Dictionary<ReferenceHub, global::InventorySystem.Items.Usables.PlayerHandler>();

		public static readonly global::System.Collections.Generic.Dictionary<ushort, float> GlobalItemCooldowns = new global::System.Collections.Generic.Dictionary<ushort, float>();

		public static event global::System.Action<ReferenceHub, global::InventorySystem.Items.Usables.UsableItem> ServerOnUsingCompleted;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void InitOnLoad()
		{
			CustomNetworkManager.OnClientReady += OnClientReady;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += ResetPlayerOnRoleChange;
			StaticUnityMethods.OnUpdate += Update;
			ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, (global::System.Action<ReferenceHub>)delegate(ReferenceHub hub)
			{
				Handlers.Remove(hub);
			});
		}

		public static void OnClientReady()
		{
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Usables.StatusMessage>(ServerReceivedStatus);
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Usables.StatusMessage>(ClientReceivedStatus);
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Usables.ItemCooldownMessage>(ClientReceivedCooldown);
			GlobalItemCooldowns.Clear();
			Handlers.Clear();
		}

		public static global::InventorySystem.Items.Usables.PlayerHandler GetHandler(ReferenceHub ply)
		{
			if (!Handlers.TryGetValue(ply, out var value))
			{
				value = new global::InventorySystem.Items.Usables.PlayerHandler();
				Handlers.Add(ply, value);
			}
			return value;
		}

		public static float GetCooldown(ushort itemSerial, global::InventorySystem.Items.ItemBase item, global::InventorySystem.Items.Usables.PlayerHandler ply)
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
			return num - global::UnityEngine.Time.timeSinceLevelLoad;
		}

		private static void Update()
		{
			if (!StaticUnityMethods.IsPlaying || !global::Mirror.NetworkServer.active)
			{
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<ReferenceHub, global::InventorySystem.Items.Usables.PlayerHandler> handler in Handlers)
			{
				handler.Value.DoUpdate(handler.Key);
				global::InventorySystem.Items.Usables.CurrentlyUsedItem currentUsable = handler.Value.CurrentUsable;
				if (currentUsable.ItemSerial == 0)
				{
					continue;
				}
				float speedMultiplier = global::CustomPlayerEffects.UsableItemModifierEffectExtensions.GetSpeedMultiplier(currentUsable.Item.ItemTypeId, handler.Key);
				if (currentUsable.ItemSerial != handler.Key.inventory.CurItem.SerialNumber)
				{
					if (currentUsable.Item != null)
					{
						currentUsable.Item.OnUsingCancelled();
					}
					handler.Value.CurrentUsable = global::InventorySystem.Items.Usables.CurrentlyUsedItem.None;
					handler.Key.inventory.connectionToClient.Send(new global::InventorySystem.Items.Usables.StatusMessage(global::InventorySystem.Items.Usables.StatusMessage.StatusType.Cancel, currentUsable.ItemSerial));
				}
				else if (global::UnityEngine.Time.timeSinceLevelLoad >= currentUsable.StartTime + currentUsable.Item.UseTime / speedMultiplier)
				{
					if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerUsedItem, handler.Key, currentUsable.Item))
					{
						break;
					}
					currentUsable.Item.ServerOnUsingCompleted();
					global::InventorySystem.Items.Usables.UsableItemsController.ServerOnUsingCompleted?.Invoke(handler.Key, currentUsable.Item);
					handler.Value.CurrentUsable = global::InventorySystem.Items.Usables.CurrentlyUsedItem.None;
				}
			}
		}

		private static void ServerReceivedStatus(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Usables.StatusMessage msg)
		{
			if (!ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) || !(hub.inventory.CurInstance is global::InventorySystem.Items.Usables.UsableItem usableItem) || usableItem.ItemSerial != msg.ItemSerial)
			{
				return;
			}
			global::InventorySystem.Items.Usables.PlayerHandler handler = GetHandler(hub);
			switch (msg.Status)
			{
			case global::InventorySystem.Items.Usables.StatusMessage.StatusType.Start:
				if (usableItem.ServerValidateStartRequest(handler) && handler.CurrentUsable.ItemSerial == 0 && usableItem.CanStartUsing)
				{
					float cooldown = GetCooldown(msg.ItemSerial, usableItem, handler);
					if (cooldown > 0f)
					{
						conn.Send(new global::InventorySystem.Items.Usables.ItemCooldownMessage(msg.ItemSerial, cooldown));
					}
					else if (global::CustomPlayerEffects.UsableItemModifierEffectExtensions.GetSpeedMultiplier(usableItem.ItemTypeId, hub) > 0f && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerUseItem, hub, usableItem))
					{
						handler.CurrentUsable = new global::InventorySystem.Items.Usables.CurrentlyUsedItem(usableItem, msg.ItemSerial, global::UnityEngine.Time.timeSinceLevelLoad);
						handler.CurrentUsable.Item.OnUsingStarted();
						global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Usables.StatusMessage(global::InventorySystem.Items.Usables.StatusMessage.StatusType.Start, msg.ItemSerial));
					}
				}
				break;
			case global::InventorySystem.Items.Usables.StatusMessage.StatusType.Cancel:
				if (usableItem.ServerValidateCancelRequest(handler) && handler.CurrentUsable.ItemSerial != 0)
				{
					float speedMultiplier = global::CustomPlayerEffects.UsableItemModifierEffectExtensions.GetSpeedMultiplier(handler.CurrentUsable.Item.ItemTypeId, hub);
					if (handler.CurrentUsable.StartTime + handler.CurrentUsable.Item.MaxCancellableTime / speedMultiplier > global::UnityEngine.Time.timeSinceLevelLoad && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerCancelUsingItem, hub, handler.CurrentUsable.Item))
					{
						handler.CurrentUsable.Item.OnUsingCancelled();
						handler.CurrentUsable = global::InventorySystem.Items.Usables.CurrentlyUsedItem.None;
						global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Usables.StatusMessage(global::InventorySystem.Items.Usables.StatusMessage.StatusType.Cancel, msg.ItemSerial));
					}
				}
				break;
			}
		}

		private static void ClientReceivedStatus(global::InventorySystem.Items.Usables.StatusMessage msg)
		{
		}

		private static void ClientReceivedCooldown(global::InventorySystem.Items.Usables.ItemCooldownMessage msg)
		{
		}

		private static void ResetPlayerOnRoleChange(ReferenceHub ply, global::PlayerRoles.PlayerRoleBase r1, global::PlayerRoles.PlayerRoleBase r2)
		{
			GetHandler(ply).ResetAll();
		}
	}
}
