using PlayerRoles.PlayableScps.Scp079.Overcons;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079OverconCameraSelector : Scp079DirectionalCameraSelector
	{
		private OverconBase CurOvercon => null;

		public override bool IsVisible => false;

		protected override bool AllowSwitchingBetweenZones => false;

		protected override bool TryGetCamera(out Scp079Camera targetCamera)
		{
			targetCamera = null;
			return false;
		}
	}
}
