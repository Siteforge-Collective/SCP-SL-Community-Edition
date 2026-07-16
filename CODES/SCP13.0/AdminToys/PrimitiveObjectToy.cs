using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

namespace AdminToys
{
    public class PrimitiveObjectToy : AdminToyBase
    {
        private static readonly Dictionary<Color, Material> CachedMaterials = new Dictionary<Color, Material>();

        private static readonly Dictionary<PrimitiveType, Mesh> PrimitiveTypeToMesh = new Dictionary<PrimitiveType, Mesh>(6);

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField]
        private Material _regularMatTemplate;

        [SerializeField]
        private Material _transparentMatTemplate;

        [SerializeField]
        private MeshFilter _filter;

        [SerializeField]
        private MeshRenderer _renderer;

        private Collider _collider;

        [SyncVar(hook = nameof(SetPrimitive))]
        public PrimitiveType PrimitiveType;

        [SyncVar(hook = nameof(SetColor))]
        public Color MaterialColor;

        [SyncVar(hook = nameof(SetFlags))]
        public PrimitiveFlags PrimitiveFlags = PrimitiveFlags.Collidable | PrimitiveFlags.Visible;

        public override string CommandName => "PrimitiveObject";

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += delegate
            {
                CachedMaterials.Clear();
                PrimitiveTypeToMesh.Clear();
                PrimitiveType[] values = EnumUtils<PrimitiveType>.Values;
                foreach (PrimitiveType primitiveType in values)
                {
                    GameObject obj = GameObject.CreatePrimitive(primitiveType);
                    Mesh sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
                    PrimitiveTypeToMesh.Add(primitiveType, sharedMesh);
                    UnityEngine.Object.Destroy(obj);
                }
            };
        }

        public override void OnSpawned(ReferenceHub admin, ArraySegment<string> arguments)
        {
            string[] array = arguments.Array;
            PrimitiveType = ((array.Length > 2 && Enum.TryParse<PrimitiveType>(array[2], ignoreCase: true, out var result)) ? result : PrimitiveType.Sphere);
            MaterialColor = ((array.Length > 3 && ColorUtility.TryParseHtmlString(array[3], out var color)) ? color : Color.gray);
            float result2;
            float num = ((array.Length > 4 && float.TryParse(array[4], out result2)) ? result2 : 1f);
            PrimitiveFlags = ((array.Length > 5 && Enum.TryParse<PrimitiveFlags>(array[5], ignoreCase: true, out var result3)) ? result3 : ((PrimitiveFlags)255));
            base.transform.SetPositionAndRotation(admin.PlayerCameraReference.position, admin.PlayerCameraReference.rotation);
            base.transform.localScale = Vector3.one * num;
            base.Scale = base.transform.localScale;
            base.OnSpawned(admin, arguments);
        }

        private void Start()
        {
            SetPrimitive(PrimitiveType.Sphere, PrimitiveType);
        }

        private void SetPrimitive(PrimitiveType _, PrimitiveType newPrim)
        {
            _filter.sharedMesh = PrimitiveTypeToMesh[newPrim];
            if (_collider != null)
            {
                UnityEngine.Object.Destroy(_collider);
            }
            if (newPrim == PrimitiveType.Cube)
            {
                _collider = base.gameObject.AddComponent<BoxCollider>();
            }
            else
            {
                MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
                bool convex = newPrim != PrimitiveType.Plane && newPrim != PrimitiveType.Quad;
                meshCollider.convex = convex;
                _collider = meshCollider;
            }
            SetColor(Color.clear, MaterialColor);
            SetFlags(PrimitiveFlags.None, PrimitiveFlags);
        }

        private void SetColor(Color _, Color newColor)
        {
            _renderer.sharedMaterial = GetMaterialFromColor(newColor);
        }

        private Material GetMaterialFromColor(Color color)
        {
            if (CachedMaterials.TryGetValue(color, out var value))
            {
                return value;
            }
            value = ((color.a >= 1f) ? new Material(_regularMatTemplate) : new Material(_transparentMatTemplate));
            value.SetColor(BaseColor, color);
            CachedMaterials.Add(color, value);
            return value;
        }

        private void SetFlags(PrimitiveFlags _, PrimitiveFlags newPrimitiveFlags)
        {
            if (_collider != null)
            {
                _collider.enabled = newPrimitiveFlags.HasFlag(PrimitiveFlags.Collidable);
            }
            _renderer.enabled = newPrimitiveFlags.HasFlag(PrimitiveFlags.Visible);
        }
    }
}
