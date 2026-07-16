using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using PlayerRoles.PlayableScps.Scp079.Map;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079Hud : ScpHudBase
	{
		[SerializeField]
		private Camera _mainCamera;

		[SerializeField]
		private CustomPassVolume _hudCustomPass;

		[SerializeField]
		private GameObject[] _aliveObjects;

		[SerializeField]
		private Volume _deathVolume;

		[SerializeField]
		private Gradient _deathBarsColor;

		[SerializeField]
		private RawImage _deathBars;

		[SerializeField]
		private float _deathAnimSpeed;

		[SerializeField]
		private Scp079NoiseController _noiseController;

		private float _defaultFov;

		private Scp079CurrentCameraSync _curCamSync;

		private Scp079MapToggler _mapToggler;

		private int _defaultLayerMask;

		public static Camera MainCamera { get; private set; }

		public static Scp079Role Instance { get; private set; }

		private Transform SceneCamera
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		protected override void Update()
		{
		}

		private void Awake()
		{
		}

		private void OnRoleChanged(ReferenceHub ply, PlayerRoleBase prev, PlayerRoleBase newRole)
		{
		}

		internal override void Init(ReferenceHub hub)
		{
		}

		internal override void OnDied()
		{
		}

		protected override void ToggleHud(bool b)
		{
		}

		protected override void OnDestroy()
		{
		}
	}
}
