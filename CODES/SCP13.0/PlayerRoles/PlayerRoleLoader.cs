using System;
using System.Collections.Generic;
using GameObjectPools;
using UnityEngine;

namespace PlayerRoles
{
    public static class PlayerRoleLoader
    {
        private static bool _loaded;

        private static Dictionary<RoleTypeId, PlayerRoleBase> _loadedRoles = new Dictionary<RoleTypeId, PlayerRoleBase>();

        public static Action OnLoaded;

        public static Dictionary<RoleTypeId, PlayerRoleBase> AllRoles
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

        public static bool TryGetRoleTemplate<T>(RoleTypeId roleType, out T result) where T : PlayerRoleBase
        {
            if (!AllRoles.TryGetValue(roleType, out var value) || !(value is T val))
            {
                result = null;
                return false;
            }
            result = val;
            return true;
        }

        private static void LoadRoles()
        {
            _loadedRoles = new Dictionary<RoleTypeId, PlayerRoleBase>();
            PlayerRoleBase[] array = Resources.LoadAll<PlayerRoleBase>("Defined Roles");
            Array.Sort(array, (PlayerRoleBase x, PlayerRoleBase y) => ((int)x.RoleTypeId).CompareTo((int)y.RoleTypeId));
            PlayerRoleBase[] array2 = array;
            foreach (PlayerRoleBase playerRoleBase in array2)
            {
                _loadedRoles[playerRoleBase.RoleTypeId] = playerRoleBase;
                PoolManager.Singleton.TryAddPool(playerRoleBase);
            }
            _loaded = true;
            OnLoaded?.Invoke();
        }
    }
}
