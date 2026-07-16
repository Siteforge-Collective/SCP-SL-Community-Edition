namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieModel : global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel
	{
		private const int StrafeLayer = 6;

		private const int ConsumeLayer = 8;

		private const float ConsumeTransitionSpeed = 10f;

		private static readonly int StrafeHash = global::UnityEngine.Animator.StringToHash("Strafe");

		private static readonly int AttackHash = global::UnityEngine.Animator.StringToHash("Attack");

		private static readonly int ConsumeHash = global::UnityEngine.Animator.StringToHash("Eat");

		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAttackAbility _attackAbility;

		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility _consumeAbility;

		private float _prevConsume;

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Transform HeadObject { get; private set; }

		private void OnAttack()
		{
			base.Animator.SetTrigger(AttackHash);
		}

		protected override void Update()
		{
			base.Update();
			float value = global::UnityEngine.Mathf.Abs(base.Animator.GetFloat(StrafeHash));
			base.Animator.SetLayerWeight(6, global::UnityEngine.Mathf.Clamp01(value));
			float num = global::UnityEngine.Mathf.Clamp01(_prevConsume + global::UnityEngine.Time.deltaTime * 10f * (float)(_consumeAbility.IsInProgress ? 1 : (-1)));
			if (_prevConsume != num)
			{
				if (_prevConsume == 0f)
				{
					base.Animator.SetTrigger(ConsumeHash);
				}
				base.Animator.SetLayerWeight(8, num);
				_prevConsume = num;
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole obj = base.OwnerHub.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole;
			obj.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility>(out _consumeAbility);
			obj.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAttackAbility>(out _attackAbility);
			_attackAbility.OnTriggered += OnAttack;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_attackAbility.OnTriggered -= OnAttack;
			_prevConsume = 0f;
		}
	}
}
