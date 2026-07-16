namespace PlayerRoles.FirstPersonControl
{
	[global::UnityEngine.CreateAssetMenu(fileName = "New CharacterController Setting Preset", menuName = "ScriptableObject/Player Roles/Character Controller Preset")]
	public class CharacterControllerSettingsPreset : global::UnityEngine.ScriptableObject
	{
		public float SlopeLimit = 45f;

		public float StepOffset = 0.3f;

		public float SkinWidth = 0.08f;

		public float MinMoveDistance = 0.001f;

		public global::UnityEngine.Vector3 Center = global::UnityEngine.Vector3.zero;

		public float Radius = 0.5f;

		public float Height = 2.5f;

		public void Apply(global::UnityEngine.CharacterController charCtrl)
		{
			charCtrl.slopeLimit = SlopeLimit;
			charCtrl.stepOffset = StepOffset;
			charCtrl.skinWidth = SkinWidth;
			charCtrl.minMoveDistance = MinMoveDistance;
			charCtrl.center = Center;
			charCtrl.radius = Radius;
			charCtrl.height = Height;
		}
	}
}
