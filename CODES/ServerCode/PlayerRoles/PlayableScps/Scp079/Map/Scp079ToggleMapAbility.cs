namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079ToggleMapAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase, global::GameObjectPools.IPoolSpawnable, global::GameObjectPools.IPoolResettable
	{
		[global::UnityEngine.SerializeField]
		private float _cooldown;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _soundOpen;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _soundClose;

		private bool _state;

		private string _openTxt;

		private string _closeTxt;

		private static bool _localInstanceReady;

		private static global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility _localInstance;

		private static readonly global::System.Diagnostics.Stopwatch CooldownSw = global::System.Diagnostics.Stopwatch.StartNew();

		public override ActionName ActivationKey => ActionName.Inventory;

		public override bool IsReady => CooldownSw.Elapsed.TotalSeconds > (double)_cooldown;

		public override bool IsVisible
		{
			get
			{
				if (global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras)
				{
					if (_state)
					{
						return !global::UnityEngine.Cursor.visible;
					}
					return false;
				}
				return true;
			}
		}

		public override string AbilityName
		{
			get
			{
				if (!_state)
				{
					return _openTxt;
				}
				return _closeTxt;
			}
		}

		public override string FailMessage => null;

		public static bool MapState
		{
			get
			{
				if (_localInstanceReady)
				{
					return _localInstance._state;
				}
				return false;
			}
			internal set
			{
				if (_localInstanceReady)
				{
					_localInstance._state = value;
					CooldownSw.Restart();
					if (!value && _localInstance.Role.IsLocalPlayer)
					{
						_localInstance.ClientSendCmd();
					}
				}
			}
		}

		public static bool MapVisible
		{
			get
			{
				if (_localInstanceReady)
				{
					if (!_localInstance._state)
					{
						return !_localInstance.IsReady;
					}
					return true;
				}
				return false;
			}
		}

		private void OnSpectatorTargetChanged()
		{
			if (base.ScpRole.IsSpectated || base.ScpRole.IsLocalPlayer)
			{
				_localInstance = this;
				_localInstanceReady = true;
			}
			else
			{
				_localInstanceReady = false;
			}
		}

		private void LateUpdate()
		{
			if (base.Role.IsLocalPlayer && _state)
			{
				ClientSendCmd();
			}
		}

		private void PlaySound()
		{
			global::AudioPooling.AudioSourcePoolManager.PlaySound(_state ? _soundOpen : _soundClose, null, 1f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.DefaultSfx, 0f);
		}

		protected override void Trigger()
		{
			if (base.CurrentCamSync.CurClientSwitchState == global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None)
			{
				_state = !_state;
				PlaySound();
				if (!_state)
				{
					ClientSendCmd();
				}
			}
		}

		protected override void Start()
		{
			base.Start();
			_openTxt = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.OpenMap);
			_closeTxt = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.CloseMap);
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			if (_state)
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.SyncVars);
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (reader.Position < reader.Length)
			{
				_state = true;
				global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.SyncVars = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
				ServerSendRpc((ReferenceHub x) => x.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole);
			}
			else
			{
				_state = false;
				ServerSendRpc((ReferenceHub x) => x != base.Owner);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			if (_state)
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.SyncVars);
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			bool state = _state;
			if (reader.Position < reader.Length)
			{
				_state = true;
				global::PlayerRoles.PlayableScps.Scp079.Map.Scp079MapGui.SyncVars = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
			}
			else
			{
				_state = false;
			}
			if (state != _state && global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner) && base.ScpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out var subroutine) && subroutine.CurClientSwitchState == global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None)
			{
				PlaySound();
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
			if (base.Role.IsLocalPlayer)
			{
				_localInstance = this;
				_localInstanceReady = true;
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Remove(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
			_state = false;
			if (_localInstanceReady && !(this != _localInstance))
			{
				_localInstanceReady = false;
			}
		}
	}
}
