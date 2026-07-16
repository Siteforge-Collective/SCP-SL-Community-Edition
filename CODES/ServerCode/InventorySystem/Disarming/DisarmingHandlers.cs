namespace InventorySystem.Disarming
{
	public static class DisarmingHandlers
	{
		private static readonly global::System.Collections.Generic.Dictionary<uint, float> ServerCooldowns = new global::System.Collections.Generic.Dictionary<uint, float>();

		private const float ServerDisarmingDistanceSqrt = 20f;

		private const float ServerRequestCooldown = 0.8f;

		private static global::InventorySystem.Disarming.DisarmedPlayersListMessage NewDisarmedList => new global::InventorySystem.Disarming.DisarmedPlayersListMessage(global::InventorySystem.Disarming.DisarmedPlayers.Entries);

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += ReplaceHandlers;
			global::InventorySystem.Inventory.OnLocalClientStarted += delegate
			{
				global::Mirror.NetworkClient.Send(new global::InventorySystem.Disarming.DisarmMessage(null, disarm: false, isNull: true));
			};
		}

		private static void ReplaceHandlers()
		{
			global::InventorySystem.Disarming.DisarmedPlayers.Entries.Clear();
			ServerCooldowns.Clear();
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Disarming.DisarmMessage>(ServerProcessDisarmMessage);
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Disarming.DisarmedPlayersListMessage>(ClientProccessListMessage);
		}

		private static void ServerProcessDisarmMessage(global::Mirror.NetworkConnection conn, global::InventorySystem.Disarming.DisarmMessage msg)
		{
			if (!global::Mirror.NetworkServer.active || !ServerCheckCooldown(conn) || !ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) || (!msg.PlayerIsNull && ((msg.PlayerToDisarm.transform.position - hub.transform.position).sqrMagnitude > 20f || (msg.PlayerToDisarm.inventory.CurInstance != null && msg.PlayerToDisarm.inventory.CurInstance.TierFlags != ItemTierFlags.Common))))
			{
				return;
			}
			bool flag = !msg.PlayerIsNull && msg.PlayerToDisarm.inventory.IsDisarmed();
			bool flag2 = !msg.PlayerIsNull && hub.CanDisarm(msg.PlayerToDisarm);
			if (flag && !msg.Disarm && global::PlayerRoles.PlayerRolesUtils.GetTeam(global::PlayerRoles.PlayerRolesUtils.GetRoleId(hub)) != global::PlayerRoles.Team.SCPs)
			{
				if (!hub.inventory.IsDisarmed() && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerRemoveHandcuffs, hub, msg.PlayerToDisarm))
				{
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
				if (msg.PlayerToDisarm.inventory.CurInstance == null || global::InventorySystem.Items.EquipDequipModifierExtensions.CanHolster(msg.PlayerToDisarm.inventory.CurInstance))
				{
					if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerHandcuff, hub, msg.PlayerToDisarm) && global::PlayerRoles.PlayerRolesUtils.GetTeam(msg.PlayerToDisarm) == global::PlayerRoles.Team.FoundationForces && global::PlayerRoles.PlayerRolesUtils.GetRoleId(hub) == global::PlayerRoles.RoleTypeId.ClassD)
					{
						global::Achievements.AchievementHandlerBase.ServerAchieve(hub.networkIdentity.connectionToClient, global::Achievements.AchievementName.TablesHaveTurned);
					}
					msg.PlayerToDisarm.inventory.SetDisarmedStatus(hub.inventory);
				}
			}
			global::Utils.Networking.NetworkUtils.SendToAuthenticated(NewDisarmedList);
		}

		private static bool ServerCheckCooldown(global::Mirror.NetworkConnection conn)
		{
			uint netId = conn.identity.netId;
			float timeSinceLevelLoad = global::UnityEngine.Time.timeSinceLevelLoad;
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

		private static void ClientProccessListMessage(global::InventorySystem.Disarming.DisarmedPlayersListMessage msg)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::InventorySystem.Disarming.DisarmedPlayers.Entries = msg.Entries;
			}
		}
	}
}
