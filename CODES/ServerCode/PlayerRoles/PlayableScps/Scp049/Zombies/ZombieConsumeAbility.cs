namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieConsumeAbility : global::PlayerRoles.PlayableScps.Scp049.RagdollAbilityBase<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole>
	{
		private enum ConsumeError : byte
		{
			None = 0,
			CannotCancel = 1,
			AlreadyConsumed = 2,
			TargetNotValid = 3,
			FullHealth = 8,
			BeingConsumed = 9
		}

		private const float ConsumeHeal = 100f;

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility> AllAbilities = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility>();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _eatAnimRotationFade;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _eatAnimPositionFade;

		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAttackAbility _attackAbility;

		private global::UnityEngine.Transform _headTransform;

		private bool _headRotationDirty;

		private global::UnityEngine.Vector3 _headRotation;

		public static readonly global::System.Collections.Generic.HashSet<BasicRagdoll> ConsumedRagdolls = new global::System.Collections.Generic.HashSet<BasicRagdoll>();

		protected override float Duration => 7f;

		protected override float RangeSqr => 3.3f;

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (_attackAbility.Cooldown.IsReady)
			{
				ClientTryStart();
			}
		}

		protected override byte ServerValidateCancel()
		{
			return 1;
		}

		protected override byte ServerValidateBegin(BasicRagdoll ragdoll)
		{
			if (ConsumedRagdolls.Contains(ragdoll))
			{
				return 2;
			}
			if (!ragdoll.Info.RoleType.IsHuman() || !base.ServerValidateAny())
			{
				return 3;
			}
			if (base.Owner.playerStats.GetModule<global::PlayerStatsSystem.HealthStat>().NormalizedValue == 1f)
			{
				return 8;
			}
			foreach (global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility allAbility in AllAbilities)
			{
				if (allAbility.IsInProgress && allAbility.CurRagdoll == ragdoll)
				{
					return 9;
				}
			}
			return 0;
		}

		protected override bool ServerValidateAny()
		{
			return true;
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAttackAbility>(out _attackAbility);
		}

		protected override void Update()
		{
			base.Update();
			_headRotationDirty = true;
		}

		protected override void ServerComplete()
		{
			if (!(base.CurRagdoll != null) || ConsumedRagdolls.Add(base.CurRagdoll))
			{
				base.Owner.playerStats.GetModule<global::PlayerStatsSystem.HealthStat>().ServerHeal(100f);
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			AllAbilities.Add(this);
			_headTransform = (base.ScpRole.FpcModule.CharacterModelInstance as global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieModel).HeadObject;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			AllAbilities.Remove(this);
		}

		public global::UnityEngine.Vector3 ProcessCamPos(global::UnityEngine.Vector3 original)
		{
			return global::UnityEngine.Vector3.Lerp(original, _headTransform.position, _eatAnimPositionFade.Evaluate(base.ProgressStatus));
		}

		public global::UnityEngine.Vector3 ProcessRotation()
		{
			if (_headRotationDirty)
			{
				_headRotation = global::UnityEngine.Quaternion.Lerp(base.Owner.PlayerCameraReference.rotation, _headTransform.rotation, _eatAnimRotationFade.Evaluate(base.ProgressStatus)).eulerAngles;
				_headRotationDirty = false;
			}
			return _headRotation;
		}
	}
}
