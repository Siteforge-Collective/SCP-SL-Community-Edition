using GameObjectPools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.Spectating
{
    public class SpectatableListManager : MonoBehaviour
    {
        [SerializeField]
        private SpectatableListElementDefinition[] _definedPairs;

        [SerializeField]
        private float _targetHeight;

        [SerializeField]
        private VerticalLayoutGroup _layoutGroup;

        private int _lastTargetId;

        private readonly List<SpectatableListSpawnedElement> _spawnedTargets = new List<SpectatableListSpawnedElement>();

        private static bool _initialized;
        private static KeyCode _nextKey;
        private static KeyCode _prevKey;

        private static readonly Dictionary<SpectatableListElementType, SpectatableListElementDefinition> Definitions = new Dictionary<SpectatableListElementType, SpectatableListElementDefinition>();

        private void OnEnable()
        {
            SpectatorTargetTracker.OnTargetChanged += RefreshSize;
            SpectatableModuleBase.OnAdded += AddTarget;
            SpectatableModuleBase.OnRemoved += RemoveTarget;

            if (!_initialized)
            {
                foreach (var pair in _definedPairs)
                {
                    Definitions[pair.Type] = pair;
                    PoolManager.Singleton.TryAddPool(pair.FullSize);
                    PoolManager.Singleton.TryAddPool(pair.Compact);
                }
                RefreshKeybinds();
                _initialized = true;
            }
            RefreshAllTargets();
        }

        private void OnDisable()
        {
            SpectatorTargetTracker.OnTargetChanged -= RefreshSize;
            SpectatableModuleBase.OnAdded -= AddTarget;
            SpectatableModuleBase.OnRemoved -= RemoveTarget;
        }

        private void LateUpdate()
        {
            if (_spawnedTargets.Count == 0) return;

            int newTargetId = _lastTargetId;

            if (Input.GetKeyDown(_prevKey)) newTargetId--;
            if (Input.GetKeyDown(_nextKey)) newTargetId++;

            if (newTargetId != _lastTargetId)
            {
                if (!ToggleableMenus.ToggleableMenuController.AnyEnabled)
                {
                    newTargetId = (newTargetId + _spawnedTargets.Count) % _spawnedTargets.Count;

                    _lastTargetId = newTargetId;
                    SpectatorTargetTracker.CurrentTarget = _spawnedTargets[_lastTargetId].Target;
                }
            }
        }

        private void AddTarget(SpectatableModuleBase target)
        {
            int priority = GetOrderPriority(target.MainRole);
            int insertIndex = _spawnedTargets.Count;

            for (int i = 0; i < _spawnedTargets.Count; i++)
            {
                if (_spawnedTargets[i].Priority > priority)
                {
                    insertIndex = i;
                    break;
                }
            }

            if (Definitions.TryGetValue(target.ListElementType, out var def) &&
                def.TryGetFromPools(transform, out var full, out var compact))
            {
                SpectatableListSpawnedElement element = new SpectatableListSpawnedElement
                {
                    Priority = priority,
                    Compact = compact,
                    FullSize = full,
                    Target = target
                };

                SetupNewTarget(element.Compact, target, insertIndex * 2);
                SetupNewTarget(element.FullSize, target, insertIndex * 2 + 1);

                _spawnedTargets.Insert(insertIndex, element);
                RefreshSize();
            }
        }

        private void RemoveTarget(SpectatableModuleBase target)
        {
            for (int i = 0; i < _spawnedTargets.Count; i++)
            {
                if (_spawnedTargets[i].Target == target)
                {
                    _spawnedTargets[i].ReturnToPool();
                    _spawnedTargets.RemoveAt(i);
                    break;
                }
            }
            RefreshSize();
        }

        private void RefreshAllTargets()
        {
            foreach (var spawned in _spawnedTargets)
            {
                spawned.ReturnToPool();
            }
            _spawnedTargets.Clear();

            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                if (hub.roleManager.CurrentRole is ISpectatableRole spectatable)
                {
                    AddTarget(spectatable.SpectatorModule);
                }
            }
        }

        private void SetupNewTarget(SpectatableListElementBase element, SpectatableModuleBase module, int order)
        {
            element.transform.localScale = Vector3.one;
            element.transform.localPosition = Vector3.zero;
            element.transform.localRotation = Quaternion.identity;
            element.transform.SetSiblingIndex(order);

            element.Index = order;
            element.Target = module;
        }

        private void RefreshSize()
        {
            int count = _spawnedTargets.Count;
            if (count == 0) return;

            float compactTotalHeight = _layoutGroup.spacing * (count - 1);
            float fullTotalHeight = compactTotalHeight;

            for (int i = 0; i < count; i++)
            {
                compactTotalHeight += _spawnedTargets[i].Compact.Height;
                fullTotalHeight += _spawnedTargets[i].FullSize.Height;

                if (_spawnedTargets[i].Target == SpectatorTargetTracker.CurrentTarget)
                    _lastTargetId = i;
            }

            float lerp = Mathf.InverseLerp(compactTotalHeight, fullTotalHeight, _targetHeight);

            int fullVisibleCount = (lerp < 1f) ? Mathf.Clamp(Mathf.FloorToInt(lerp * count) - 1, 0, 12) : count;
            if (fullVisibleCount % 2 != 0 && fullVisibleCount != count) fullVisibleCount--;

            int start = Mathf.Clamp(_lastTargetId - fullVisibleCount / 2, 0, Mathf.Max(0, count - fullVisibleCount));
            int end = Mathf.Min(start + fullVisibleCount, count - 1);

            for (int j = 0; j < count; j++)
            {
                bool isFull = (j >= start && j <= end);
                _spawnedTargets[j].FullSize.gameObject.SetActive(isFull);
                _spawnedTargets[j].Compact.gameObject.SetActive(!isFull);
            }
        }

        private static void RefreshKeybinds()
        {
            _nextKey = NewInput.GetKey(ActionName.Shoot);
            _prevKey = NewInput.GetKey(ActionName.Zoom);
        }

        private static int GetOrderPriority(PlayerRoleBase prb)
        {
            int factionPart = (int)prb.Team.GetFaction() * 65535;
            int teamPart = (int)prb.Team * 255;
            return factionPart + teamPart + (int)prb.RoleTypeId;
        }
    }
}