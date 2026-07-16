using System.Collections.Generic;
using PlayerRoles.Ragdolls;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieConsumeAbility : RagdollAbilityBase<ZombieRole>
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

		private const float SimulatedBloodlustDuration = 5f;

		private static readonly HashSet<ZombieConsumeAbility> AllAbilities;

		[SerializeField]
		private AnimationCurve _eatAnimRotationFade;

		[SerializeField]
		private AnimationCurve _eatAnimPositionFade;

		private ZombieAttackAbility _attackAbility;

		private ZombieBloodlustAbility _bloodlustAbility;

		private Transform _headTransform;

		private bool _headRotationDirty;

		private Vector3 _headRotation;

		public static readonly HashSet<BasicRagdoll> ConsumedRagdolls;

		protected override float Duration => 0f;

		protected override float RangeSqr => 0f;

		protected override void OnKeyDown()
		{
		}

		protected override byte ServerValidateCancel()
		{
			return 0;
		}

		protected override void OnProgressSet()
		{
		}

		protected override byte ServerValidateBegin(BasicRagdoll ragdoll)
		{
			return 0;
		}

		protected override bool ServerValidateAny()
		{
			return false;
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		protected override void ServerComplete()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public Vector3 ProcessCamPos(Vector3 original)
		{
			return default(Vector3);
		}

		public Vector3 ProcessRotation()
		{
			return default(Vector3);
		}
	}
}
