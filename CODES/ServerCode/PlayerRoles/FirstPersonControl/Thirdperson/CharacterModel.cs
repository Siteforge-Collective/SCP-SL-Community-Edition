namespace PlayerRoles.FirstPersonControl.Thirdperson
{
	public class CharacterModel : global::GameObjectPools.PoolObject, global::GameObjectPools.IPoolResettable, global::GameObjectPools.IPoolSpawnable
	{
		private float _lastFade = 1f;

		private bool _transparencyGenerated;

		private global::UnityEngine.Material[] _originalMaterials;

		private global::UnityEngine.Material[] _transparentMaterials;

		private int _renderersLength;

		private static readonly int DisintegrateId = global::UnityEngine.Shader.PropertyToID("_Disintegrate");

		private static readonly int BurnRampId = global::UnityEngine.Shader.PropertyToID("_BurnRamp");

		private static readonly int DissolveId = global::UnityEngine.Shader.PropertyToID("_DisolveGuide");

		private static readonly global::UnityEngine.AnimationCurve CorrectionCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe(0f, 0f, 0.077f, 0.077f, 0.226f, 0.815f), new global::UnityEngine.Keyframe(0.1f, 0.65f, 0.22f, 0.22f, 1f, 0.209f), new global::UnityEngine.Keyframe(0.5f, 0.7f, 0.031f, 0.031f, 0.425f, 0.485f), new global::UnityEngine.Keyframe(0.95f, 0.8f, 0.403f, 0.403f, 0.393f, 1f), new global::UnityEngine.Keyframe(1f, 1f, 2.762f, 2.762f, 0.325f, 0f));

		[global::UnityEngine.Header("Other")]
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _fadeMaterial;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Renderer[] _renderers;

		public HitboxIdentity[] Hitboxes;

		public ReferenceHub OwnerHub { get; private set; }

		protected global::UnityEngine.Transform CachedTransform { get; private set; }

		public virtual float Fade
		{
			get
			{
				return global::UnityEngine.Mathf.Clamp01(_lastFade);
			}
			set
			{
				value = global::UnityEngine.Mathf.Clamp01(value);
				if (Fade == value)
				{
					return;
				}
				bool flag = _lastFade < 1f;
				_lastFade = value;
				if (flag && Fade == 1f)
				{
					SetShader(transparent: false);
					return;
				}
				if (!flag)
				{
					if (!_transparencyGenerated)
					{
						GenerateTransparentMaterials();
					}
					SetShader(transparent: true);
				}
				SetTransparency(1f - CorrectionCurve.Evaluate(Fade));
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerRoles.PlayerRoleLoader.OnLoaded = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.PlayerRoleLoader.OnLoaded, (global::System.Action)delegate
			{
				foreach (global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase> allRole in global::PlayerRoles.PlayerRoleLoader.AllRoles)
				{
					if (allRole.Value is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole && fpcRole.FpcModule.CharacterModelTemplate.TryGetComponent<global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel>(out var component))
					{
						global::GameObjectPools.PoolManager.Singleton.TryAddPool(component);
					}
				}
			});
		}

		private void SetShader(bool transparent)
		{
			global::UnityEngine.Material[] array = (transparent ? _transparentMaterials : _originalMaterials);
			for (int i = 0; i < _renderersLength; i++)
			{
				_renderers[i].sharedMaterial = array[i];
			}
		}

		private void GenerateTransparentMaterials()
		{
			_renderersLength = _renderers.Length;
			_originalMaterials = new global::UnityEngine.Material[_renderersLength];
			_transparentMaterials = new global::UnityEngine.Material[_renderersLength];
			for (int i = 0; i < _renderersLength; i++)
			{
				global::UnityEngine.Material sharedMaterial = _renderers[i].sharedMaterial;
				global::UnityEngine.Material material = new global::UnityEngine.Material(sharedMaterial)
				{
					shader = _fadeMaterial.shader
				};
				material.SetTexture(BurnRampId, _fadeMaterial.GetTexture(BurnRampId));
				material.SetTexture(DissolveId, _fadeMaterial.GetTexture(DissolveId));
				_originalMaterials[i] = sharedMaterial;
				_transparentMaterials[i] = material;
			}
			_transparencyGenerated = true;
		}

		private void SetTransparency(float t)
		{
			for (int i = 0; i < _renderersLength; i++)
			{
				_transparentMaterials[i].SetFloat(DisintegrateId, t);
			}
		}

		protected virtual void Awake()
		{
			CachedTransform = base.transform;
		}

		protected virtual void OnValidate()
		{
			Hitboxes = GetComponentsInChildren<HitboxIdentity>();
			global::UnityEngine.MeshRenderer[] componentsInChildren = GetComponentsInChildren<global::UnityEngine.MeshRenderer>();
			global::UnityEngine.SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<global::UnityEngine.SkinnedMeshRenderer>();
			int num = componentsInChildren.Length;
			int num2 = componentsInChildren2.Length;
			_renderers = new global::UnityEngine.Renderer[num + num2];
			global::System.Array.Copy(componentsInChildren, _renderers, num);
			global::System.Array.Copy(componentsInChildren2, 0, _renderers, num, num2);
		}

		public virtual void SetVisibility(bool newState)
		{
		}

		public virtual void ResetObject()
		{
			Fade = 1f;
			HitboxIdentity[] hitboxes = Hitboxes;
			foreach (HitboxIdentity item in hitboxes)
			{
				HitboxIdentity.Instances.Remove(item);
			}
		}

		public virtual void SpawnObject()
		{
			OwnerHub = ReferenceHub.GetHub(base.transform.root.gameObject);
			HitboxIdentity[] hitboxes = Hitboxes;
			foreach (HitboxIdentity hitboxIdentity in hitboxes)
			{
				HitboxIdentity.Instances.Add(hitboxIdentity);
				hitboxIdentity.SetColliders(!OwnerHub.isLocalPlayer);
			}
		}
	}
}
