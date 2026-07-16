namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079OverconCameraSelector : global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079DirectionalCameraSelector
	{
		private global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase CurOvercon => global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconManager.Singleton.HighlightedOvercon;

		public override bool IsVisible
		{
			get
			{
				if (!global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras && CurOvercon is global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon cameraOvercon)
				{
					return cameraOvercon != null;
				}
				return false;
			}
		}

		protected override bool TryGetCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera targetCamera)
		{
			if (!IsVisible)
			{
				targetCamera = null;
				return false;
			}
			targetCamera = (CurOvercon as global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon).Target;
			return true;
		}
	}
}
