namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryMenuController : global::ToggleableMenus.ToggleableMenuBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _highlightColor;

		[global::UnityEngine.SerializeField]
		private float _highlightLerpSpeed;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _blur;

		[global::UnityEngine.SerializeField]
		private float _fadeSpeed;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _defaultGroup;

		private readonly global::System.Collections.Generic.HashSet<global::UnityEngine.CanvasGroup> _registeredGroups = new global::System.Collections.Generic.HashSet<global::UnityEngine.CanvasGroup>();

		private readonly global::System.Diagnostics.Stopwatch _interactionDelay = global::System.Diagnostics.Stopwatch.StartNew();

		private global::UnityEngine.CanvasGroup _curGroup;

		private bool _recenterCursor;

		private const float InteractionDelay = 0.2f;

		public static global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicryMenuController Singleton { get; private set; }

		public static bool ReadyForInteraction { get; private set; }

		public override bool CanToggle
		{
			get
			{
				if (ReferenceHub.TryGetLocalHub(out var hub))
				{
					return hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp939.Scp939Role;
				}
				return false;
			}
		}

		public override global::CursorManagement.CursorOverrideMode CursorOverride
		{
			get
			{
				if (_recenterCursor)
				{
					_recenterCursor = false;
					return global::CursorManagement.CursorOverrideMode.Centered;
				}
				return base.CursorOverride;
			}
		}

		public global::UnityEngine.CanvasGroup CurrentGroup
		{
			get
			{
				return _curGroup;
			}
			set
			{
				if (!(value == _curGroup))
				{
					if (_curGroup != null)
					{
						_recenterCursor = true;
					}
					_curGroup = value;
					if (value != null)
					{
						_blur.SetActive(value: true);
						_registeredGroups.Add(value);
						_defaultGroup = value;
					}
					else
					{
						_blur.SetActive(value: false);
						IsEnabled = false;
					}
				}
			}
		}

		private void Update()
		{
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(_registeredGroups, delegate(global::UnityEngine.CanvasGroup x)
			{
				x.alpha = global::UnityEngine.Mathf.MoveTowards(x.alpha, (x == CurrentGroup) ? 1 : 0, global::UnityEngine.Time.deltaTime * _fadeSpeed);
			});
			if (CurrentGroup != null)
			{
				_interactionDelay.Restart();
				ReadyForInteraction = false;
			}
			else if (!ReadyForInteraction)
			{
				ReadyForInteraction = _interactionDelay.Elapsed.TotalSeconds > 0.20000000298023224;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			Singleton = this;
		}

		protected override void OnToggled()
		{
			CurrentGroup = (IsEnabled ? _defaultGroup : null);
		}

		public static void UpdateHighlight(global::UnityEngine.UI.Image img, bool isHighlighted)
		{
			UpdateHighlight(img, isHighlighted, global::UnityEngine.Time.deltaTime * Singleton._highlightLerpSpeed);
		}

		public static void UpdateHighlight(global::UnityEngine.UI.Image img, bool isHighlighted, float lerpAmount)
		{
			global::UnityEngine.Color b = (isHighlighted ? Singleton._highlightColor : global::UnityEngine.Color.clear);
			img.color = global::UnityEngine.Color.Lerp(img.color, b, lerpAmount);
		}
	}
}
