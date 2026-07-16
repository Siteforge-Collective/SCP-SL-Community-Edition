using PlayerRoles.PlayableScps.Scp079.Cameras;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079SelectRoomMapAbility : Scp079DirectionalCameraSelector
	{
		public override bool IsVisible => false;

		protected override bool AllowSwitchingBetweenZones => false;

		protected override bool TryGetCamera(out Scp079Camera targetCamera)
		{
			targetCamera = null;
			return false;
		}

		protected override void Trigger()
		{
		}
	}
}
