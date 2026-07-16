using System.Collections.Generic;
using UnityEngine;

namespace CustomCulling
{
    public class CullableAttachToSurface : CullableBase
    {
        private Transform _cachedTransform;
        private CullableBase _linkedCullableBase;

        public CullableBase LinkedCullableBase => _linkedCullableBase;

        protected override void OnAwake()
        {
            base.OnAwake();
            _cachedTransform = transform;
        }

        public void Attach()
        {
            var cullable = GetComponentInParent<CullableBase>();
            Attach(cullable);
        }

        public void Attach(RaycastHit hit)
        {
            if (hit.collider == null) return;
            var cullable = hit.collider.GetComponentInParent<CullableBase>();
            Attach(cullable);
        }

        public void Attach(CullableBase cullableBase)
        {
            if (cullableBase == null)
            {
                var roomId = MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(
                    _cachedTransform.position);
                if (roomId != null)
                    cullableBase = roomId.GetComponent<CullableRoom>();
            }

            if (cullableBase == null) return;

            if (_linkedCullableBase != null)
                Detach();

            _linkedCullableBase = cullableBase;

            _linkedCullableBase.lights.AddRange(lights);
            _linkedCullableBase.renderers.AddRange(renderers);
            _linkedCullableBase.otherBehaviours.AddRange(otherBehaviours);
            _linkedCullableBase.Decals.AddRange(Decals);

            CullEnabled = _linkedCullableBase.CullEnabled;
            ForceUpdateBehaviours(CullEnabled);
        }

        public void Detach()
        {
            if (_linkedCullableBase == null) return;

            RemoveListFromList(_linkedCullableBase.lights, lights);
            RemoveListFromList(_linkedCullableBase.renderers, renderers);
            RemoveListFromList(_linkedCullableBase.otherBehaviours, otherBehaviours);
            RemoveListFromList(_linkedCullableBase.Decals, Decals);

            _linkedCullableBase = null;
        }

        private void RemoveListFromList<T>(List<T> listToRemoveFrom, List<T> listToRemove)
        {
            if (listToRemove.Count == 0) return;
            foreach (var item in listToRemove)
                listToRemoveFrom.Remove(item);
        }
    }
}