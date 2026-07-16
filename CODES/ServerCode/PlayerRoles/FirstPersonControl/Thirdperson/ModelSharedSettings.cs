namespace PlayerRoles.FirstPersonControl.Thirdperson
{
	[global::UnityEngine.CreateAssetMenu(fileName = "New Model Settings", menuName = "ScriptableObject/Role Management/Model Shared Settings")]
	public class ModelSharedSettings : global::UnityEngine.ScriptableObject
	{
		public global::UnityEngine.AudioClip FallDamageSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _walkBobHorizontal;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _walkBobVertical;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _strafeBobHorizontal;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _strafeBobVertical;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _bobScaleOverParams;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _landingAnimation;

		private readonly global::System.Diagnostics.Stopwatch _landingSw = new global::System.Diagnostics.Stopwatch();

		public global::UnityEngine.Vector3 GetHeadBob(float time, global::UnityEngine.Vector2 animDirection)
		{
			float magnitude = animDirection.magnitude;
			if (magnitude > 1f)
			{
				animDirection /= magnitude;
			}
			global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(0f, _landingAnimation.Evaluate((float)_landingSw.Elapsed.TotalSeconds), 0f);
			global::UnityEngine.Vector3 vector2 = new global::UnityEngine.Vector3(_walkBobHorizontal.Evaluate(time), _walkBobVertical.Evaluate(time), 0f);
			global::UnityEngine.Vector3 vector3 = new global::UnityEngine.Vector3(_strafeBobHorizontal.Evaluate(time), _strafeBobVertical.Evaluate(time), 0f);
			return (vector2 * global::UnityEngine.Mathf.Abs(animDirection.y) + vector3 * global::UnityEngine.Mathf.Abs(animDirection.x)) * _bobScaleOverParams.Evaluate(magnitude) + vector;
		}

		public void PlayLandingAnimation()
		{
			_landingSw.Restart();
		}
	}
}
