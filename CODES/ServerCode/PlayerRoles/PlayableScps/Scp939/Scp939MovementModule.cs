namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939MovementModule : global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _staminaRegenerationCurve;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939Motor _motor939;

		private const float StaminaUseRate = 0.1f;

		private const float StaminaRespawnImmunity = 2f;

		protected override global::PlayerRoles.FirstPersonControl.PlayerMovementState ValidateMovementState(global::PlayerRoles.FirstPersonControl.PlayerMovementState state)
		{
			state = base.ValidateMovementState(state);
			if (state == global::PlayerRoles.FirstPersonControl.PlayerMovementState.Sprinting && !_motor939.MovingForwards)
			{
				state = global::PlayerRoles.FirstPersonControl.PlayerMovementState.Walking;
			}
			return state;
		}

		protected override void SetupModules()
		{
			base.Motor = (_motor939 = new global::PlayerRoles.PlayableScps.Scp939.Scp939Motor(base.Hub, base.Role as global::PlayerRoles.PlayableScps.Scp939.Scp939Role));
			base.Noclip = new global::PlayerRoles.FirstPersonControl.FpcNoclip(base.Hub, this);
			base.MouseLook = new global::PlayerRoles.PlayableScps.Scp939.Scp939MouseLook(base.Hub, this);
			base.StateProcessor = new global::PlayerRoles.PlayableScps.Scp939.Scp939StateProcessor(base.Hub, _motor939, this, 0.1f, 2f, _staminaRegenerationCurve);
		}
	}
}
