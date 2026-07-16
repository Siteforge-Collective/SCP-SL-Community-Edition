namespace MapGeneration.Distributors
{
	public class Scp079Generator : global::MapGeneration.Distributors.SpawnableStructure, global::Interactables.IServerInteractable, global::Interactables.IInteractable
	{
		[global::System.Serializable]
		private class GeneratorGauge
		{
			[global::UnityEngine.SerializeField]
			private global::UnityEngine.Transform _gauge;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.Vector3 _mask;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AnimationCurve _values;

			[global::UnityEngine.SerializeField]
			private float _smoothing;

			public void UpdateValue(float f)
			{
				global::UnityEngine.Quaternion localRotation = _gauge.transform.localRotation;
				global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.Euler(_mask * _values.Evaluate(f));
				_gauge.transform.localRotation = global::UnityEngine.Quaternion.Lerp(localRotation, b, global::UnityEngine.Time.deltaTime * _smoothing);
			}
		}

		[global::System.Serializable]
		private class GeneratorLED
		{
			[global::UnityEngine.SerializeField]
			private global::UnityEngine.Renderer _rend;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.Material _onMat;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.Material _offMat;

			private byte _prevValue;

			public void UpdateValue(bool b)
			{
				byte b2 = (byte)(b ? 1u : 2u);
				if (b2 != _prevValue)
				{
					_rend.sharedMaterial = (b ? _onMat : _offMat);
					_prevValue = b2;
				}
			}
		}

		[global::System.Flags]
		public enum GeneratorFlags : byte
		{
			None = 1,
			Unlocked = 2,
			Open = 4,
			Activating = 8,
			Engaged = 0x10
		}

		public enum GeneratorColliderId : byte
		{
			Door = 0,
			Switch = 1,
			CancelButton = 2
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Animator _doorAnimator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Animator _leverAnimator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _audioSource;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _deniedClip;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _unlockClip;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _openClip;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _closeClip;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _countdownClip;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Renderer _keycardRenderer;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _lockedMaterial;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _unlockedMaterial;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _deniedMaterial;

		[global::UnityEngine.SerializeField]
		private float _deniedCooldownTime;

		[global::UnityEngine.SerializeField]
		private float _doorToggleCooldownTime;

		[global::UnityEngine.SerializeField]
		private float _unlockCooldownTime;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.KeycardPermissions _requiredPermission;

		[global::UnityEngine.SerializeField]
		private float _leverDelay;

		[global::UnityEngine.SerializeField]
		private float _totalActivationTime;

		[global::UnityEngine.SerializeField]
		private float _totalDeactivationTime;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.Distributors.Scp079Generator.GeneratorGauge _localGauge;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.Distributors.Scp079Generator.GeneratorGauge _totalGauge;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.Distributors.Scp079Generator.GeneratorLED _onLED;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.Distributors.Scp079Generator.GeneratorLED _offLED;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.Distributors.Scp079Generator.GeneratorLED[] _waitLights;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _screen;

		[global::UnityEngine.Multiline]
		[global::UnityEngine.SerializeField]
		private string _screenCountdown;

		[global::UnityEngine.Multiline]
		[global::UnityEngine.SerializeField]
		private string _screenEngaged;

		[global::UnityEngine.Multiline]
		[global::UnityEngine.SerializeField]
		private string _screenOffline;

		[global::Mirror.SyncVar]
		private byte _flags;

		[global::Mirror.SyncVar]
		private short _syncTime;

		private static readonly int DoorAnimHash;

		private static readonly int LeverAnimHash;

		private short _prevTime;

		private byte _prevFlags;

		private float _targetCooldown;

		private float _currentTime;

		private global::Footprinting.Footprint _lastActivator;

		private readonly global::System.Diagnostics.Stopwatch _cooldownStopwatch = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Diagnostics.Stopwatch _leverStopwatch = new global::System.Diagnostics.Stopwatch();

		private const float UnlockTokenReward = 0.5f;

		private const float EngageTokenReward = 1f;

		private float DropdownSpeed => _totalActivationTime / _totalDeactivationTime;

		private bool ActivationReady
		{
			get
			{
				if (Activating)
				{
					return _leverStopwatch.Elapsed.TotalSeconds > (double)_leverDelay;
				}
				return false;
			}
		}

		public bool Engaged
		{
			get
			{
				return HasFlag(_flags, global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Engaged);
			}
			set
			{
				ServerSetFlag(global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Engaged, value);
			}
		}

		public bool Activating
		{
			get
			{
				return HasFlag(_flags, global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Activating);
			}
			set
			{
				ServerSetFlag(global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Activating, value);
			}
		}

		public int RemainingTime => _syncTime;

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public byte Network_flags
		{
			get
			{
				return _flags;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _flags))
				{
					byte flags = _flags;
					SetSyncVar(value, ref _flags, 1uL);
				}
			}
		}

		public short Network_syncTime
		{
			get
			{
				return _syncTime;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncTime))
				{
					short syncTime = _syncTime;
					SetSyncVar(value, ref _syncTime, 2uL);
				}
			}
		}

		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if ((_cooldownStopwatch.IsRunning && _cooldownStopwatch.Elapsed.TotalSeconds < (double)_targetCooldown) || (colliderId != 0 && !HasFlag(_flags, global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Open)))
			{
				return;
			}
			_cooldownStopwatch.Stop();
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerInteractGenerator, ply, this, (global::MapGeneration.Distributors.Scp079Generator.GeneratorColliderId)colliderId))
			{
				_cooldownStopwatch.Restart();
				return;
			}
			switch ((global::MapGeneration.Distributors.Scp079Generator.GeneratorColliderId)colliderId)
			{
			case global::MapGeneration.Distributors.Scp079Generator.GeneratorColliderId.Door:
				if (HasFlag(_flags, global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Unlocked))
				{
					if (HasFlag(_flags, global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Open))
					{
						if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerCloseGenerator, ply, this))
						{
							break;
						}
					}
					else if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerOpenGenerator, ply, this))
					{
						break;
					}
					ServerSetFlag(global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Open, !HasFlag(_flags, global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Open));
					_targetCooldown = _doorToggleCooldownTime;
				}
				else if (!ply.serverRoles.BypassMode && (!(ply.inventory.CurInstance != null) || !(ply.inventory.CurInstance is global::InventorySystem.Items.Keycards.KeycardItem keycardItem) || !global::Interactables.Interobjects.DoorUtils.DoorPermissionUtils.HasFlagFast(keycardItem.Permissions, _requiredPermission)))
				{
					_targetCooldown = _unlockCooldownTime;
					RpcDenied();
				}
				else if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerUnlockGenerator, ply, this))
				{
					ServerSetFlag(global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Unlocked, state: true);
					ServerGrantTicketsConditionally(new global::Footprinting.Footprint(ply), 0.5f);
				}
				break;
			case global::MapGeneration.Distributors.Scp079Generator.GeneratorColliderId.Switch:
				if ((!global::PlayerRoles.PlayerRolesUtils.IsHuman(ply) && !Activating) || Engaged)
				{
					break;
				}
				if (!Activating)
				{
					if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerActivateGenerator, ply, this))
					{
						break;
					}
				}
				else if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDeactivatedGenerator, ply, this))
				{
					break;
				}
				Activating = !Activating;
				if (Activating)
				{
					_leverStopwatch.Restart();
					_lastActivator = new global::Footprinting.Footprint(ply);
				}
				else
				{
					_lastActivator = default(global::Footprinting.Footprint);
				}
				_targetCooldown = _doorToggleCooldownTime;
				break;
			case global::MapGeneration.Distributors.Scp079Generator.GeneratorColliderId.CancelButton:
				if (Activating && !Engaged && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDeactivatedGenerator, ply, this))
				{
					ServerSetFlag(global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags.Activating, state: false);
					_targetCooldown = _unlockCooldownTime;
					_lastActivator = default(global::Footprinting.Footprint);
				}
				break;
			default:
				_targetCooldown = 1f;
				break;
			}
			_cooldownStopwatch.Restart();
		}

		private void Start()
		{
			global::PlayerRoles.PlayableScps.Scp079.Scp079Recontainer.AllGenerators.Add(this);
		}

		private void OnDestroy()
		{
			global::PlayerRoles.PlayableScps.Scp079.Scp079Recontainer.AllGenerators.Remove(this);
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active)
			{
				ServerUpdate();
			}
		}

		[global::Mirror.Server]
		private void ServerGrantTicketsConditionally(global::Footprinting.Footprint ply, float reward)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void MapGeneration.Distributors.Scp079Generator::ServerGrantTicketsConditionally(Footprinting.Footprint,System.Single)' called when server was not active");
			}
			else if (ply.IsSet && global::PlayerRoles.PlayerRolesUtils.GetFaction(ply.Role) == global::PlayerRoles.Faction.FoundationStaff)
			{
				global::Respawning.RespawnTokensManager.GrantTokens(global::Respawning.SpawnableTeamType.NineTailedFox, reward);
			}
		}

		[global::Mirror.Server]
		private void ServerUpdate()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void MapGeneration.Distributors.Scp079Generator::ServerUpdate()' called when server was not active");
				return;
			}
			bool flag = _currentTime >= _totalActivationTime;
			if (!flag)
			{
				int num = global::UnityEngine.Mathf.FloorToInt(_totalActivationTime - _currentTime);
				if (num != _syncTime)
				{
					Network_syncTime = (short)num;
				}
			}
			if (ActivationReady)
			{
				if (flag && !Engaged)
				{
					global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.GeneratorActivated, this);
					Engaged = true;
					Activating = false;
					ServerGrantTicketsConditionally(_lastActivator, 1f);
					return;
				}
				_currentTime += global::UnityEngine.Time.deltaTime;
			}
			else
			{
				if (_currentTime == 0f || flag)
				{
					return;
				}
				_currentTime -= DropdownSpeed * global::UnityEngine.Time.deltaTime;
			}
			_currentTime = global::UnityEngine.Mathf.Clamp(_currentTime, 0f, _totalActivationTime);
		}

		[global::Mirror.ClientRpc]
		private void RpcDenied()
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			SendRPCInternal(typeof(global::MapGeneration.Distributors.Scp079Generator), "RpcDenied", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private bool HasFlag(byte flags, global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags flag)
		{
			return ((uint)flags & (uint)flag) == (uint)flag;
		}

		[global::Mirror.Server]
		private void ServerSetFlag(global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags flag, bool state)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void MapGeneration.Distributors.Scp079Generator::ServerSetFlag(MapGeneration.Distributors.Scp079Generator/GeneratorFlags,System.Boolean)' called when server was not active");
				return;
			}
			global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags flags = (global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags)_flags;
			flags = ((!state) ? ((global::MapGeneration.Distributors.Scp079Generator.GeneratorFlags)((uint)flags & (uint)(byte)(~(int)flag))) : (flags | flag));
			byte b = (byte)flags;
			if (b != _flags)
			{
				Network_flags = b;
			}
		}

		static Scp079Generator()
		{
			DoorAnimHash = global::UnityEngine.Animator.StringToHash("isOpen");
			LeverAnimHash = global::UnityEngine.Animator.StringToHash("isOn");
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::MapGeneration.Distributors.Scp079Generator), "RpcDenied", InvokeUserCode_RpcDenied);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcDenied()
		{
			_cooldownStopwatch.Restart();
			_targetCooldown = _deniedCooldownTime;
		}

		protected static void InvokeUserCode_RpcDenied(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcDenied called on server.");
			}
			else
			{
				((global::MapGeneration.Distributors.Scp079Generator)obj).UserCode_RpcDenied();
			}
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _flags);
				global::Mirror.NetworkWriterExtensions.WriteInt16(writer, _syncTime);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _flags);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt16(writer, _syncTime);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				byte flags = _flags;
				Network_flags = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				short syncTime = _syncTime;
				Network_syncTime = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				byte flags2 = _flags;
				Network_flags = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
			if ((num & 2L) != 0L)
			{
				short syncTime2 = _syncTime;
				Network_syncTime = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
			}
		}
	}
}
