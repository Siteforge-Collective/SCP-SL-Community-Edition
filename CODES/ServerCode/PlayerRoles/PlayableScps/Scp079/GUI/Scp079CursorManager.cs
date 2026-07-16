namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079CursorManager : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase, global::CursorManagement.ICursorOverride
	{
		[global::UnityEngine.SerializeField]
		private ActionName _freeLookKey;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _freeLookCursor;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		private bool _freeLookToggleMode;

		private bool _mouseLook;

		public static global::CursorManagement.CursorOverrideMode CurrentMode { get; private set; }

		public static bool LockCameras { get; private set; }

		public global::CursorManagement.CursorOverrideMode CursorOverride => CurrentMode;

		public bool LockMovement => LockCameras;

		private void Update()
		{
			global::UnityEngine.KeyCode key = NewInput.GetKey(_freeLookKey);
			if (_freeLookToggleMode)
			{
				if (global::UnityEngine.Input.GetKeyDown(key))
				{
					_mouseLook = !_mouseLook;
				}
			}
			else
			{
				_mouseLook = global::UnityEngine.Input.GetKey(key);
			}
			CurrentMode = (_mouseLook ? global::CursorManagement.CursorOverrideMode.Centered : global::CursorManagement.CursorOverrideMode.Confined);
			LockCameras = false;
			if (_curCamSync.CurClientSwitchState != global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None || global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapState)
			{
				LockCameras = true;
				CurrentMode = global::CursorManagement.CursorOverrideMode.Centered;
			}
			if (global::UnityEngine.Cursor.lockState == global::UnityEngine.CursorLockMode.None || !global::UnityEngine.Application.isFocused)
			{
				LockCameras = true;
			}
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			if (owner.isLocalPlayer)
			{
				global::CursorManagement.CursorManager.Register(this);
				_freeLookToggleMode = PlayerPrefsSl.Get(SettingsOption.ModeSwitchSetting079.ToString(), defaultValue: false);
			}
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
		}

		private void OnDestroy()
		{
			global::CursorManagement.CursorManager.Unregister(this);
		}
	}
}
