namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939AmnesticCloudInstance : global::Hazards.TemporaryHazard
	{
		private enum CloudState
		{
			Spawning = 0,
			Created = 1,
			Destroyed = 2
		}

		public static readonly global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance> ActiveInstances;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _overallCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private readonly global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown> _individualCooldown = new global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown>();

		private global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility cloud;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility _lunge;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939Role _scpRole;

		private global::UnityEngine.Transform _t;

		private bool _abilitiesSet;

		private float _targetDuration;

		private float _lastHoldTime;

		private float _prevRange;

		private bool _localOwner;

		private bool _alreadyCreated;

		[global::Mirror.SyncVar]
		private byte _syncHoldTime;

		[global::Mirror.SyncVar]
		private byte _syncState;

		[global::Mirror.SyncVar]
		private uint _syncOwner;

		[global::Mirror.SyncVar]
		private global::RelativePositioning.RelativePosition _syncPos;

		[global::UnityEngine.Header("Balance")]
		[global::UnityEngine.SerializeField]
		private float _minHoldTime;

		[global::UnityEngine.SerializeField]
		private float _maxHoldTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _rangeOverHeldTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _durationOverHeldTime;

		[global::UnityEngine.SerializeField]
		private float _amnesiaDuration;

		[global::UnityEngine.SerializeField]
		private float _pauseDuration;

		[global::UnityEngine.Header("Lights")]
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Light _radiusLight;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Light _areaLight;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _radiusToLightAngle;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _radiusToLightRange;

		[global::UnityEngine.Header("Other audiovisual")]
		[global::UnityEngine.SerializeField]
		private float _destroyTime;

		[global::UnityEngine.SerializeField]
		private float _soundDropRate;

		[global::UnityEngine.SerializeField]
		private float _sizeLerpTime;

		[global::UnityEngine.SerializeField]
		private float _colorLerpTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _deploySound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _chargeupSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _chargeupVolumeOverSize;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState State
		{
			get
			{
				return (global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState)_syncState;
			}
			set
			{
				Network_syncState = (byte)value;
			}
		}

		private float NormalizedHoldTime => global::UnityEngine.Mathf.Clamp01(cloud.HoldDuration / _maxHoldTime);

		protected override float HazardDuration => _targetDuration;

		protected override float DecaySpeed
		{
			get
			{
				if (State != global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Created)
				{
					return 0f;
				}
				return 1f;
			}
		}

		public global::UnityEngine.Vector2 MinMaxTime => new global::UnityEngine.Vector2(_minHoldTime, _maxHoldTime);

		public byte Network_syncHoldTime
		{
			get
			{
				return _syncHoldTime;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncHoldTime))
				{
					byte syncHoldTime = _syncHoldTime;
					SetSyncVar(value, ref _syncHoldTime, 1uL);
				}
			}
		}

		public byte Network_syncState
		{
			get
			{
				return _syncState;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncState))
				{
					byte syncState = _syncState;
					SetSyncVar(value, ref _syncState, 2uL);
				}
			}
		}

		public uint Network_syncOwner
		{
			get
			{
				return _syncOwner;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncOwner))
				{
					uint syncOwner = _syncOwner;
					SetSyncVar(value, ref _syncOwner, 4uL);
				}
			}
		}

		public global::RelativePositioning.RelativePosition Network_syncPos
		{
			get
			{
				return _syncPos;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncPos))
				{
					global::RelativePositioning.RelativePosition syncPos = _syncPos;
					SetSyncVar(value, ref _syncPos, 8uL);
				}
			}
		}

		[global::Mirror.Server]
		public override void ServerDestroy()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance::ServerDestroy()' called when server was not active");
				return;
			}
			base.ServerDestroy();
			_abilitiesSet = false;
			State = global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Destroyed;
		}

		public override void OnStay(ReferenceHub player)
		{
			base.OnStay(player);
			if (State == global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Created && player.roleManager.CurrentRole is global::PlayerRoles.HumanRole)
			{
				PlayerEffectsController playerEffectsController = player.playerEffectsController;
				playerEffectsController.EnableEffect<global::CustomPlayerEffects.AmnesiaItems>(_amnesiaDuration);
				if (_overallCooldown.IsReady && (!_individualCooldown.TryGetValue(player.netId, out var value) || value.IsReady))
				{
					playerEffectsController.EnableEffect<global::CustomPlayerEffects.AmnesiaVision>(_amnesiaDuration);
				}
			}
		}

		protected override void Start()
		{
			_t = base.transform;
			ActiveInstances.Add(this);
			if (ReferenceHub.TryGetHubNetID(_syncOwner, out var hub) && hub.isLocalPlayer)
			{
				_localOwner = true;
				SetAbilityCache();
			}
			base.Start();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			ActiveInstances.Remove(this);
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDamaged -= OnAnyPlayerDamaged;
			if (_lunge != null)
			{
				_lunge.OnStateChanged -= OnLungeStateChanged;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (_localOwner)
			{
				UpdateLocal();
			}
			else
			{
				UpdateVisuals((float)(int)_syncHoldTime / 255f, global::UnityEngine.Time.deltaTime * _sizeLerpTime);
			}
			if (global::Mirror.NetworkServer.active)
			{
				switch (State)
				{
				case global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Spawning:
					ServerUpdateSpawning();
					break;
				case global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Destroyed:
					ServerUpdateDestroyed();
					break;
				}
			}
		}

		private void TryGetPlayer(out bool is939, out bool isOwner)
		{
			is939 = false;
			isOwner = false;
			if (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) || ReferenceHub.TryGetLocalHub(out hub))
			{
				is939 = hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp939.Scp939Role;
				isOwner = hub.netId == _syncOwner;
			}
		}

		private void OnAnyPlayerDamaged(ReferenceHub hub, global::PlayerStatsSystem.DamageHandlerBase dhb)
		{
			if (dhb is global::PlayerRoles.PlayableScps.Scp939.Scp939DamageHandler)
			{
				PauseAll();
			}
			else if (hub.netId == _syncOwner && dhb is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler)
			{
				global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown abilityCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();
				abilityCooldown.Trigger(_pauseDuration);
				uint attackerId = attackerDamageHandler.Attacker.NetId;
				_individualCooldown[attackerId] = abilityCooldown;
				if (global::Utils.NonAllocLINQ.ListExtensions.TryGetFirst(base.AffectedPlayers, (ReferenceHub x) => x.netId == attackerId, out var first) && first.playerEffectsController.TryGetEffect<global::CustomPlayerEffects.AmnesiaVision>(out var playerEffect))
				{
					playerEffect.IsEnabled = false;
				}
			}
		}

		private void OnLungeStateChanged(global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState state)
		{
			if (state != global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.None)
			{
				PauseAll();
			}
		}

		private void PauseAll()
		{
			foreach (ReferenceHub affectedPlayer in base.AffectedPlayers)
			{
				if (!affectedPlayer.playerEffectsController.TryGetEffect<global::CustomPlayerEffects.AmnesiaVision>(out var playerEffect))
				{
					return;
				}
				playerEffect.IsEnabled = false;
			}
			_overallCooldown.Trigger(_pauseDuration);
		}

		private void SetAbilityCache()
		{
			_abilitiesSet = false;
			if (ReferenceHub.TryGetHubNetID(_syncOwner, out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp939.Scp939Role scpRole)
			{
				_scpRole = scpRole;
				_abilitiesSet = _scpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility>(out cloud) && _scpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility>(out _lunge);
			}
		}

		private void RefreshPosition(ReferenceHub owner)
		{
			_t.position = owner.PlayerCameraReference.position;
		}

		private void UpdateLocal()
		{
			if (_abilitiesSet && ReferenceHub.TryGetLocalHub(out var hub))
			{
				switch (State)
				{
				case global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Destroyed:
					cloud.ClientCancel(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.CloudFailedSizeInsufficient);
					break;
				case global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Created:
					cloud.ClientCancel(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge);
					break;
				}
				if (!cloud.ValidateFloor())
				{
					cloud.ClientCancel((cloud.HoldDuration < _minHoldTime) ? global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.CloudFailedSizeInsufficient : global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.PressKeyToLunge);
				}
				if (cloud.TargetState)
				{
					UpdateVisuals(NormalizedHoldTime, 1f);
					RefreshPosition(hub);
				}
				else if (State != global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Spawning)
				{
					_localOwner = false;
				}
			}
		}

		private void UpdateVisuals(float normalizedSize, float lerpTime)
		{
			_deploySound.mute = (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) || ReferenceHub.TryGetLocalHub(out hub)) && hub.roleManager.CurrentRole is global::PlayerRoles.HumanRole;
			TryGetPlayer(out var @is, out var isOwner);
			_t.position = _syncPos.Position;
			float time = normalizedSize * _maxHoldTime;
			_prevRange = global::UnityEngine.Mathf.Lerp(_prevRange, _rangeOverHeldTime.Evaluate(time), lerpTime);
			_areaLight.enabled = @is;
			_radiusLight.enabled = @is;
			SetLightRadius(_areaLight, _prevRange);
			SetLightRadius(_radiusLight, _prevRange);
			_chargeupSound.mute = !isOwner;
			switch (State)
			{
			case global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Spawning:
				LerpLight(_radiusLight, global::UnityEngine.Color.white);
				_chargeupSound.volume = _chargeupVolumeOverSize.Evaluate(normalizedSize);
				return;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Destroyed:
				LerpLight(_radiusLight, global::UnityEngine.Color.black);
				LerpLight(_areaLight, global::UnityEngine.Color.black);
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Created:
				LerpLight(_radiusLight, global::UnityEngine.Color.white);
				LerpLight(_areaLight, global::UnityEngine.Color.white);
				break;
			}
			_chargeupSound.volume -= global::UnityEngine.Time.deltaTime;
		}

		private void LerpLight(global::UnityEngine.Light l, global::UnityEngine.Color c)
		{
			l.color = global::UnityEngine.Color.Lerp(l.color, c, global::UnityEngine.Time.deltaTime * _colorLerpTime);
		}

		private void SetLightRadius(global::UnityEngine.Light l, float range)
		{
			l.spotAngle = _radiusToLightAngle.Evaluate(range);
			l.range = _radiusToLightRange.Evaluate(range);
		}

		[global::Mirror.Server]
		private void ServerUpdateSpawning()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance::ServerUpdateSpawning()' called when server was not active");
				return;
			}
			if (!_abilitiesSet || !ReferenceHub.TryGetHubNetID(_syncOwner, out var hub) || _scpRole == null || _scpRole.Pooled)
			{
				ServerDestroy();
				return;
			}
			RefreshPosition(hub);
			Network_syncPos = new global::RelativePositioning.RelativePosition(_t.position);
			if (cloud.TargetState)
			{
				_lastHoldTime = cloud.HoldDuration;
				Network_syncHoldTime = (byte)global::UnityEngine.Mathf.RoundToInt(NormalizedHoldTime * 255f);
				if (_lastHoldTime < _maxHoldTime)
				{
					return;
				}
			}
			if (_lastHoldTime < _minHoldTime)
			{
				cloud.ServerFailPlacement();
				ServerDestroy();
				return;
			}
			_targetDuration = _durationOverHeldTime.Evaluate(_lastHoldTime);
			cloud.ServerConfirmPlacement(_targetDuration);
			MaxDistance = _rangeOverHeldTime.Evaluate(_lastHoldTime);
			State = global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.CloudState.Created;
			RpcPlayCreateSound();
		}

		[global::Mirror.Server]
		private void ServerUpdateDestroyed()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance::ServerUpdateDestroyed()' called when server was not active");
				return;
			}
			_destroyTime -= global::UnityEngine.Time.deltaTime;
			if (!(_destroyTime > 0f))
			{
				global::Mirror.NetworkServer.Destroy(base.gameObject);
			}
		}

		[global::Mirror.ClientRpc]
		private void RpcPlayCreateSound()
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			SendRPCInternal(typeof(global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance), "RpcPlayCreateSound", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.Server]
		public void ServerSetup(ReferenceHub owner)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance::ServerSetup(ReferenceHub)' called when server was not active");
				return;
			}
			Network_syncOwner = owner.netId;
			SetAbilityCache();
			_lunge.OnStateChanged += OnLungeStateChanged;
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDamaged += OnAnyPlayerDamaged;
		}

		static Scp939AmnesticCloudInstance()
		{
			ActiveInstances = new global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance>();
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance), "RpcPlayCreateSound", InvokeUserCode_RpcPlayCreateSound);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPlayCreateSound()
		{
			if (!_alreadyCreated)
			{
				_deploySound.Play();
				if (ReferenceHub.TryGetHubNetID(_syncOwner, out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp939.Scp939Role scp939Role && scp939Role.FpcModule.CharacterModelInstance is global::PlayerRoles.PlayableScps.Scp939.Scp939Model scp939Model)
				{
					_alreadyCreated = true;
					scp939Model.PlayCloudRelease();
				}
			}
		}

		protected static void InvokeUserCode_RpcPlayCreateSound(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcPlayCreateSound called on server.");
			}
			else
			{
				((global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance)obj).UserCode_RpcPlayCreateSound();
			}
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncHoldTime);
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncState);
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, _syncOwner);
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _syncPos);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncHoldTime);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncState);
				result = true;
			}
			if ((base.syncVarDirtyBits & 4L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, _syncOwner);
				result = true;
			}
			if ((base.syncVarDirtyBits & 8L) != 0L)
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _syncPos);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				byte syncHoldTime = _syncHoldTime;
				Network_syncHoldTime = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				byte syncState = _syncState;
				Network_syncState = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				uint syncOwner = _syncOwner;
				Network_syncOwner = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
				global::RelativePositioning.RelativePosition syncPos = _syncPos;
				Network_syncPos = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				byte syncHoldTime2 = _syncHoldTime;
				Network_syncHoldTime = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
			if ((num & 2L) != 0L)
			{
				byte syncState2 = _syncState;
				Network_syncState = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
			if ((num & 4L) != 0L)
			{
				uint syncOwner2 = _syncOwner;
				Network_syncOwner = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
			}
			if ((num & 8L) != 0L)
			{
				global::RelativePositioning.RelativePosition syncPos2 = _syncPos;
				Network_syncPos = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
			}
		}
	}
}
