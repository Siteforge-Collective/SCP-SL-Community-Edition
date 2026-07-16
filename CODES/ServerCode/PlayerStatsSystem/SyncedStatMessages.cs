namespace PlayerStatsSystem
{
	public static class SyncedStatMessages
	{
		public struct StatMessage : global::Mirror.NetworkMessage
		{
			public global::PlayerStatsSystem.SyncedStatBase Stat;

			public float SyncedValue;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += RegisterHandler;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
		}

		private static void OnRoleChanged(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevClass, global::PlayerRoles.PlayerRoleBase newClass)
		{
			if (!global::Mirror.NetworkServer.active || !(newClass is global::PlayerRoles.Spectating.SpectatorRole) || userHub.isLocalPlayer)
			{
				return;
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (global::PlayerRoles.PlayerRolesUtils.IsAlive(allHub) && !(allHub == userHub))
				{
					SendAllStats(userHub.networkIdentity.connectionToClient, allHub.playerStats);
				}
			}
		}

		private static void SendAllStats(global::Mirror.NetworkConnectionToClient conn, global::PlayerStatsSystem.PlayerStats ply)
		{
			global::PlayerStatsSystem.StatBase[] statModules = ply.StatModules;
			for (int i = 0; i < statModules.Length; i++)
			{
				if (statModules[i] is global::PlayerStatsSystem.SyncedStatBase syncedStatBase && syncedStatBase.Mode != global::PlayerStatsSystem.SyncedStatBase.SyncMode.Private)
				{
					conn.Send(new global::PlayerStatsSystem.SyncedStatMessages.StatMessage
					{
						Stat = syncedStatBase,
						SyncedValue = syncedStatBase.CurValue
					});
				}
			}
		}

		public static void Serialize(this global::Mirror.NetworkWriter writer, global::PlayerStatsSystem.SyncedStatMessages.StatMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, value.Stat.Hub.netId);
			writer.WriteByte(value.Stat.SyncId);
			value.Stat.WriteValue(writer);
		}

		public static global::PlayerStatsSystem.SyncedStatMessages.StatMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			uint netId = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
			byte syncId = reader.ReadByte();
			global::PlayerStatsSystem.SyncedStatBase statOfUser = global::PlayerStatsSystem.SyncedStatBase.GetStatOfUser(netId, syncId);
			return new global::PlayerStatsSystem.SyncedStatMessages.StatMessage
			{
				Stat = statOfUser,
				SyncedValue = statOfUser.ReadValue(reader)
			};
		}

		private static void RegisterHandler()
		{
			global::Mirror.NetworkClient.ReplaceHandler(delegate(global::PlayerStatsSystem.SyncedStatMessages.StatMessage msg)
			{
				msg.Stat.CurValue = msg.SyncedValue;
			});
		}
	}
}
