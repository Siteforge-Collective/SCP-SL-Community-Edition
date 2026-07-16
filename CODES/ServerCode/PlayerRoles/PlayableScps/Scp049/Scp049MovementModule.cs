namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049MovementModule : global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp049.Scp049Role _role;

		private float _normalSpeed;

		private float _enragedSpeed;

		private global::PlayerRoles.PlayableScps.Scp049.Scp049SenseAbility _senseAbility;

		private float MovementSpeed
		{
			set
			{
				WalkSpeed = value;
				SprintSpeed = value;
			}
		}

		private void Awake()
		{
			_normalSpeed = WalkSpeed;
			_enragedSpeed = SprintSpeed;
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049SenseAbility>(out _senseAbility);
		}

		protected override void UpdateMovement()
		{
			MovementSpeed = (_senseAbility.HasTarget ? _enragedSpeed : _normalSpeed);
			base.UpdateMovement();
		}
	}
}
