namespace PlayerRoles
{
	public static class PlayerRoleLoader
	{
		private static bool _loaded;

		private static global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase> _loadedRoles = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase>();

		public static global::System.Action OnLoaded;

		public static global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase> AllRoles
		{
			get
			{
				if (!_loaded)
				{
					LoadRoles();
				}
				return _loadedRoles;
			}
		}

		public static bool TryGetRoleTemplate<T>(global::PlayerRoles.RoleTypeId itemType, out T result) where T : global::PlayerRoles.PlayerRoleBase
		{
			if (!AllRoles.TryGetValue(itemType, out var value) || !(value is T val))
			{
				result = null;
				return false;
			}
			result = val;
			return true;
		}

		private static void LoadRoles()
		{
			try
			{
				_loadedRoles = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase>();
				global::PlayerRoles.PlayerRoleBase[] array = global::UnityEngine.Resources.LoadAll<global::PlayerRoles.PlayerRoleBase>("Defined Roles");
				global::System.Array.Sort(array, (global::PlayerRoles.PlayerRoleBase x, global::PlayerRoles.PlayerRoleBase y) => ((int)x.RoleTypeId).CompareTo((int)y.RoleTypeId));
				global::PlayerRoles.PlayerRoleBase[] array2 = array;
				foreach (global::PlayerRoles.PlayerRoleBase playerRoleBase in array2)
				{
					_loadedRoles[playerRoleBase.RoleTypeId] = playerRoleBase;
					global::GameObjectPools.PoolManager.Singleton.TryAddPool(playerRoleBase);
				}
				_loaded = true;
				OnLoaded?.Invoke();
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.LogError("Error while loading roles from the resources folder: " + ex.Message);
				_loaded = false;
			}
		}
	}
}
