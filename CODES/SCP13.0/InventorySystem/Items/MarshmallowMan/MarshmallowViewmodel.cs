using UnityEngine;

namespace InventorySystem.Items.MarshmallowMan
{
	public class MarshmallowViewmodel : StandardAnimatedViemodel
	{
		[SerializeField]
		private int _attackVariants;

		private int _curAttackVariant;

		private float _prevVel;

		private static readonly int AttackVariantHash;

		private static readonly int AttackTriggerHash;

		private static readonly int HolsterTriggerHash;

		private static readonly int WalkCycleHash;

		private const float VelAdjustSpeed = 28.5f;

		private const float WalkMaxVel = 7.5f;

		private const int WalkLayer = 2;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void OnHolsterRequested(ushort serial)
		{
		}

		private void OnSwing(ushort serial)
		{
		}

		public override void InitLocal(ItemBase ib)
		{
		}
	}
}
