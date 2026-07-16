using System;
using System.Collections.Generic;
using Mirror;
using PlayerStatsSystem;
using UnityEngine;
using UserSettings;
using UserSettings.VideoSettings;
using Utils.NonAllocLINQ;

namespace PlayerRoles.Ragdolls
{
    public static class RagdollManager
    {
        public static readonly HashSet<BasicRagdoll> AllRagdolls = new HashSet<BasicRagdoll>();

        private static readonly HashSet<NetworkIdentity> CachedRagdollPrefabs = new HashSet<NetworkIdentity>();

        private static bool _prefabsCacheSet;

        public static int FreezeTime { get; set; }

        private static HashSet<NetworkIdentity> AllRagdollPrefabs
        {
            get
            {
                if (_prefabsCacheSet)
                {
                    return CachedRagdollPrefabs;
                }
                foreach (KeyValuePair<RoleTypeId, PlayerRoleBase> allRole in PlayerRoleLoader.AllRoles)
                {
                    if (allRole.Value is IRagdollRole ragdollRole
                        && ragdollRole.Ragdoll != null
                        && ragdollRole.Ragdoll.netIdentity != null)
                    {
                        CachedRagdollPrefabs.Add(ragdollRole.Ragdoll.netIdentity);
                    }
                }
                _prefabsCacheSet = true;
                return CachedRagdollPrefabs;
            }
        }

        public static event Action<ReferenceHub, BasicRagdoll> ServerOnRagdollCreated;

        public static event Action<BasicRagdoll> OnRagdollSpawned;

        public static event Action<BasicRagdoll> OnRagdollRemoved;

        internal static void OnSpawnedRagdoll(BasicRagdoll ragdoll)
        {
            AllRagdolls.Add(ragdoll);
            RagdollManager.OnRagdollSpawned?.Invoke(ragdoll);
        }

        internal static void OnRemovedRagdoll(BasicRagdoll ragdoll)
        {
            AllRagdolls.Remove(ragdoll);
            RagdollManager.OnRagdollRemoved?.Invoke(ragdoll);
        }

        public static BasicRagdoll ServerSpawnRagdoll(ReferenceHub owner, DamageHandlerBase handler)
        {
            if (!NetworkServer.active || owner == null)
            {
                return null;
            }
            if (!(owner.roleManager.CurrentRole is IRagdollRole ragdollRole))
            {
                return null;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate(ragdollRole.Ragdoll.gameObject);
            if (gameObject.TryGetComponent<BasicRagdoll>(out var component))
            {
                Transform transform = ragdollRole.Ragdoll.transform;
                component.Info = new RagdollData(owner, handler, transform.localPosition, transform.localRotation);
                RagdollManager.ServerOnRagdollCreated?.Invoke(owner, component);
            }
            else
            {
                component = null;
            }
            NetworkServer.Spawn(gameObject);
            return component;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += delegate
            {
                AllRagdollPrefabs.ForEach(delegate (NetworkIdentity x)
                {
                    if (x == null || x.gameObject == null)
                        return;
                    NetworkClient.prefabs[x.assetId] = x.gameObject;
                });
            };
            UserSetting<bool>.AddListener(PerformanceVideoSetting.RagdollFreeze, delegate
            {
                UpdateCleanupPrefs();
            });
            UserSetting<float>.AddListener(PerformanceVideoSetting.RagdollFreeze, delegate
            {
                UpdateCleanupPrefs();
            });
            UpdateCleanupPrefs();
        }

        private static void UpdateCleanupPrefs()
        {
            bool num = UserSetting<bool>.Get(PerformanceVideoSetting.RagdollFreeze);
            int num2 = Mathf.RoundToInt(UserSetting<float>.Get(PerformanceVideoSetting.RagdollFreeze));
            FreezeTime = (num ? num2 : int.MaxValue);
        }
    }
}
