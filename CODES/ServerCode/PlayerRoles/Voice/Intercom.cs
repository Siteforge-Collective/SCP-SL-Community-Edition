namespace PlayerRoles.Voice
{
	public class Intercom : global::Mirror.NetworkBehaviour
	{
		private static global::PlayerRoles.Voice.Intercom _singleton;

		private static bool _singletonSet;

		private readonly global::System.Diagnostics.Stopwatch _sustain = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Diagnostics.Stopwatch _clipSw = global::System.Diagnostics.Stopwatch.StartNew();

		private readonly global::System.Collections.Generic.HashSet<ReferenceHub> _adminOverrides = new global::System.Collections.Generic.HashSet<ReferenceHub>();

		private ReferenceHub _curSpeaker;

		private float _cooldownTime;

		private float _speechTime;

		private float _rangeSqr;

		private global::UnityEngine.Vector3 _worldPos;

		[global::UnityEngine.SerializeField]
		private float _range;

		[global::UnityEngine.SerializeField]
		private float _wakeupTime;

		[global::UnityEngine.SerializeField]
		private float _sustainTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _startClip;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _endClip;

		[global::UnityEngine.SerializeField]
		private float _clipCooldown;

		[global::Mirror.SyncVar]
		private byte _state;

		[global::Mirror.SyncVar]
		private double _nextTime;

		public static global::PlayerRoles.Voice.IntercomState State
		{
			get
			{
				if (_singletonSet)
				{
					return (global::PlayerRoles.Voice.IntercomState)_singleton._state;
				}
				return global::PlayerRoles.Voice.IntercomState.NotFound;
			}
			set
			{
				if (!_singletonSet || !global::Mirror.NetworkServer.active)
				{
					return;
				}
				global::PlayerRoles.Voice.Intercom singleton = _singleton;
				switch (value)
				{
				case global::PlayerRoles.Voice.IntercomState.InUse:
					singleton.Network_nextTime = global::Mirror.NetworkTime.time + (double)singleton._speechTime;
					break;
				case global::PlayerRoles.Voice.IntercomState.Starting:
					singleton.Network_nextTime = global::Mirror.NetworkTime.time + (double)singleton._wakeupTime;
					singleton.RpcPlayClip(state: true);
					break;
				case global::PlayerRoles.Voice.IntercomState.Cooldown:
					singleton.RpcPlayClip(state: false);
					if (singleton._curSpeaker != null && singleton._curSpeaker.serverRoles.BypassMode)
					{
						singleton.Network_nextTime = 0.0;
					}
					else
					{
						singleton.Network_nextTime = global::Mirror.NetworkTime.time + (double)singleton._cooldownTime;
					}
					break;
				}
				singleton.Network_state = (byte)value;
			}
		}

		public float RemainingTime => global::UnityEngine.Mathf.Max((float)(_nextTime - global::Mirror.NetworkTime.time), 0f);

		public bool BypassMode
		{
			get
			{
				if (State == global::PlayerRoles.Voice.IntercomState.InUse)
				{
					return _nextTime == 0.0;
				}
				return false;
			}
		}

		public byte Network_state
		{
			get
			{
				return _state;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _state))
				{
					byte state = _state;
					SetSyncVar(value, ref _state, 1uL);
				}
			}
		}

		public double Network_nextTime
		{
			get
			{
				return _nextTime;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _nextTime))
				{
					double nextTime = _nextTime;
					SetSyncVar(value, ref _nextTime, 2uL);
				}
			}
		}

		private void Awake()
		{
			if (_singletonSet)
			{
				throw new global::System.InvalidOperationException("Multiple instances of Intercom detected. Last name: '" + base.name + "'");
			}
			_singleton = this;
			_singletonSet = true;
			global::GameCore.ConfigFile.OnConfigReloaded = (global::System.Action)global::System.Delegate.Combine(global::GameCore.ConfigFile.OnConfigReloaded, new global::System.Action(ReloadConfigs));
			global::MapGeneration.SeedSynchronizer.OnMapGenerated += SetupPos;
			ReloadConfigs();
			if (global::MapGeneration.SeedSynchronizer.MapGenerated)
			{
				SetupPos();
			}
		}

		private void Update()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			switch (State)
			{
			case global::PlayerRoles.Voice.IntercomState.Ready:
			{
				if (global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(ReferenceHub.AllHubs, CheckPlayer, out var first))
				{
					_curSpeaker = first;
					State = global::PlayerRoles.Voice.IntercomState.Starting;
				}
				break;
			}
			case global::PlayerRoles.Voice.IntercomState.Starting:
				if (!(_nextTime > global::Mirror.NetworkTime.time))
				{
					_sustain.Restart();
					State = global::PlayerRoles.Voice.IntercomState.InUse;
				}
				break;
			case global::PlayerRoles.Voice.IntercomState.InUse:
			{
				bool flag;
				if (_curSpeaker != null && CheckPlayer(_curSpeaker))
				{
					flag = true;
					_sustain.Restart();
				}
				else
				{
					flag = _sustain.Elapsed.TotalSeconds < (double)_sustainTime;
				}
				if (!flag || (!(_nextTime > global::Mirror.NetworkTime.time) && _nextTime != 0.0))
				{
					State = global::PlayerRoles.Voice.IntercomState.Cooldown;
				}
				break;
			}
			case global::PlayerRoles.Voice.IntercomState.Cooldown:
				if (!(_nextTime > global::Mirror.NetworkTime.time))
				{
					State = global::PlayerRoles.Voice.IntercomState.Ready;
				}
				break;
			}
		}

		private void OnDestroy()
		{
			global::GameCore.ConfigFile.OnConfigReloaded = (global::System.Action)global::System.Delegate.Remove(global::GameCore.ConfigFile.OnConfigReloaded, new global::System.Action(ReloadConfigs));
			global::MapGeneration.SeedSynchronizer.OnMapGenerated -= SetupPos;
			_singletonSet = false;
		}

		private void OnDrawGizmosSelected()
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.green;
			global::UnityEngine.Gizmos.DrawWireSphere(base.transform.position, _range);
		}

		private void SetupPos()
		{
			_worldPos = base.transform.position;
			_rangeSqr = _range * _range;
		}

		private void ReloadConfigs()
		{
			_cooldownTime = global::GameCore.ConfigFile.ServerConfig.GetFloat("intercom_cooldown", 120f);
			_speechTime = global::GameCore.ConfigFile.ServerConfig.GetFloat("intercom_max_speech_time", 20f);
		}

		private bool CheckRange(ReferenceHub hub)
		{
			if (hub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole)
			{
				return (humanRole.FpcModule.Position - _worldPos).sqrMagnitude < _rangeSqr;
			}
			return false;
		}

		private bool CheckPlayer(ReferenceHub hub)
		{
			global::PlayerRoles.PlayerRoleBase currentRole = hub.roleManager.CurrentRole;
			if (!CheckRange(hub) || !(currentRole as global::PlayerRoles.HumanRole).VoiceModule.ServerIsSending || global::VoiceChat.VoiceChatMutes.GetFlags(hub) != global::VoiceChat.VcMuteFlags.None)
			{
				return false;
			}
			return global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerUsingIntercom, hub, State);
		}

		[global::Mirror.ClientRpc]
		private void RpcPlayClip(bool state)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, state);
			SendRPCInternal(typeof(global::PlayerRoles.Voice.Intercom), "RpcPlayClip", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		public static bool CheckPerms(ReferenceHub hub)
		{
			if (!_singletonSet)
			{
				return false;
			}
			if (global::VoiceChat.VoiceChatMutes.GetFlags(hub) != global::VoiceChat.VcMuteFlags.None)
			{
				return false;
			}
			bool flag = State == global::PlayerRoles.Voice.IntercomState.InUse;
			if (!_singleton._adminOverrides.Contains(hub))
			{
				if (flag && _singleton.CheckRange(hub))
				{
					return _singleton._curSpeaker == hub;
				}
				return false;
			}
			return true;
		}

		public static bool TrySetOverride(ReferenceHub ply, bool newState)
		{
			if (!_singletonSet || ply == null)
			{
				return false;
			}
			global::System.Collections.Generic.HashSet<ReferenceHub> adminOverrides = _singleton._adminOverrides;
			if (!newState)
			{
				return adminOverrides.Remove(ply);
			}
			adminOverrides.Add(ply);
			return true;
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPlayClip(bool state)
		{
			if (!(_clipSw.Elapsed.TotalSeconds < (double)_clipCooldown))
			{
				global::AudioPooling.AudioSourcePoolManager.PlaySound(state ? _startClip : _endClip, null, 1f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.VoiceChat, 0f);
				_clipSw.Restart();
			}
		}

		protected static void InvokeUserCode_RpcPlayClip(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcPlayClip called on server.");
			}
			else
			{
				((global::PlayerRoles.Voice.Intercom)obj).UserCode_RpcPlayClip(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}

		static Intercom()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::PlayerRoles.Voice.Intercom), "RpcPlayClip", InvokeUserCode_RpcPlayClip);
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _state);
				global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _nextTime);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _state);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _nextTime);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				byte state = _state;
				Network_state = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				double nextTime = _nextTime;
				Network_nextTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				byte state2 = _state;
				Network_state = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
			if ((num & 2L) != 0L)
			{
				double nextTime2 = _nextTime;
				Network_nextTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
			}
		}
	}
}
