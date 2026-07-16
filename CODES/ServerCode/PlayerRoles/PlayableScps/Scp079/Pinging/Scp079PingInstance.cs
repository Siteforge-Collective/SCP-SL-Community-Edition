namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class Scp079PingInstance : global::UnityEngine.MonoBehaviour
	{
		public static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Pinging.Scp079PingInstance> Instances = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Pinging.Scp079PingInstance>();

		[global::UnityEngine.SerializeField]
		private float _destroyTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _icon;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.SpriteRenderer _spriteRenderer;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _sizeOverDistance;

		[global::UnityEngine.SerializeField]
		private float _distanceCap;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Renderer[] _renderers;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _src;

		private global::UnityEngine.Vector3 _startPos;

		private bool _wasVisible;

		private const float MaxRangeSqr = 1500f;

		public global::UnityEngine.Sprite IconSprite
		{
			set
			{
				_spriteRenderer.sprite = value;
			}
		}

		private bool IsVisible
		{
			get
			{
				if (!ReferenceHub.TryGetLocalHub(out var hub))
				{
					return false;
				}
				if (hub.IsSCP())
				{
					return true;
				}
				if (!global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub2))
				{
					return false;
				}
				if ((_startPos - MainCameraController.CurrentCamera.position).sqrMagnitude > 1500f)
				{
					return false;
				}
				return hub2.IsSCP();
			}
		}

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Pinging.Scp079PingInstance> OnSpawned;

		private void Start()
		{
			_startPos = base.transform.position;
			global::UnityEngine.Object.Destroy(base.gameObject, _destroyTime);
			_wasVisible = true;
			UpdateVisibility();
			Instances.Add(this);
			global::PlayerRoles.PlayableScps.Scp079.Pinging.Scp079PingInstance.OnSpawned?.Invoke(this);
		}

		private void OnDestroy()
		{
			Instances.Remove(this);
		}

		private void Update()
		{
			global::UnityEngine.Transform currentCamera = MainCameraController.CurrentCamera;
			float num = global::UnityEngine.Mathf.Max(_distanceCap, global::UnityEngine.Vector3.Distance(currentCamera.position, _icon.position));
			_icon.LookAt(currentCamera);
			_icon.localScale = _sizeOverDistance.Evaluate(num) * num * global::UnityEngine.Vector3.one;
			UpdateVisibility();
		}

		private void OnValidate()
		{
			_renderers = GetComponentsInChildren<global::UnityEngine.Renderer>();
		}

		private void UpdateVisibility()
		{
			if (_wasVisible != IsVisible)
			{
				_src.mute = _wasVisible;
				_wasVisible = !_wasVisible;
				_renderers.ForEach(delegate(global::UnityEngine.Renderer x)
				{
					x.enabled = _wasVisible;
				});
			}
		}
	}
}
