namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173BreakneckSpeedsAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173Role>
	{
		private const float RechargeTime = 40f;

		private const float StareLimit = 10f;

		private const float MinimalTime = 1f;

		private readonly global::System.Diagnostics.Stopwatch _duration = new global::System.Diagnostics.Stopwatch();

		private float _disableTime;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observersTracker;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Rendering.PostProcessing.PostProcessVolume _ppVolume;

		[global::UnityEngine.SerializeField]
		private float _ppLerpSpeed;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		public global::System.Action OnToggled;

		private float Elapsed => (float)_duration.Elapsed.TotalSeconds;

		public bool IsActive
		{
			get
			{
				return _duration.IsRunning;
			}
			private set
			{
				if (value == IsActive || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp173BreakneckSpeeds, base.Owner, value))
				{
					return;
				}
				if (value)
				{
					_duration.Start();
					_disableTime = 0f;
				}
				else
				{
					_duration.Reset();
				}
				if (global::Mirror.NetworkServer.active)
				{
					if (!value)
					{
						Cooldown.Trigger(40f);
					}
					ServerSendRpc(toAll: true);
					OnToggled?.Invoke();
				}
			}
		}

		protected override ActionName TargetKey => ActionName.Run;

		private void UpdateServerside()
		{
			if (!IsActive)
			{
				return;
			}
			if (_disableTime > 0f)
			{
				if (!(Elapsed < _disableTime))
				{
					IsActive = false;
				}
			}
			else if (_observersTracker.IsObserved)
			{
				_disableTime = Elapsed + 10f;
			}
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			ClientSendCmd();
		}

		protected override void Update()
		{
			base.Update();
			if (base.Owner.isLocalPlayer || global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner))
			{
				_ppVolume.enabled = true;
				_ppVolume.weight = global::UnityEngine.Mathf.Lerp(_ppVolume.weight, IsActive ? 1 : 0, global::UnityEngine.Time.deltaTime * _ppLerpSpeed);
			}
			else
			{
				_ppVolume.enabled = false;
			}
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServerside();
			}
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observersTracker);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			if (IsActive)
			{
				if (!(Elapsed < 1f))
				{
					IsActive = false;
				}
			}
			else if (Cooldown.IsReady)
			{
				IsActive = true;
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, IsActive);
			if (!IsActive)
			{
				Cooldown.WriteCooldown(writer);
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			IsActive = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			if (!IsActive)
			{
				Cooldown.ReadCooldown(reader);
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_ppVolume.weight = 0f;
			IsActive = false;
			Cooldown.Clear();
		}
	}
}
