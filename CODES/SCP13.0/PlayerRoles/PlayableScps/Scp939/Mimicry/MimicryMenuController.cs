using ToggleableMenus;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryMenuController : ToggleableMenuBase
	{
		[SerializeField]
		private float _fadeSpeed;

		[SerializeField]
		private CanvasGroup _fader;

		[SerializeField]
		private CanvasGroup[] _inverseFaders;

		private bool _canvasCacheSet;

		private Canvas _cachedCanvas;

		public static MimicryMenuController Singleton { get; private set; }

		public static bool SingletonSet { get; private set; }

		public static bool FullyClosed => false;

		public override bool CanToggle => false;

		public static float ScaleFactor => 0f;

		private void Update()
		{
		}

		protected override void Awake()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void OnToggled()
		{
		}
	}
}
