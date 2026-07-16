namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079SpectatableModule : global::PlayerRoles.Spectating.SpectatableModuleBase
	{
		private global::PlayerRoles.PlayableScps.Scp079.Scp079Role Scp079 => base.MainRole as global::PlayerRoles.PlayableScps.Scp079.Scp079Role;

		public override global::UnityEngine.Vector3 CameraPosition => Scp079.CameraPosition;

		public override global::UnityEngine.Vector3 CameraRotation
		{
			get
			{
				global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = Scp079.CurrentCamera;
				return new global::UnityEngine.Vector3(currentCamera.VerticalRotation, currentCamera.HorizontalRotation, currentCamera.RollRotation);
			}
		}
	}
}
