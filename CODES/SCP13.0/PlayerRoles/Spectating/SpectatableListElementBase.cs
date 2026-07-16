using GameObjectPools;
using UnityEngine;
using System;

namespace PlayerRoles.Spectating
{
    public class SpectatableListElementBase : PoolObject
    {
        private RectTransform _cachedTransform;

        protected SpectatableModuleBase _target;

        private bool _transformCacheSet;

        protected RectTransform CachedRectTransform
        {
            get
            {
                if (!_transformCacheSet)
                {
                    if (!base.TryGetComponent<RectTransform>(out _cachedTransform))
                    {
                        throw new InvalidOperationException($"SpectatableListElementBase of name '{base.name}' does not have a rect transform!");
                    }
                    _transformCacheSet = true;
                }
                return _cachedTransform;
            }
        }

        public SpectatableModuleBase Target
        {
            get => _target;
            internal set
            {
                if (value != _target)
                {
                    SpectatableModuleBase prevTarget = _target;
                    _target = value;
                    OnTargetChanged(prevTarget, value);
                }
            }
        }

        public int Index { get; internal set; }

        public float Height
        {
            get
            {
                return CachedRectTransform.sizeDelta.y;
            }
        }

        public bool IsCurrent
        {
            get
            {
                return _target == SpectatorTargetTracker.CurrentTarget;
            }
        }

        protected virtual void OnTargetChanged(SpectatableModuleBase prevTarget, SpectatableModuleBase newTarget)
        {
        }

        public void BeginSpectating()
        {
            if (_target != null)
            {
                SpectatorTargetTracker.CurrentTarget = _target;
            }
        }
    }
}