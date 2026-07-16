namespace PlayerRoles
{
	public class PlayerRoleManager : global::Mirror.NetworkBehaviour
	{
		public delegate void ServerRoleSet(ReferenceHub userHub, global::PlayerRoles.RoleTypeId newRole, global::PlayerRoles.RoleChangeReason reason);

		public delegate void RoleChanged(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole);

		public readonly global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.RoleTypeId> PreviouslySentRole = new global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.RoleTypeId>();

		private ReferenceHub _hub;

		private bool _hubSet;

		private bool _anySet;

		private bool _sendNextFrame;

		private global::PlayerRoles.PlayerRoleBase _curRole;

		private const global::PlayerRoles.RoleTypeId DefaultRole = global::PlayerRoles.RoleTypeId.None;

		public global::PlayerRoles.PlayerRoleBase CurrentRole
		{
			get
			{
				if (!_anySet)
				{
					InitializeNewRole(global::PlayerRoles.RoleTypeId.None, global::PlayerRoles.RoleChangeReason.None);
				}
				return _curRole;
			}
			set
			{
				_curRole = value;
				_anySet = true;
			}
		}

		private ReferenceHub Hub
		{
			get
			{
				if (!_hubSet && ReferenceHub.TryGetHub(base.gameObject, out _hub))
				{
					_hubSet = true;
				}
				return _hub;
			}
		}

		public static event global::PlayerRoles.PlayerRoleManager.ServerRoleSet OnServerRoleSet;

		public static event global::PlayerRoles.PlayerRoleManager.RoleChanged OnRoleChanged;

		private void Update()
		{
			if (!global::Mirror.NetworkServer.active || !_sendNextFrame)
			{
				return;
			}
			_sendNextFrame = false;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.isLocalPlayer)
				{
					continue;
				}
				global::PlayerRoles.RoleTypeId roleTypeId = CurrentRole.RoleTypeId;
				if (CurrentRole is global::PlayerRoles.IObfuscatedRole obfuscatedRole)
				{
					roleTypeId = obfuscatedRole.GetRoleForUser(allHub);
					if (PreviouslySentRole.TryGetValue(allHub.netId, out var value) && value == roleTypeId)
					{
						continue;
					}
				}
				allHub.connectionToClient.Send(new global::PlayerRoles.RoleSyncInfo(Hub, roleTypeId, allHub));
				PreviouslySentRole[allHub.netId] = roleTypeId;
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			InitializeNewRole(global::PlayerRoles.RoleTypeId.None, global::PlayerRoles.RoleChangeReason.Destroyed);
		}

		private global::PlayerRoles.PlayerRoleBase GetRoleBase(global::PlayerRoles.RoleTypeId targetId)
		{
			if (!global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayerRoleBase>(targetId, out var result))
			{
				global::UnityEngine.Debug.LogError($"Role #{targetId} could not be found. Player with ID {Hub.PlayerId} will receive the default role ({(global::PlayerRoles.RoleTypeId.None)}).");
				if (!global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayerRoleBase>(global::PlayerRoles.RoleTypeId.None, out result))
				{
					throw new global::System.NotImplementedException("Role change failed. Default role is not correctly implemented.");
				}
			}
			if (!global::GameObjectPools.PoolManager.Singleton.TryGetPoolObject(result.gameObject, out var poolObject) || !(poolObject is global::PlayerRoles.PlayerRoleBase result2))
			{
				throw new global::System.InvalidOperationException($"Role {targetId} failed to initialize, pool was not found or dequed object was of incorrect type.");
			}
			return result2;
		}

		public void InitializeNewRole(global::PlayerRoles.RoleTypeId targetId, global::PlayerRoles.RoleChangeReason reason, global::PlayerRoles.RoleSpawnFlags spawnFlags = global::PlayerRoles.RoleSpawnFlags.All, global::Mirror.NetworkReader data = null)
		{
			global::PlayerRoles.PlayerRoleBase playerRoleBase;
			bool flag;
			if (_anySet)
			{
				playerRoleBase = CurrentRole;
				playerRoleBase.DisableRole(targetId);
				flag = true;
			}
			else
			{
				playerRoleBase = null;
				flag = false;
			}
			if (reason == global::PlayerRoles.RoleChangeReason.Destroyed && targetId == global::PlayerRoles.RoleTypeId.None)
			{
				playerRoleBase = null;
				flag = false;
				return;
			}
			global::PlayerRoles.PlayerRoleBase roleBase = GetRoleBase(targetId);
			global::UnityEngine.Transform obj = roleBase.transform;
			obj.parent = base.transform;
			obj.localPosition = global::UnityEngine.Vector3.zero;
			obj.localRotation = global::UnityEngine.Quaternion.identity;
			CurrentRole = roleBase;
			roleBase.Init(Hub, reason, spawnFlags);
			roleBase.SpawnPoolObject();
			if (CurrentRole is global::PlayerRoles.SpawnData.ISpawnDataReader spawnDataReader && data != null)
			{
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerSpawn, Hub, CurrentRole.RoleTypeId))
				{
					spawnDataReader.ReadSpawnData(data);
				}
			}
			else
			{
				global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerSpawn, Hub, CurrentRole.RoleTypeId);
			}
			if (flag)
			{
				global::PlayerRoles.PlayerRoleManager.OnRoleChanged?.Invoke(Hub, playerRoleBase, CurrentRole);
			}
			global::CustomPlayerEffects.SpawnProtected.TryGiveProtection(Hub);
		}

		[global::Mirror.Server]
		public void ServerSetRole(global::PlayerRoles.RoleTypeId newRole, global::PlayerRoles.RoleChangeReason reason, global::PlayerRoles.RoleSpawnFlags spawnFlags = global::PlayerRoles.RoleSpawnFlags.All)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void PlayerRoles.PlayerRoleManager::ServerSetRole(PlayerRoles.RoleTypeId,PlayerRoles.RoleChangeReason,PlayerRoles.RoleSpawnFlags)' called when server was not active");
				return;
			}
			global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerChangeRole, Hub, CurrentRole, newRole, reason);
			global::PlayerRoles.PlayerRoleManager.OnServerRoleSet?.Invoke(Hub, newRole, reason);
			InitializeNewRole(newRole, reason, spawnFlags);
			_sendNextFrame = true;
		}

		private void MirrorProcessed()
		{
		}
	}
}
