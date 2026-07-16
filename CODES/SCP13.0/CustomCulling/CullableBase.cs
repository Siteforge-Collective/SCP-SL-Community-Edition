using System.Collections.Generic;
using Decals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CustomCulling
{
    public class CullableBase : MonoBehaviour
    {
        public bool StaticObject;

        public List<Renderer> renderers = new List<Renderer>();
        public List<Behaviour> otherBehaviours = new List<Behaviour>();
        public List<Light> lights = new List<Light>();
        public List<Decal> Decals = new List<Decal>();

        private bool _isSerialized;
        protected bool _currentlyEnabled = true;

        public List<Renderer> Renderers => renderers;
        public List<Behaviour> OtherBehaviours => otherBehaviours;
        public List<Light> Lights => lights;

        public virtual bool CullEnabled
        {
            get => _currentlyEnabled;
            set
            {
                if (_currentlyEnabled == value) return;
                _currentlyEnabled = value;
                ForceUpdateBehaviours(value);
            }
        }

        protected virtual void Awake()
        {
            if (!_isSerialized)
            {
                GetChildren();
            }

            ForceUpdateBehaviours(_currentlyEnabled);
            OnAwake();
        }

        protected virtual void OnAwake() { }

        protected virtual void GetChildren()
        {
            lights = new List<Light>(GetComponentsInChildrenWithoutCullableBase<Light>());
            renderers = new List<Renderer>(GetComponentsInChildrenWithoutCullableBase<Renderer>());

            otherBehaviours = new List<Behaviour>(GetComponentsInChildrenWithoutCullableBase<Canvas>());
            otherBehaviours.AddRange(GetComponentsInChildrenWithoutCullableBase<CanvasScaler>());
            otherBehaviours.AddRange(GetComponentsInChildrenWithoutCullableBase<TextMeshPro>());
            otherBehaviours.AddRange(GetComponentsInChildrenWithoutCullableBase<ReflectionProbe>());

            Decals = new List<Decal>(GetComponentsInChildrenWithoutCullableBase<Decal>());

            for (int i = renderers.Count - 1; i >= 0; i--)
            {
                if (renderers[i] != null && renderers[i].gameObject.name.StartsWith("RID"))
                {
                    renderers.RemoveAt(i);
                }
            }
        }

        public virtual void UpdateBehaviours()
        {
            if (_currentlyEnabled != CullEnabled)
            {
                ForceUpdateBehaviours(CullEnabled);
            }
        }

        protected virtual void ForceUpdateBehaviours(bool enabled)
        {
            _currentlyEnabled = enabled;

            foreach (var b in otherBehaviours) if (b) b.enabled = enabled;
            otherBehaviours.RemoveAll(x => x == null);

            foreach (var l in lights) if (l) l.enabled = enabled;
            lights.RemoveAll(x => x == null);

            foreach (var r in renderers) if (r) r.enabled = enabled;
            renderers.RemoveAll(x => x == null);

            foreach (var d in Decals) if (d) d.enabled = enabled;
            Decals.RemoveAll(x => x == null);
        }

        protected List<T> GetComponentsInChildrenWithoutCullableBase<T>() where T : Component
        {
            var result = new List<T>();
            GetComponentsInChildrenWithoutCullableBase(result, transform, false);
            return result;
        }

        private void GetComponentsInChildrenWithoutCullableBase<T>(List<T> list, Transform trans, bool skipCheck) where T : Component
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);
                if (!skipCheck && child.GetComponent<CullableBase>() != null) continue;

                var component = child.GetComponent<T>();
                if (component != null) list.Add(component);

                GetComponentsInChildrenWithoutCullableBase(list, child, false);
            }
        }
    }
}