using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.HUDs;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096HandsViewmodel : ScpViewmodelBase
	{
		[SerializeField]
		private float _fieldOfView;

		[SerializeField]
		private float _dampTime;

		[SerializeField]
		private float _weightAdjustSpeed;

		private bool _useEnragedLayer;

		private FirstPersonMovementModule _fpc;

		private Scp096AttackAbility _attackAbility;

		private Scp096StateController _stateController;

		private const int EnrageLayer = 1;

		private static readonly int HashWalk;

		private static readonly int HashExitRage;

		private static readonly int HashEnterRage;

		private static readonly int HashPryGate;

		private static readonly int HashLeftAttack;

		private static readonly int HashAttackTrigger;

		private static readonly int HashTryNotToCry;

		public override float CamFOV => 0f;

		protected override void Start()
		{
		}

		protected override void OnDestroy()
		{
		}

		private void OnAbilityUpdate(Scp096AbilityState newState)
		{
		}

		private void OnRageUpdate(Scp096RageState newState)
		{
		}

		private void OnAttackTriggered()
		{
		}

		private void OnHitReceived(Scp096HitResult hit)
		{
		}

		private void UpdateLayerWeight(float maxDelta)
		{
		}

		private void UpdateWalk(bool instant)
		{
		}

		protected override void UpdateAnimations()
		{
		}
	}
}
