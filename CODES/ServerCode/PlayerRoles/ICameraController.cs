namespace PlayerRoles
{
	public interface ICameraController
	{
		global::UnityEngine.Vector3 CameraPosition { get; }

		float VerticalRotation { get; }

		float HorizontalRotation { get; }
	}
}
