namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096CharacterModel : global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel
	{
		private static readonly int AnimatorEnragingHash = global::UnityEngine.Animator.StringToHash("Enraging");

		private static readonly int AnimatorEnragedHash = global::UnityEngine.Animator.StringToHash("Enraged");

		private static readonly int AnimatorChargingHash = global::UnityEngine.Animator.StringToHash("Charging");

		private static readonly int AnimatorTryNotToCryHash = global::UnityEngine.Animator.StringToHash("TryNotToCry");

		private static readonly int AnimatorLeftAttackHash = global::UnityEngine.Animator.StringToHash("LeftAttack");

		private static readonly int AnimatorPryGateHash = global::UnityEngine.Animator.StringToHash("PryGate");

		private static readonly int AnimatorCalmingHash = global::UnityEngine.Animator.StringToHash("Calming");

		private static readonly int AnimatorAttackHash = global::UnityEngine.Animator.StringToHash("Attack");

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Animator _thirdPersonAnimator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.ParticleSystem _shieldParticles;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096Role _role;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096AttackAbility _attackAbility;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096RageManager _rageAbility;

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Transform Head { get; private set; }

		protected override void Update()
		{
			base.Update();
			if (!(_rageAbility == null))
			{
				bool flag = _role.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.TryingNotToCry);
				_thirdPersonAnimator.SetBool(AnimatorEnragedHash, _rageAbility.IsEnraged);
				_thirdPersonAnimator.SetBool(AnimatorEnragingHash, _rageAbility.IsDistressed);
				_thirdPersonAnimator.SetBool(AnimatorChargingHash, _role.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.Charging));
				_thirdPersonAnimator.SetBool(AnimatorTryNotToCryHash, flag);
				_thirdPersonAnimator.SetBool(AnimatorAttackHash, _role.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.Attacking));
				_thirdPersonAnimator.SetBool(AnimatorLeftAttackHash, _attackAbility.LeftAttack);
				_thirdPersonAnimator.SetBool(AnimatorPryGateHash, _role.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.PryingGate));
				_thirdPersonAnimator.SetBool(AnimatorCalmingHash, _role.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming));
				global::UnityEngine.ParticleSystem.MainModule main = _shieldParticles.main;
				float num = ((!_role.IsLocalPlayer && _role.HumeShieldModule.HsCurrent > 0f && !flag) ? (_role.HumeShieldModule.HsCurrent / _role.HumeShieldModule.HsMax * 40f) : 0f);
				main.maxParticles = (int)(num + (float)((_rageAbility.IsEnraged && !_role.IsLocalPlayer) ? 10 : 0));
				main.simulationSpeed = ((!_rageAbility.IsEnraged || _role.IsLocalPlayer) ? 1 : 2);
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_role = base.OwnerHub.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp096.Scp096Role;
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096AttackAbility>(out _attackAbility);
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096RageManager>(out _rageAbility);
		}
	}
}
