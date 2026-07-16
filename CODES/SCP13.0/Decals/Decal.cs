using CustomCulling;
using GameObjectPools;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Decals
{
    [RequireComponent(typeof(CullableAttachToSurface))]
    [RequireComponent(typeof(DecalProjector))]
    public class Decal : PoolObject, IPoolResettable
    {
        [SerializeField]
        private DecalProjector _projector;
        private Color _instancedColor;
        private Material _materialInstance;
        private bool _initialized;
        private Transform _cachedTransform;

        public DecalPoolType DecalPoolType;

        public CullableAttachToSurface CullableAttachToSurface { get; private set; }

        public Transform CachedTransform => _cachedTransform;

        public Material DecalMaterial
        {
            get => _projector.material;
            set => _projector.material = value;
        }

        public Vector3 DecalSize
        {
            get => _projector.size;
            set => _projector.size = value;
        }

        public Vector2 UVOffset
        {
            get => _projector.uvBias;
            set => _projector.uvBias = value;
        }

        public Vector2 UVTiling
        {
            get => _projector.uvScale;
            set => _projector.uvScale = value;
        }

        public Color InstancedColor
        {
            get => _instancedColor;
            set
            {
                if (_materialInstance == null)
                {
                    _materialInstance = new Material(_projector.material);
                    _projector.material = _materialInstance;
                }

                _instancedColor = value;
                _projector.material.SetColor("_BaseColor", value);
            }
        }


        protected virtual void Awake()
        {
            _cachedTransform = transform;
        }

        private void OnEnable()
        {
            if (_projector != null)
                _projector.enabled = true;
        }

        private void OnDisable()
        {
            if (_projector != null)
                _projector.enabled = false;
        }


        protected override void OnInstantiated()
        {
            if (_initialized) return;

            CullableAttachToSurface = GetComponent<CullableAttachToSurface>();
            _cachedTransform = transform;
            _initialized = true;
        }

        public void ResetObject()
        {
            _initialized = false;
            CullableAttachToSurface.Detach();
        }

        public void AttachToSurface(RaycastHit hitSurface)
        {
            _cachedTransform.rotation = Quaternion.LookRotation(-hitSurface.normal, Vector3.up);
            _cachedTransform.position = hitSurface.point;
            CullableAttachToSurface.Attach(hitSurface);
        }

        public void AttachToSurface(Collider collider, Vector3 point, Vector3 normal)
        {
            RaycastHit hit = new RaycastHit
            {
                point = point,
                normal = normal
            };
            AttachToSurface(hit);
        }

        public void Init(RaycastHit hitSurface)
        {
            AttachToSurface(hitSurface);
        }

        public void SetRandomRotation()
        {
            _cachedTransform.Rotate(Vector3.forward, Random.value * 360f, Space.Self);
        }
    }
}