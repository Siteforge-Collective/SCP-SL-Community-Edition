namespace VoiceChat.Playbacks
{
	public class PersonalRadioPlayback : global::VoiceChat.Playbacks.VoiceChatPlaybackBase, global::VoiceChat.Playbacks.IGlobalPlayback
	{
		public struct TransmitterPositionMessage : global::Mirror.NetworkMessage
		{
			public RecyclablePlayerId Transmitter;

			public byte WaypointId;
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _noiseSource;

		private int _currentId;

		private bool _hasProximity;

		private bool _isLocalPlayer;

		private bool _recheckCachedRadio;

		private ReferenceHub _owner;

		private global::InventorySystem.Items.Radio.RadioItem _cachedRadio;

		private global::VoiceChat.Playbacks.SingleBufferPlayback _proxPlayback;

		private readonly global::VoiceChat.Networking.PlaybackBuffer _personalBuffer = new global::VoiceChat.Networking.PlaybackBuffer();

		private const int RadioDelay = 4000;

		private const float ProxVolumeRatio = 0.35f;

		private static global::VoiceChat.Playbacks.PersonalRadioPlayback _localPlayer;

		private static bool _hasLocalPlayer;

		private static int _freeIdsCount;

		private static int _lastTopNumber;

		private static float _noiseLevel;

		private static global::InventorySystem.Items.Radio.RadioItem _templateRadio;

		private static bool _templateRadioLoaded;

		private static readonly global::System.Collections.Generic.HashSet<int> FreeIds = new global::System.Collections.Generic.HashSet<int>();

		private int RangeId
		{
			get
			{
				if (!global::InventorySystem.Items.Radio.RadioMessages.SyncedRangeLevels.TryGetValue(_owner.netId, out var value))
				{
					return 1;
				}
				return global::UnityEngine.Mathf.Abs((int)value.Range);
			}
		}

		private global::InventorySystem.Items.Radio.RadioItem RadioTemplate
		{
			get
			{
				if (_templateRadioLoaded)
				{
					return _templateRadio;
				}
				if (!global::InventorySystem.InventoryItemLoader.TryGetItem<global::InventorySystem.Items.Radio.RadioItem>(ItemType.Radio, out var result))
				{
					return null;
				}
				_templateRadioLoaded = true;
				_templateRadio = result;
				return result;
			}
		}

		private static global::VoiceChat.Playbacks.PersonalRadioPlayback LocalPlayer
		{
			get
			{
				return _localPlayer;
			}
			set
			{
				if (_hasLocalPlayer)
				{
					_localPlayer._isLocalPlayer = false;
				}
				if (value == null)
				{
					_localPlayer = null;
					_hasLocalPlayer = false;
				}
				else
				{
					_localPlayer = value;
					_hasLocalPlayer = true;
					value._isLocalPlayer = true;
				}
			}
		}

		public global::UnityEngine.Vector3 LastKnownLocation { get; private set; }

		public int TemporaryId
		{
			get
			{
				UpdateTemporaryId();
				return _currentId;
			}
		}

		public bool RadioUsable
		{
			get
			{
				if (TryGetUserRadio(out var radio))
				{
					return radio.IsUsable;
				}
				return false;
			}
		}

		public override int MaxSamples => _personalBuffer.Length;

		public bool GlobalChatActive
		{
			get
			{
				if (IsTransmitting(_owner))
				{
					return !base.Source.mute;
				}
				return false;
			}
		}

		public global::UnityEngine.Color GlobalChatColor => _owner.serverRoles.GetVoiceColor();

		public string GlobalChatName => _owner.nicknameSync.DisplayName;

		public float GlobalChatLoudness => base.Loudness;

		public global::VoiceChat.Playbacks.GlobalChatIconType GlobalChatIcon => global::VoiceChat.Playbacks.GlobalChatIconType.Radio;

		private void OnItemsModified(ReferenceHub hub)
		{
			if (!(hub != _owner))
			{
				_recheckCachedRadio = true;
			}
		}

		private void UpdateTemporaryId()
		{
			if (_personalBuffer.Length == 0)
			{
				if (_currentId != 0)
				{
					FreeIds.Add(_currentId);
					_freeIdsCount++;
					_currentId = 0;
				}
			}
			else if (_currentId == 0)
			{
				if (_freeIdsCount > 0)
				{
					_currentId = global::System.Linq.Enumerable.Min(FreeIds);
					_freeIdsCount--;
				}
				else
				{
					_currentId = ++_lastTopNumber;
				}
			}
		}

		private void UpdateLoudness()
		{
			if (!_hasLocalPlayer || _isLocalPlayer || !LocalPlayer.RadioUsable)
			{
				base.Source.mute = true;
				if (_hasProximity)
				{
					_proxPlayback.Source.volume = 1f;
				}
				return;
			}
			int num = global::UnityEngine.Mathf.Max(_localPlayer.RangeId, RangeId);
			float num2 = RadioTemplate.Ranges[num].MaximumRange;
			float sqrMagnitude = (MainCameraController.CurrentCamera.position - LastKnownLocation).sqrMagnitude;
			if (sqrMagnitude > num2 * num2)
			{
				base.Source.mute = true;
				return;
			}
			base.Source.mute = false;
			float time = global::UnityEngine.Mathf.Sqrt(sqrMagnitude) / num2;
			base.Source.volume = RadioTemplate.VoiceVolumeCurve.Evaluate(time);
			if (_personalBuffer.Length > 0)
			{
				_noiseLevel = global::UnityEngine.Mathf.Max(_noiseLevel, RadioTemplate.NoiseLevelCurve.Evaluate(time));
			}
			if (_hasProximity)
			{
				_proxPlayback.Source.volume = ((_personalBuffer.Length > 0) ? 0.35f : 1f);
			}
		}

		private void UpdateNoise()
		{
			if (_isLocalPlayer)
			{
				_noiseSource.volume = _noiseLevel;
				_noiseLevel = 0f;
			}
		}

		private bool TryGetUserRadio(out global::InventorySystem.Items.Radio.RadioItem radio)
		{
			if (_cachedRadio != null)
			{
				radio = _cachedRadio;
				return true;
			}
			if (!_recheckCachedRadio)
			{
				radio = null;
				return false;
			}
			radio = global::System.Linq.Enumerable.FirstOrDefault(_owner.inventory.UserInventory.Items, (global::System.Collections.Generic.KeyValuePair<ushort, global::InventorySystem.Items.ItemBase> x) => x.Value.ItemTypeId == ItemType.Radio).Value as global::InventorySystem.Items.Radio.RadioItem;
			if (radio == null)
			{
				return false;
			}
			_cachedRadio = radio;
			return true;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			global::InventorySystem.Inventory.OnItemsModified -= OnItemsModified;
			global::VoiceChat.GlobalChatIndicatorManager.Unsubscribe(this);
			if (_isLocalPlayer)
			{
				LocalPlayer = null;
				_noiseSource.volume = 0f;
			}
		}

		protected override void Update()
		{
			base.Update();
			UpdateTemporaryId();
			UpdateLoudness();
			UpdateNoise();
			if (global::Mirror.NetworkServer.active && IsTransmitting(_owner))
			{
				global::Mirror.NetworkServer.SendToReady(new global::VoiceChat.Playbacks.PersonalRadioPlayback.TransmitterPositionMessage
				{
					Transmitter = new RecyclablePlayerId(_owner.PlayerId),
					WaypointId = new global::RelativePositioning.RelativePosition(base.transform.position).WaypointId
				});
			}
		}

		protected override float ReadSample()
		{
			return _personalBuffer.Read();
		}

		public void Setup(ReferenceHub owner, global::VoiceChat.Playbacks.SingleBufferPlayback proximityPlayback)
		{
			_owner = owner;
			_proxPlayback = proximityPlayback;
			_personalBuffer.Clear();
			global::InventorySystem.Inventory.OnItemsModified += OnItemsModified;
			if (_owner.isLocalPlayer)
			{
				LocalPlayer = this;
			}
			else
			{
				_isLocalPlayer = false;
				global::VoiceChat.GlobalChatIndicatorManager.Subscribe(this, owner);
			}
			_hasProximity = _proxPlayback != null;
			_recheckCachedRadio = true;
		}

		public void DistributeSamples(float[] samples, int length)
		{
			_personalBuffer.Write(samples, length);
			if (_hasProximity)
			{
				_personalBuffer.SyncWith(_proxPlayback.Buffer, 4000);
			}
			int num = TemporaryId - 1;
			if (num < 0 || num >= 8)
			{
				return;
			}
			foreach (global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase allInstance in global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase.AllInstances)
			{
				if (allInstance.IgnoredNetId == _owner.netId || allInstance.Culled)
				{
					continue;
				}
				float num2 = RadioTemplate.Ranges[global::UnityEngine.Mathf.Max(allInstance.RangeId, RangeId)].MaximumRange;
				if (!((allInstance.LastPosition - LastKnownLocation).sqrMagnitude > num2 * num2))
				{
					global::VoiceChat.Networking.PlaybackBuffer playbackBuffer = allInstance.Buffers[num];
					playbackBuffer.Write(samples, length);
					if (_hasProximity)
					{
						playbackBuffer.SyncWith(_proxPlayback.Buffer, 4000);
					}
				}
			}
		}

		public static bool IsTransmitting(ReferenceHub hub)
		{
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.Voice.IVoiceRole voiceRole))
			{
				return false;
			}
			global::PlayerRoles.Voice.VoiceModuleBase voiceModule = voiceRole.VoiceModule;
			if (!(voiceModule is global::PlayerRoles.Voice.IRadioVoiceModule))
			{
				return false;
			}
			if (hub.isLocalPlayer)
			{
				return global::VoiceChat.VoiceChatMicCapture.GetCurrentChannel() == global::VoiceChat.VoiceChatChannel.Radio;
			}
			if (global::Mirror.NetworkServer.active ? voiceModule.ServerIsSending : voiceModule.IsSpeaking)
			{
				return voiceModule.CurrentChannel == global::VoiceChat.VoiceChatChannel.Radio;
			}
			return false;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkClient.ReplaceHandler(delegate(global::VoiceChat.Playbacks.PersonalRadioPlayback.TransmitterPositionMessage msg)
				{
					if (ReferenceHub.TryGetHub(msg.Transmitter.Value, out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.Voice.IVoiceRole voiceRole && voiceRole.VoiceModule is global::PlayerRoles.Voice.IRadioVoiceModule radioVoiceModule)
					{
						radioVoiceModule.RadioPlayback.LastKnownLocation = global::RelativePositioning.WaypointBase.GetWorldPosition(msg.WaypointId, global::UnityEngine.Vector3.zero);
					}
				});
			};
		}
	}
}
