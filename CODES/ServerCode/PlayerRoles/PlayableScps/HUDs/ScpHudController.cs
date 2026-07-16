namespace PlayerRoles.PlayableScps.HUDs
{
	public static class ScpHudController
	{
		public static global::PlayerRoles.PlayableScps.HUDs.ScpHudBase CurInstance { get; private set; }

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void InitOnLoad()
		{
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += RoleChanged;
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(TargetChanged));
		}

		private static bool ValidatePlayer(ReferenceHub hub)
		{
			if (hub.isLocalPlayer)
			{
				return true;
			}
			if (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub2))
			{
				return hub2 == hub;
			}
			if (CurInstance == null)
			{
				return false;
			}
			return CurInstance.Hub == hub;
		}

		private static void RoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prev, global::PlayerRoles.PlayerRoleBase cur)
		{
			if (!ValidatePlayer(hub))
			{
				return;
			}
			if (cur is global::PlayerRoles.PlayableScps.HUDs.IHudScp hudScp)
			{
				DestroyOld();
				SpawnNew(hudScp, hub);
			}
			else if (cur is global::PlayerRoles.Spectating.SpectatorRole)
			{
				if (CurInstance != null)
				{
					CurInstance.OnDied();
				}
			}
			else
			{
				DestroyOld();
			}
		}

		private static void TargetChanged()
		{
			if (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub))
			{
				DestroyOld();
				if (hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.HUDs.IHudScp hudScp)
				{
					SpawnNew(hudScp, hub);
				}
			}
		}

		private static void DestroyOld()
		{
			if (!(CurInstance == null))
			{
				global::UnityEngine.Object.Destroy(CurInstance.gameObject);
			}
		}

		private static void SpawnNew(global::PlayerRoles.PlayableScps.HUDs.IHudScp hudScp, ReferenceHub owner)
		{
			CurInstance = global::UnityEngine.Object.Instantiate(hudScp.HudPrefab);
			CurInstance.Init(owner);
		}
	}
}
