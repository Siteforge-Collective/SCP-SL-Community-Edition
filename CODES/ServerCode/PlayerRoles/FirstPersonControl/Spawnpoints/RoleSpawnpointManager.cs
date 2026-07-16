namespace PlayerRoles.FirstPersonControl.Spawnpoints
{
	public static class RoleSpawnpointManager
	{
		private struct SpawnpointDefinition
		{
			public global::PlayerRoles.RoleTypeId[] Roles;

			public global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler[] CompatibleSpawnpoints;

			public SpawnpointDefinition(params global::PlayerRoles.RoleTypeId[] roles)
			{
				Roles = roles;
				CompatibleSpawnpoints = null;
			}

			public global::PlayerRoles.FirstPersonControl.Spawnpoints.RoleSpawnpointManager.SpawnpointDefinition SetSpawnpoints(params global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler[] spawnpoints)
			{
				CompatibleSpawnpoints = spawnpoints;
				return this;
			}
		}

		private static readonly global::PlayerRoles.FirstPersonControl.Spawnpoints.RoleSpawnpointManager.SpawnpointDefinition[] DefinedSpawnpoints = new global::PlayerRoles.FirstPersonControl.Spawnpoints.RoleSpawnpointManager.SpawnpointDefinition[1] { new global::PlayerRoles.FirstPersonControl.Spawnpoints.RoleSpawnpointManager.SpawnpointDefinition(global::PlayerRoles.RoleTypeId.ClassD).SetSpawnpoints(new global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint(new global::UnityEngine.Vector3(-6.18f, 0.91f, -4.23f), 5f, 0f, 26.26f, 0.73f, 7, 1, global::MapGeneration.RoomName.LczClassDSpawn), new global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint(new global::UnityEngine.Vector3(-6.18f, 0.91f, 4.23f), 175f, 0f, 26.26f, 0.73f, 7, 1, global::MapGeneration.RoomName.LczClassDSpawn)) };

		public static bool TryGetSpawnpointForRole(global::PlayerRoles.RoleTypeId role, out global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler spawnpoint)
		{
			bool flag = false;
			global::System.Collections.Generic.List<global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler> list = new global::System.Collections.Generic.List<global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler>();
			global::PlayerRoles.FirstPersonControl.Spawnpoints.RoleSpawnpointManager.SpawnpointDefinition[] definedSpawnpoints = DefinedSpawnpoints;
			for (int i = 0; i < definedSpawnpoints.Length; i++)
			{
				global::PlayerRoles.FirstPersonControl.Spawnpoints.RoleSpawnpointManager.SpawnpointDefinition spawnpointDefinition = definedSpawnpoints[i];
				if (spawnpointDefinition.Roles.Contains(role))
				{
					flag = true;
					list.AddRange(spawnpointDefinition.CompatibleSpawnpoints);
				}
			}
			spawnpoint = (flag ? list.RandomItem() : null);
			return flag;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += delegate(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
			{
				if (global::Mirror.NetworkServer.active && newRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole && fpcRole.SpawnpointHandler != null && fpcRole.SpawnpointHandler.TryGetSpawnpoint(out var position, out var horizontalRot) && newRole.ServerSpawnFlags.HasFlag(global::PlayerRoles.RoleSpawnFlags.UseSpawnpoint))
				{
					hub.transform.position = position;
					fpcRole.FpcModule.MouseLook.CurrentHorizontal = horizontalRot;
				}
			};
		}
	}
}
