using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class OverconManager : MonoBehaviour
	{
		[SerializeField]
		private OverconRendererBase[] _renderers;

		private Scp079CurrentCameraSync _curCamSync;

		private const string OverconLayer = "Viewmodel";

		private const float Range = 200f;

		private static int _raycastMask;

		private static int RaycastMask => 0;

		public OverconBase HighlightedOvercon { get; private set; }

		public static OverconManager Singleton { get; private set; }

		private void OnCameraChanged()
		{
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}
	}
}
