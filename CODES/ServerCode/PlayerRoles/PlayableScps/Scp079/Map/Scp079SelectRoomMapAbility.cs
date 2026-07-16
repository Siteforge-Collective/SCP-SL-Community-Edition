namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079SelectRoomMapAbility : global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079DirectionalCameraSelector
	{
		public override bool IsVisible
		{
			get
			{
				if (global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapState)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.HighlightedCamera != null;
				}
				return false;
			}
		}

		protected override bool TryGetCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera targetCamera)
		{
			targetCamera = global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.HighlightedCamera;
			return true;
		}

		protected override void Trigger()
		{
			base.Trigger();
			global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapState = false;
		}
	}
}
