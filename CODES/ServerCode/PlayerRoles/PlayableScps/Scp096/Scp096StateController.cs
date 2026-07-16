namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096StateController : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>
	{
		private global::PlayerRoles.PlayableScps.Scp096.Scp096RageState _rageState;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState _abilityState;

		private readonly global::System.Diagnostics.Stopwatch _rageChangeSw = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Diagnostics.Stopwatch _abilityChangeSw = new global::System.Diagnostics.Stopwatch();

		public global::PlayerRoles.PlayableScps.Scp096.Scp096RageState RageState
		{
			get
			{
				return _rageState;
			}
			set
			{
				if (_rageState != value)
				{
					this.OnRageUpdate?.Invoke(value);
					_rageState = value;
					_rageChangeSw.Restart();
					if (global::Mirror.NetworkServer.active)
					{
						ServerSendRpc(toAll: true);
					}
				}
			}
		}

		public global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState AbilityState
		{
			get
			{
				return _abilityState;
			}
			set
			{
				if (_abilityState != value)
				{
					this.OnAbilityUpdate?.Invoke(value);
					_abilityState = value;
					_abilityChangeSw.Restart();
					if (global::Mirror.NetworkServer.active)
					{
						ServerSendRpc(toAll: true);
					}
				}
			}
		}

		public float LastRageUpdate => (float)_rageChangeSw.Elapsed.TotalSeconds;

		public float LastAbilityUpdate => (float)_abilityChangeSw.Elapsed.TotalSeconds;

		public event global::System.Action<global::PlayerRoles.PlayableScps.Scp096.Scp096RageState> OnRageUpdate;

		public event global::System.Action<global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState> OnAbilityUpdate;

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)RageState);
			writer.WriteByte((byte)AbilityState);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (!global::Mirror.NetworkServer.active)
			{
				RageState = (global::PlayerRoles.PlayableScps.Scp096.Scp096RageState)reader.ReadByte();
				AbilityState = (global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState)reader.ReadByte();
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			RageState = global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Docile;
			AbilityState = global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.None;
			_rageChangeSw.Stop();
			_abilityChangeSw.Stop();
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_rageChangeSw.Start();
			_abilityChangeSw.Start();
		}

		public void SetRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState state)
		{
			base.Role.TryGetOwner(out var hub);
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp096ChangeState, hub, state))
			{
				RageState = state;
			}
		}

		public void SetAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState state)
		{
			AbilityState = state;
		}
	}
}
