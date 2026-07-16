using GameObjectPools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace PlayerRoles.FirstPersonControl.Thirdperson
{
	public class CharacterModel : PoolObject, IPoolResettable, IPoolSpawnable
	{
        private record RendererMaterialPair(Renderer Rend, Material Mat);

        private float _lastFade = 1f;

		private Material[] _fadeableMaterials;

		private RendererMaterialPair[] _originalMaterials;

        private static readonly int FadeHash = Shader.PropertyToID("_Fade");

        private static Material[] _copyMaterialsNonAlloc = new Material[16];

        [SerializeField]
		private Renderer[] _renderers;

		public HitboxIdentity[] Hitboxes;

		public bool IsVisible { get; private set; }

		public ReferenceHub OwnerHub { get; private set; }

        public ReadOnlySpan<Renderer> Renderers => _renderers;

        protected Transform CachedTransform { get; private set; }

        public virtual float Fade
        {
            get
            {
                return Mathf.Clamp01(_lastFade);
            }
            set
            {
                value = Mathf.Clamp01(value);
                if (Fade != value)
                {
                    int num = FadeableMaterials.Length;
                    for (int i = 0; i < num; i++)
                    {
                        _fadeableMaterials[i].SetFloat(FadeHash, value);
                    }
                    _lastFade = value;
                    this.OnFadeChanged?.Invoke();
                }
            }
        }


        public Material[] FadeableMaterials
        {
            get
            {
                if (_fadeableMaterials == null)
                {
                    InstantiateFadeableMaterials();
                }
                return _fadeableMaterials;
            }
        }

        public event Action OnVisibilityChanged;

		public event Action OnFadeChanged;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            PlayerRoleLoader.OnLoaded = (Action)Delegate.Combine(PlayerRoleLoader.OnLoaded, (Action)delegate
            {
                foreach (KeyValuePair<RoleTypeId, PlayerRoleBase> allRole in PlayerRoleLoader.AllRoles)
                {
                    if (allRole.Value is IFpcRole fpcRole && fpcRole.FpcModule.CharacterModelTemplate.TryGetComponent<CharacterModel>(out var component))
                    {
                        PoolManager.Singleton.TryAddPool(component);
                    }
                }
            });
        }

        private void InstantiateFadeableMaterials()
        {
            int count = _renderers.Length;
            if (_copyMaterialsNonAlloc.Length < count)
                _copyMaterialsNonAlloc = new Material[count];

            if (_originalMaterials == null)
                SetOriginalMaterials();

            int fadeableCount = 0;
            for (int i = 0; i < count; i++)
            {
                Renderer renderer = _renderers[i];
                Material sharedMat = renderer.sharedMaterial;

                if (sharedMat.HasFloat(FadeHash))
                {
                    Material instance = new Material(sharedMat);
                    renderer.material = instance;
                    _copyMaterialsNonAlloc[fadeableCount] = instance;
                    fadeableCount++;
                }
            }

            _fadeableMaterials = new Material[fadeableCount];
            Array.Copy(_copyMaterialsNonAlloc, _fadeableMaterials, fadeableCount);
        }

        protected virtual void Awake()
        {
            CachedTransform = base.transform;
        }

        protected virtual void OnValidate()
        {
            Hitboxes = GetComponentsInChildren<HitboxIdentity>();
            MeshRenderer[] meshRends = GetComponentsInChildren<MeshRenderer>();
            SkinnedMeshRenderer[] skinnedRends = GetComponentsInChildren<SkinnedMeshRenderer>();

            _renderers = new Renderer[meshRends.Length + skinnedRends.Length];
            Array.Copy(meshRends, _renderers, meshRends.Length);
            Array.Copy(skinnedRends, 0, _renderers, meshRends.Length, skinnedRends.Length);
        }

        public virtual void SetVisibility(bool newState)
        {
            IsVisible = newState;
            foreach (var rend in _renderers)
            {
                if (rend != null)
                    rend.enabled = newState;
            }
            OnVisibilityChanged?.Invoke();
        }

        public virtual void ResetObject()
        {
            Fade = 1f;
            foreach (var hitbox in Hitboxes)
            {
                HitboxIdentity.Instances.Remove(hitbox);
            }
        }

        public virtual void SpawnObject()
        {
            OwnerHub = ReferenceHub.GetHub(base.transform.root.gameObject);
            foreach (HitboxIdentity hitboxIdentity in Hitboxes)
            {
                HitboxIdentity.Instances.Add(hitboxIdentity);
                hitboxIdentity.SetColliders(!OwnerHub.isLocalPlayer);
            }
        }

        public void SetOriginalMaterials()
        {
            int num = _renderers.Length;
            if (_copyMaterialsNonAlloc.Length < num)
            {
                _copyMaterialsNonAlloc = new Material[num];
            }
            if (_originalMaterials == null)
            {
                _originalMaterials ??= new RendererMaterialPair[num];
            }
            for (int i = 0; i < num; i++)
            {
                Renderer renderer = _renderers[i];
                Material sharedMaterial = renderer.sharedMaterial;
                _originalMaterials[i] = new RendererMaterialPair(renderer, sharedMaterial);
            }
        }

        public void RestoreOriginalMaterials()
        {
            if (_originalMaterials != null)
            {
                for (int i = 0; i < _originalMaterials.Length; i++)
                {
                    RendererMaterialPair rendererMaterialPair = _originalMaterials[i];
                    rendererMaterialPair.Rend.sharedMaterial = rendererMaterialPair.Mat;
                }
                _fadeableMaterials = null;
            }
        }

        public virtual void OnTreadmillInitialized()
		{
		}
	}
}
