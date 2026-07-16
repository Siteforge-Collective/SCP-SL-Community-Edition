namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173BlinkTimer : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::GameObjectPools.IPoolResettable
	{
		private const float CooldownBaseline = 3f;

		private const float CooldownPerObserver = 0f;

		private const float BreakneckCooldownMultiplier = 0.5f;

		private const float SustainTime = 3f;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observers;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173MovementModule _fpcModule;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private float _totalCooldown;

		private double _initialStopTime;

		private double _endSustainTime;

		private float TotalCooldown
		{
			get
			{
				if (!global::Mirror.NetworkServer.active)
				{
					return _totalCooldown;
				}
				return TotalCooldownServer;
			}
		}

		private float TotalCooldownServer => _totalCooldown * (_breakneckSpeedsAbility.IsActive ? 0.5f : 1f);

		private float RemainingSustain => (float)(_endSustainTime - global::Mirror.NetworkTime.time);

		public float RemainingBlinkCooldown => global::UnityEngine.Mathf.Max(0f, (float)(_initialStopTime + (double)TotalCooldown - global::Mirror.NetworkTime.time));

		public float RemainingSustainPercent
		{
			get
			{
				if (_endSustainTime != -1.0)
				{
					return global::UnityEngine.Mathf.Clamp(RemainingSustain, 0f, 3f) / 3f;
				}
				return 1f;
			}
		}

		public bool AbilityReady
		{
			get
			{
				if (RemainingSustainPercent > 0f)
				{
					return RemainingBlinkCooldown <= 0f;
				}
				return false;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (base.Role is global::PlayerRoles.PlayableScps.Scp173.Scp173Role scp173Role)
			{
				_fpcModule = scp173Role.FpcModule as global::PlayerRoles.PlayableScps.Scp173.Scp173MovementModule;
				scp173Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility>(out _breakneckSpeedsAbility);
				scp173Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observers);
				_observers.OnObserversChanged += OnObserversChanged;
				global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility breakneckSpeedsAbility = _breakneckSpeedsAbility;
				breakneckSpeedsAbility.OnToggled = (global::System.Action)global::System.Delegate.Combine(breakneckSpeedsAbility.OnToggled, (global::System.Action)delegate
				{
					ServerSendRpc(toAll: true);
				});
			}
		}

		private void OnObserversChanged(int prev, int current)
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (prev == 0 && RemainingSustainPercent == 0f)
				{
					_initialStopTime = global::Mirror.NetworkTime.time;
					_totalCooldown = 3f;
				}
				_totalCooldown += 0f * (float)(current - prev);
				_endSustainTime = ((current > 0) ? (-1.0) : (global::Mirror.NetworkTime.time + 3.0));
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _initialStopTime);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _endSustainTime);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, TotalCooldownServer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (!global::Mirror.NetworkServer.active)
			{
				_initialStopTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
				_endSustainTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
				_totalCooldown = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
		}

		public void ResetObject()
		{
			_totalCooldown = 0f;
			_initialStopTime = 0.0;
			_endSustainTime = 0.0;
		}

		public void ServerBlink(global::UnityEngine.Vector3 pos)
		{
			int currentObservers = _observers.CurrentObservers;
			_fpcModule.ServerTeleportTo(pos);
			_initialStopTime = global::Mirror.NetworkTime.time;
			_observers.UpdateObservers();
			if (currentObservers == _observers.CurrentObservers)
			{
				ServerSendRpc(toAll: true);
			}
		}
	}
}
