using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieModel : HumanCharacterModel
	{
		private const int StrafeLayer = 6;

		private const int ConsumeLayer = 8;

		private const float ConsumeTransitionSpeed = 10f;

		private static readonly int StrafeHash;

		private static readonly int AttackHash;

		private static readonly int ConsumeHash;

		private ZombieAttackAbility _attackAbility;

		private ZombieConsumeAbility _consumeAbility;

		private float _prevConsume;

		[field: SerializeField]
		public Transform HeadObject { get; private set; }

		private void OnAttack()
		{
		}

		protected override void Update()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
