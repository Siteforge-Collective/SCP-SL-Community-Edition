using System;
using System.Collections.Generic;
using GameObjectPools;
using UnityEngine;
using UserSettings;
using UserSettings.VideoSettings;

namespace Decals
{
    public class DecalPoolManager : MonoBehaviour
    {
        public static DecalPoolManager Singleton;

        [SerializeField]
        private List<Decal> _decalPrefabs;

        private List<Decal> _spawnedDecals = new List<Decal>();

        private static readonly CachedUserSetting<bool> BloodDecalsEnabled =
            new CachedUserSetting<bool>(PerformanceVideoSetting.BloodDecalsEnabled);

        private static readonly CachedUserSetting<bool> BulletDecalsEnabled =
            new CachedUserSetting<bool>(PerformanceVideoSetting.BulletDecalsEnabled);


        public bool TrySpawnDecal(DecalPoolType decalType, out Decal decal)
        {
            switch (decalType)
            {
                case DecalPoolType.Blood:
                    if (!BloodDecalsEnabled.Value) { decal = null; return false; }
                    break;

                case DecalPoolType.Bullet:
                case DecalPoolType.Buckshot:
                    if (!BulletDecalsEnabled.Value) { decal = null; return false; }
                    break;
            }

            foreach (Decal prefab in _decalPrefabs)
            {
                if (prefab.DecalPoolType != decalType) continue;

                if (PoolManager.Singleton.TryGetPoolObject(prefab.gameObject, out var poolObj)
                    && poolObj is Decal spawnedDecal)
                {
                    decal = spawnedDecal;
                    _spawnedDecals.Add(decal);
                    return true;
                }

                break;
            }

            decal = null;
            return false;
        }

        public void ReturnDecal(Decal decal)
        {
            decal.ReturnToPool(true);
        }

        private void Awake()
        {
            Singleton = this;

            foreach (Decal prefab in _decalPrefabs)
                PoolManager.Singleton.TryAddPool(prefab);

            UserSetting<bool>.AddListener(PerformanceVideoSetting.BloodDecalsEnabled, OnBloodSettingChanged);
            UserSetting<bool>.AddListener(PerformanceVideoSetting.BulletDecalsEnabled, OnBulletSettingChanged);
        }

        private void OnDestroy()
        {
            UserSetting<bool>.RemoveListener(PerformanceVideoSetting.BloodDecalsEnabled, OnBloodSettingChanged);
            UserSetting<bool>.RemoveListener(PerformanceVideoSetting.BulletDecalsEnabled, OnBulletSettingChanged);
        }

        private void OnBloodSettingChanged(bool isEnabled)
        {
            if (!isEnabled)
                RemoveConditionally(x => x.DecalPoolType == DecalPoolType.Blood);
        }

        private void OnBulletSettingChanged(bool isEnabled)
        {
            if (!isEnabled)
                RemoveConditionally(x => x.DecalPoolType == DecalPoolType.Bullet ||
                                         x.DecalPoolType == DecalPoolType.Buckshot);
        }

        private void RemoveConditionally(Func<Decal, bool> conditionToRemove)
        {
            if (_spawnedDecals.Count == 0) return;

            var remaining = new List<Decal>();

            foreach (Decal d in _spawnedDecals)
            {
                if (d == null) continue;

                if (conditionToRemove(d))
                    d.ReturnToPool(true);  
                else
                    remaining.Add(d);
            }

            _spawnedDecals = remaining;
        }
    }
}