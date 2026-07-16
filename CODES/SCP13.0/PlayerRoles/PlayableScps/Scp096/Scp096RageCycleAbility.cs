using GameObjectPools;
using Mirror;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096RageCycleAbility : KeySubroutine<Scp096Role>, IPoolResettable
	{
		public const ActionName RageKey = ActionName.Reload;

		private const float EnragingTime = 6.1f;

		private const float CalmingTime = 5f;

		private const float DefaultActivationDuration = 10f;

		private const float RateCompensation = 0.2f;

		private const float KeyHoldTime = 0.4f;

		private readonly AbilityCooldown _activationTime;

		private Scp096RageManager _rageManager;

		private Scp096TargetsTracker _targetsTracker;

		private float _holdingRageCycleKey;

		private bool _wantsToToggle;

		private float _timeToChangeState;

		public float HudEnterRageSustain => 0f;

		public float HudEnterRageKeyProgress => 0f;

		public bool CanStartCycle => false;

		public bool CanEndCycle => false;

		protected override ActionName TargetKey => default(ActionName);

		public bool ServerTryEnableInput(float duration = 10f)
		{
			return false;
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		private void UpdateKeyHeld()
		{
		}

		private void UpdateServerside()
		{
		}

		private void AddTarget(ReferenceHub ownerHub, ReferenceHub targetedHub)
		{
		}
	}
}
