namespace PlayerRoles.Spectating
{
	public static class SpectatorNetworking
	{
		public struct SpectatedNetIdSyncMessage : global::Mirror.NetworkMessage
		{
			public uint NetId;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::PlayerRoles.Spectating.SpectatorNetworking.SpectatedNetIdSyncMessage msg)
				{
					if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole spectatorRole)
					{
						ReferenceHub.TryGetHubNetID(spectatorRole.SyncedSpectatedNetId, out var hub2);
						ReferenceHub.TryGetHubNetID(msg.NetId, out var hub3);
						global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerChangeSpectator, hub, hub2, hub3);
						spectatorRole.SyncedSpectatedNetId = msg.NetId;
					}
				});
			};
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, (global::System.Action)delegate
			{
				global::Mirror.NetworkClient.Send(new global::PlayerRoles.Spectating.SpectatorNetworking.SpectatedNetIdSyncMessage
				{
					NetId = (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) ? hub.netId : 0u)
				});
			});
		}

		public static void SendToSpectatorsOf<T>(this T msg, ReferenceHub target, bool includeTarget = false) where T : struct, global::Mirror.NetworkMessage
		{
			global::Utils.Networking.NetworkUtils.SendToHubsConditionally(msg, (ReferenceHub x) => target.IsSpectatedBy(x) || (includeTarget && x == target));
		}

		public static void ForeachSpectatorOf(ReferenceHub target, global::System.Action<ReferenceHub> action)
		{
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (target.IsSpectatedBy(allHub))
				{
					action(allHub);
				}
			}
		}

		public static bool IsSpectatedBy(this ReferenceHub target, ReferenceHub spectator)
		{
			if (spectator.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole spectatorRole)
			{
				return spectatorRole.SyncedSpectatedNetId == target.netId;
			}
			return false;
		}

		public static bool IsLocallySpectated(this ReferenceHub target)
		{
			if (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub))
			{
				return hub == target;
			}
			return false;
		}
	}
}
