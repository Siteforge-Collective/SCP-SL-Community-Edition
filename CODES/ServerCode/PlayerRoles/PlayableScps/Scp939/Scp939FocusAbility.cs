namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939FocusAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>, global::CameraShaking.IShakeEffect
	{
		private global::CustomPlayerEffects.SoundtrackMute _muteEffect;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusKeySync _keySync;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939ClawAbility _clawAbility;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility _lungeAbility;

		private global::UnityEngine.Transform _ownerTransform;

		private float _state;

		private bool _targetState;

		private float _offsetMultiplier;

		private float _relativeFreezeRot;

		private byte _relativeWaypoint;

		[global::UnityEngine.SerializeField]
		private float _transitionSpeed;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _focusInSource;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _serverSendCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private readonly global::System.Diagnostics.Stopwatch _frozenSw = new global::System.Diagnostics.Stopwatch();

		private const int AngleSyncAccuracy = 64;

		private const float ResendCooldown = 1.5f;

		public float State
		{
			get
			{
				return _state;
			}
			set
			{
				value = global::UnityEngine.Mathf.Clamp01(value);
				if (value != _state)
				{
					_state = value;
					this.OnStateChanged?.Invoke();
				}
			}
		}

		public bool TargetState
		{
			get
			{
				if (!_targetState)
				{
					return _lungeAbility.State == global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.Triggered;
				}
				return true;
			}
			private set
			{
				if (_targetState != value)
				{
					if (value && State == 0f)
					{
						_relativeWaypoint = CurrentWaypointId;
						_relativeFreezeRot = global::RelativePositioning.WaypointBase.GetRelativeRotation(_relativeWaypoint, _ownerTransform.rotation).eulerAngles.y;
						_frozenSw.Restart();
					}
					_targetState = value;
					_serverSendCooldown.Clear();
				}
			}
		}

		public float FrozenTime => (float)_frozenSw.Elapsed.TotalSeconds;

		public float FrozenRotation => global::RelativePositioning.WaypointBase.GetWorldRotation(_relativeWaypoint, global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * _relativeFreezeRot)).eulerAngles.y;

		public float AngularDeviation
		{
			get
			{
				if (!(State > 0f))
				{
					return 0f;
				}
				return global::UnityEngine.Mathf.DeltaAngle(FrozenRotation, _ownerTransform.eulerAngles.y);
			}
		}

		private byte CurrentWaypointId => new global::RelativePositioning.RelativePosition(base.ScpRole.FpcModule.Position).WaypointId;

		private bool IsAvailable => base.ScpRole.FpcModule.IsGrounded;

		public event global::System.Action OnStateChanged;

		private void Update()
		{
			State += global::UnityEngine.Time.deltaTime * (TargetState ? _transitionSpeed : (0f - _transitionSpeed));
			if (global::Mirror.NetworkServer.active)
			{
				TargetState = IsAvailable && _keySync.FocusKeyHeld && _clawAbility.Cooldown.IsReady && (_targetState || State == 0f);
				_muteEffect.IsEnabled = TargetState;
				UpdateRelativeRotation();
				if (_serverSendCooldown.IsReady)
				{
					ServerSendRpc(toAll: true);
					_serverSendCooldown.Trigger(1.5f);
				}
			}
		}

		private void UpdateRelativeRotation()
		{
			if (TargetState)
			{
				byte currentWaypointId = CurrentWaypointId;
				if (currentWaypointId != _relativeWaypoint)
				{
					_relativeFreezeRot = global::RelativePositioning.WaypointBase.GetRelativeRotation(currentWaypointId, global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * FrozenRotation)).eulerAngles.y;
					_relativeWaypoint = currentWaypointId;
					_serverSendCooldown.Clear();
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusKeySync>(out _keySync);
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939ClawAbility>(out _clawAbility);
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility>(out _lungeAbility);
			_lungeAbility.OnStateChanged += delegate(global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState newState)
			{
				if (newState != global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.None)
				{
					TargetState = false;
				}
			};
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			int num = global::UnityEngine.Mathf.RoundToInt(_relativeFreezeRot * 64f) + 1;
			if (!_targetState)
			{
				num *= -1;
			}
			global::Mirror.NetworkWriterExtensions.WriteInt16(writer, (short)num);
			writer.WriteByte(_relativeWaypoint);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			short num = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
			_targetState = num > 0;
			_relativeWaypoint = reader.ReadByte();
			_relativeFreezeRot = ((float)global::UnityEngine.Mathf.Abs(num) - 1f) / 64f;
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_ownerTransform = base.Owner.transform;
			if (global::Mirror.NetworkServer.active)
			{
				_muteEffect = base.Owner.playerEffectsController.GetEffect<global::CustomPlayerEffects.SoundtrackMute>();
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_state = 0f;
			_targetState = false;
		}
	}
}
