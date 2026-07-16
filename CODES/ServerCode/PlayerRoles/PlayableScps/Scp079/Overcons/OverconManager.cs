namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class OverconManager : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconRendererBase[] _renderers;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		private const string OverconLayer = "Interface079";

		private const float Range = 200f;

		private static int _raycastMask;

		private static int RaycastMask
		{
			get
			{
				if (_raycastMask == 0)
				{
					_raycastMask = global::UnityEngine.LayerMask.GetMask("Interface079");
				}
				return _raycastMask;
			}
		}

		private static global::UnityEngine.Camera RaycastCam => global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079Hud.RaycastCamera;

		public global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase HighlightedOvercon { get; private set; }

		public static global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconManager Singleton { get; private set; }

		private void OnCameraChanged()
		{
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = _curCamSync.CurrentCamera;
			global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconRendererBase[] renderers = _renderers;
			for (int i = 0; i < renderers.Length; i++)
			{
				renderers[i].SpawnOvercons(currentCamera);
			}
		}

		private void Start()
		{
			Singleton = this;
			global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079Hud.Instance.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
			_curCamSync.OnCameraChanged += OnCameraChanged;
			OnCameraChanged();
		}

		private void OnDestroy()
		{
			global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase[] array = global::System.Linq.Enumerable.ToArray(global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase.ActiveInstances);
			foreach (global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase overconBase in array)
			{
				if (!(overconBase == null))
				{
					global::UnityEngine.Object.Destroy(overconBase.gameObject);
				}
			}
			global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase.ActiveInstances.Clear();
			if (_curCamSync != null)
			{
				_curCamSync.OnCameraChanged -= OnCameraChanged;
			}
		}

		private void Update()
		{
			if (!global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive)
			{
				return;
			}
			global::UnityEngine.RaycastHit hitInfo;
			global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase overconBase = (global::UnityEngine.Physics.Raycast(RaycastCam.ScreenPointToRay(global::UnityEngine.Input.mousePosition), out hitInfo, 200f, RaycastMask) ? hitInfo.collider.GetComponent<global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase>() : null);
			if (!(overconBase == HighlightedOvercon))
			{
				if (HighlightedOvercon != null)
				{
					HighlightedOvercon.IsHighlighted = false;
				}
				if (overconBase != null)
				{
					overconBase.IsHighlighted = true;
				}
				HighlightedOvercon = overconBase;
			}
		}
	}
}
