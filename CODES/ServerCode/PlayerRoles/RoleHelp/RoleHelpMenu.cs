namespace PlayerRoles.RoleHelp
{
	public class RoleHelpMenu : global::ToggleableMenus.ToggleableMenuBase
	{
		private bool _instanceSet;

		private global::UnityEngine.GameObject _instance;

		private bool _fading;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _parent;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _root;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _fader;

		[global::UnityEngine.SerializeField]
		private float _fadeSpeed;

		public override bool CanToggle => _instanceSet;

		public override global::CursorManagement.CursorOverrideMode CursorOverride => global::CursorManagement.CursorOverrideMode.NoOverride;

		protected override void OnToggled()
		{
			_fading = true;
			_root.SetActive(value: true);
		}

		protected override void Awake()
		{
			base.Awake();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
		}

		private void Update()
		{
			if (_fading && UpdateAlpha(IsEnabled ? _fadeSpeed : (0f - _fadeSpeed)))
			{
				_fading = false;
				_root.SetActive(IsEnabled);
			}
		}

		private bool UpdateAlpha(float speed)
		{
			float alpha = _fader.alpha;
			alpha = global::UnityEngine.Mathf.Clamp01(alpha + global::UnityEngine.Time.deltaTime * speed);
			bool result = alpha == 1f;
			bool result2 = alpha == 0f;
			_fader.alpha = alpha;
			if (!(speed > 0f))
			{
				return result2;
			}
			return result;
		}

		private void OnRoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (hub.isLocalPlayer)
			{
				IsEnabled = false;
				if (_instanceSet)
				{
					global::UnityEngine.Object.Destroy(_instance);
					_instanceSet = false;
				}
				global::UnityEngine.GameObject roleHelpInfo = newRole.RoleHelpInfo;
				if (!(roleHelpInfo == null))
				{
					_instance = global::UnityEngine.Object.Instantiate(roleHelpInfo, _parent);
					_instanceSet = true;
				}
			}
		}
	}
}
