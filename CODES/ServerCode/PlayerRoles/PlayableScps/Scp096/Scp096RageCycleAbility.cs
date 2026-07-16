namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096RageCycleAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>, global::GameObjectPools.IPoolResettable
	{
		public const ActionName RageKey = ActionName.Reload;

		private const float EnragingTime = 6.1f;

		private const float CalmingTime = 5f;

		private const float DefaultActivationDuration = 10f;

		private const float RateCompensation = 0.2f;

		private const float KeyHoldTime = 0.4f;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _activationTime = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private global::PlayerRoles.PlayableScps.Scp096.Scp096RageManager _rageManager;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker _targetsTracker;

		private float _holdingRageCycleKey;

		private bool _wantsToToggle;

		private float _timeToChangeState;

		public float HudEnterRageSustain => global::UnityEngine.Mathf.InverseLerp(0.4f, 9.8f, _activationTime.Remaining);

		public float HudEnterRageKeyProgress => global::UnityEngine.Mathf.Clamp01(_holdingRageCycleKey / 0.4f);

		public bool CanStartCycle
		{
			get
			{
				if (!_activationTime.IsReady)
				{
					return _targetsTracker.CanReceiveTargets;
				}
				return false;
			}
		}

		public bool CanEndCycle => base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged);

		protected override ActionName TargetKey => ActionName.Reload;

		public bool ServerTryEnableInput(float duration = 10f)
		{
			_activationTime.Trigger(duration);
			ServerSendRpc(toAll: true);
			return true;
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (CanStartCycle)
			{
				_rageManager.ServerEnrage();
			}
			else if (CanEndCycle)
			{
				_rageManager.ServerEndEnrage();
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			_activationTime.WriteCooldown(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_activationTime.ReadCooldown(reader);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_activationTime.Clear();
			_holdingRageCycleKey = 0f;
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			_wantsToToggle = true;
		}

		protected override void Awake()
		{
			base.Awake();
			base.ScpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker>(out _targetsTracker);
			base.ScpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096RageManager>(out _rageManager);
			_targetsTracker.OnTargetAdded += AddTarget;
			_targetsTracker.OnTargetAttacked += AddTarget;
			base.ScpRole.StateController.OnRageUpdate += delegate(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState newState)
			{
				switch (newState)
				{
				case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming:
					_timeToChangeState = 5f;
					break;
				case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Distressed:
					_timeToChangeState = 6.1f;
					break;
				}
			};
		}

		protected override void Update()
		{
			base.Update();
			if (_wantsToToggle)
			{
				UpdateKeyHeld();
			}
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServerside();
			}
		}

		private void UpdateKeyHeld()
		{
			if ((!CanEndCycle && !CanStartCycle) || !IsKeyHeld)
			{
				_holdingRageCycleKey = 0f;
				_wantsToToggle = false;
				return;
			}
			_holdingRageCycleKey += global::UnityEngine.Time.deltaTime;
			if (_holdingRageCycleKey >= 0.4f)
			{
				ClientSendCmd();
				_holdingRageCycleKey = 0f;
			}
		}

		private void UpdateServerside()
		{
			switch (base.ScpRole.StateController.RageState)
			{
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged:
				return;
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Distressed:
				_timeToChangeState -= global::UnityEngine.Time.deltaTime;
				if (_timeToChangeState < 0f)
				{
					base.ScpRole.StateController.SetRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged);
				}
				return;
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming:
				_timeToChangeState -= global::UnityEngine.Time.deltaTime;
				if (_timeToChangeState < 0f)
				{
					base.ScpRole.StateController.SetRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Docile);
				}
				return;
			}
			foreach (ReferenceHub target in _targetsTracker.Targets)
			{
				if (_targetsTracker.IsObservedBy(target))
				{
					ServerTryEnableInput();
				}
			}
			if (_activationTime.IsReady && _targetsTracker.Targets.Count > 0)
			{
				_targetsTracker.ClearAllTargets();
				ServerSendRpc(toAll: true);
			}
		}

		private void AddTarget(ReferenceHub hub)
		{
			if (global::Mirror.NetworkServer.active)
			{
				ServerTryEnableInput();
			}
		}
	}
}
