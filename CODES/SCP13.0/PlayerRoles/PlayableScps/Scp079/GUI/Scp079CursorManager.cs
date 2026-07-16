using CursorManagement;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;
using UserSettings;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079CursorManager : Scp079GuiElementBase, ICursorOverride
	{
		[SerializeField]
		private GameObject _freeLookCursor;

		private Scp079CurrentCameraSync _curCamSync;

		private Scp079LostSignalHandler _lostSignalHandler;

		private static readonly ToggleOrHoldInput FreeLookMode;

		public static CursorOverrideMode CurrentMode { get; private set; }

		public static bool LockCameras { get; private set; }

		public CursorOverrideMode CursorOverride => default(CursorOverrideMode);

		public bool LockMovement => false;

		private void Update()
		{
		}

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}
	}
}
