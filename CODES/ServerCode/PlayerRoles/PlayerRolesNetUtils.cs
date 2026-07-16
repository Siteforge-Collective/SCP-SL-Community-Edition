namespace PlayerRoles
{
	public static class PlayerRolesNetUtils
	{
		public static readonly global::System.Collections.Generic.Dictionary<uint, global::Mirror.NetworkReader> QueuedRoles = new global::System.Collections.Generic.Dictionary<uint, global::Mirror.NetworkReader>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				QueuedRoles.Clear();
				global::Mirror.NetworkClient.ReplaceHandler((global::System.Action<global::PlayerRoles.RoleSyncInfo>)delegate
				{
				}, true);
				global::Mirror.NetworkClient.ReplaceHandler((global::System.Action<global::PlayerRoles.RoleSyncInfoPack>)delegate
				{
				}, true);
			};
			ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(HandleSpawnedPlayer));
		}

		private static void HandleSpawnedPlayer(ReferenceHub hub)
		{
			global::Mirror.NetworkReader value;
			if (global::Mirror.NetworkServer.active)
			{
				if (!hub.isLocalPlayer)
				{
					hub.connectionToClient.Send(new global::PlayerRoles.RoleSyncInfoPack(hub));
				}
			}
			else if (QueuedRoles.TryGetValue(hub.netId, out value))
			{
				hub.roleManager.InitializeNewRole(value.ReadRoleType(), global::PlayerRoles.RoleChangeReason.None, global::PlayerRoles.RoleSpawnFlags.All, value);
			}
		}

		public static void WriteRoleSyncInfo(this global::Mirror.NetworkWriter writer, global::PlayerRoles.RoleSyncInfo info)
		{
			info.Write(writer);
		}

		public static global::PlayerRoles.RoleSyncInfo ReadRoleSyncInfo(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.RoleSyncInfo(reader);
		}

		public static void WriteRoleSyncInfoPack(this global::Mirror.NetworkWriter writer, global::PlayerRoles.RoleSyncInfoPack info)
		{
			info.WritePlayers(writer);
		}

		public static global::PlayerRoles.RoleSyncInfoPack ReadRoleSyncInfoPack(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.RoleSyncInfoPack(reader);
		}
	}
}
