namespace PlayerRoles.FirstPersonControl
{
	public interface IMovementInputOverride
	{
		bool MovementOverrideActive { get; }

		global::UnityEngine.Vector3 MovementOverrideDirection { get; }
	}
}
