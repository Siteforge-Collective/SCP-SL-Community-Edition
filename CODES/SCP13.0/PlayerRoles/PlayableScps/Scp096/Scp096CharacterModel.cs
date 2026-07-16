using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096CharacterModel : AnimatedCharacterModel
	{
		private static readonly int AnimatorEnragingHash;

		private static readonly int AnimatorEnragedHash;

		private static readonly int AnimatorChargingHash;

		private static readonly int AnimatorTryNotToCryHash;

		private static readonly int AnimatorLeftAttackHash;

		private static readonly int AnimatorPryGateHash;

		private static readonly int AnimatorCalmingHash;

		private static readonly int AnimatorAttackHash;

		[SerializeField]
		private Animator _thirdPersonAnimator;

		[SerializeField]
		private ParticleSystem _shieldParticles;

		private Scp096Role _role;

		private Scp096AttackAbility _attackAbility;

		private Scp096RageManager _rageAbility;

		[field: SerializeField]
		public Transform Head { get; private set; }

		protected override void Update()
		{
		}

		public override void SpawnObject()
		{
		}
	}
}
