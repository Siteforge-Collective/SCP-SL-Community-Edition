using PlayerRoles.FirstPersonControl;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939MouseLook : FpcMouseLook
	{
		private float _angleBank;

		private readonly Scp939FocusAbility _focus;

		private readonly Scp939LungeAbility _lunge;

		private readonly Scp939MovementModule _fpc939;

		private readonly bool _isSet;

		private const float FocusAngleLimit = 90f;

		private const float SlowdownAngle = 30f;

		private const float LungeAnglePerSecond = 85f;

		private const float LungeStartAngle = 5f;

		private const float LungeAngleMax = 70f;

		private bool Lunging => false;

		protected override float ProcessHorizontalInput(float f)
		{
			return 0f;
		}

		protected override float ClampHorizontal(float f)
		{
			return 0f;
		}

		public Scp939MouseLook(ReferenceHub hub, Scp939MovementModule mm)
			: base(null, null)
		{
		}
	}
}
