namespace PlayerRoles.RoleAssign
{
	public static class ScpSpawnPreferences
	{
		public struct SpawnPreferences : global::Mirror.NetworkMessage
		{
			private readonly byte _count;

			public readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, int> Preferences;

			public SpawnPreferences(bool autoSetup)
			{
				_count = 0;
				Preferences = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, int>();
				if (!autoSetup)
				{
					return;
				}
				foreach (global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase> allRole in global::PlayerRoles.PlayerRoleLoader.AllRoles)
				{
					if (allRole.Value is global::PlayerRoles.PlayableScps.ISpawnableScp)
					{
						Preferences[allRole.Key] = GetPreference(allRole.Key);
						_count++;
					}
				}
			}

			public SpawnPreferences(global::Mirror.NetworkReader reader)
			{
				_count = reader.ReadByte();
				Preferences = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, int>(_count);
				for (int i = 0; i < _count; i++)
				{
					global::PlayerRoles.RoleTypeId key = reader.ReadRoleType();
					int val = global::Mirror.NetworkReaderExtensions.ReadSByte(reader);
					Preferences[key] = ClampPreference(val);
				}
			}

			public void Serialize(global::Mirror.NetworkWriter writer)
			{
				writer.WriteByte(_count);
				foreach (global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.RoleTypeId, int> preference in Preferences)
				{
					writer.WriteRoleType(preference.Key);
					global::Mirror.NetworkWriterExtensions.WriteSByte(writer, (sbyte)preference.Value);
				}
			}
		}

		public static readonly global::System.Collections.Generic.Dictionary<int, global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences> Preferences = new global::System.Collections.Generic.Dictionary<int, global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences>();

		public const int MaxPreference = 5;

		private const string PrefsPrefix = "SpawnPreference_Role_";

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				Preferences.Clear();
				global::Mirror.NetworkServer.ReplaceHandler<global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences>(OnMessageReceived);
				global::Mirror.NetworkClient.Send(new global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences(autoSetup: true));
			};
		}

		private static int ClampPreference(int val)
		{
			return global::UnityEngine.Mathf.Clamp(val, -5, 5);
		}

		private static void OnMessageReceived(global::Mirror.NetworkConnection conn, global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences msg)
		{
			Preferences[conn.connectionId] = msg;
		}

		public static int GetPreference(global::PlayerRoles.RoleTypeId role)
		{
			return ClampPreference(PlayerPrefsSl.Get("SpawnPreference_Role_" + (int)role, 0));
		}

		public static void SavePreference(global::PlayerRoles.RoleTypeId role, int value)
		{
			PlayerPrefsSl.Set("SpawnPreference_Role_" + (int)role, value);
		}

		public static void WriteSpawnPreferences(this global::Mirror.NetworkWriter writer, global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences msg)
		{
			msg.Serialize(writer);
		}

		public static global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences ReadSpawnPreferences(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences(reader);
		}
	}
}
