namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079BlackoutRoomAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase, global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079LevelUpNotifier
	{
		private enum ValidationError
		{
			None = 0,
			NotEnoughAux = 1,
			NoController = 26,
			MaxCapacityReached = 27,
			RoomOnCooldown = 28,
			AlreadyBlackedOut = 60
		}

		[global::UnityEngine.SerializeField]
		private int[] _capacityPerTier;

		[global::UnityEngine.SerializeField]
		private float _blackoutDuration;

		[global::UnityEngine.SerializeField]
		private float _cooldown;

		[global::UnityEngine.SerializeField]
		private int _cost;

		private string _textUnlock;

		private string _textCapacityIncreased;

		private string _nameFormat;

		private string _failMessage;

		private bool _hasFailMessage;

		private bool _hasController;

		private FlickerableLightController _successfulController;

		private FlickerableLightController _roomController;

		private readonly global::System.Collections.Generic.Dictionary<uint, double> _blackoutCooldowns = new global::System.Collections.Generic.Dictionary<uint, double>();

		private readonly global::System.Collections.Generic.HashSet<uint> _obsoleteCooldowns = new global::System.Collections.Generic.HashSet<uint>();

		public override ActionName ActivationKey => ActionName.Scp079Blackout;

		public override bool IsReady => ErrorCode == global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;

		public override bool IsVisible
		{
			get
			{
				if (CurrentCapacity > 0)
				{
					return !global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras;
				}
				return false;
			}
		}

		public override string AbilityName => string.Format(_nameFormat, _cost);

		public override string FailMessage
		{
			get
			{
				if (!_hasFailMessage)
				{
					return null;
				}
				switch (ErrorCode)
				{
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom:
					return null;
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux:
					return GetNoAuxMessage(_cost);
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutRoomCooldown:
					return _failMessage + "\n" + base.AuxManager.GenerateCustomETA(global::UnityEngine.Mathf.CeilToInt(RemainingCooldown));
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutRoomLimit:
					return string.Format(_failMessage, RoomsOnCooldown, CurrentCapacity);
				default:
					return _failMessage;
				}
			}
		}

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.AudioClip ConfirmationSound { get; private set; }

		private int CurrentCapacity => GetCapacityOfTier(base.TierManager.AccessTierIndex);

		private int RoomsOnCooldown
		{
			get
			{
				int num = 0;
				bool flag = false;
				foreach (global::System.Collections.Generic.KeyValuePair<uint, double> blackoutCooldown in _blackoutCooldowns)
				{
					if (blackoutCooldown.Value < global::Mirror.NetworkTime.time)
					{
						_obsoleteCooldowns.Add(blackoutCooldown.Key);
						flag = true;
					}
					else
					{
						num++;
					}
				}
				if (!flag)
				{
					return num;
				}
				foreach (uint obsoleteCooldown in _obsoleteCooldowns)
				{
					_blackoutCooldowns.Remove(obsoleteCooldown);
				}
				_obsoleteCooldowns.Clear();
				return num;
			}
		}

		private float RemainingCooldown
		{
			get
			{
				if (!_hasController || !_blackoutCooldowns.TryGetValue(_roomController.netId, out var value))
				{
					return 0f;
				}
				double num = value - global::Mirror.NetworkTime.time;
				return global::UnityEngine.Mathf.Max(0f, (float)num);
			}
		}

		private global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation ErrorCode
		{
			get
			{
				if (!_hasController)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutRoomUnavailable;
				}
				if (!_roomController.LightsEnabled)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutAlreadyActive;
				}
				if (RemainingCooldown > 0f)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutRoomCooldown;
				}
				if (RoomsOnCooldown >= CurrentCapacity)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutRoomLimit;
				}
				if (base.AuxManager.CurrentAuxFloored < _cost)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux;
				}
				return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
			}
		}

		private void RefreshCurrentController()
		{
			_hasController = false;
			_hasFailMessage = false;
			_failMessage = null;
			global::MapGeneration.RoomIdentifier room = base.CurrentCamSync.CurrentCamera.Room;
			foreach (FlickerableLightController instance in FlickerableLightController.Instances)
			{
				if (!(instance.Room != room))
				{
					float y = base.CurrentCamSync.CurrentCamera.Position.y;
					float y2 = instance.transform.position.y;
					if (!(global::UnityEngine.Mathf.Abs(y - y2) > 100f))
					{
						_roomController = instance;
						_hasController = true;
						break;
					}
				}
			}
		}

		private int GetCapacityOfTier(int index)
		{
			index = global::UnityEngine.Mathf.Clamp(index, 0, _capacityPerTier.Length - 1);
			return _capacityPerTier[index];
		}

		protected override void Start()
		{
			base.Start();
			_nameFormat = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ActivateRoomBlackout);
			_textUnlock = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutRoomAvailable);
			_textCapacityIncreased = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.BlackoutCapacityIncreased);
			base.CurrentCamSync.OnCameraChanged += RefreshCurrentController;
		}

		protected override void Trigger()
		{
			ClientSendCmd();
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (IsReady && !base.LostSignalHandler.Lost)
			{
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079BlackoutRoom, base.Owner, _roomController.Room))
				{
					base.AuxManager.CurrentAux -= _cost;
					base.RewardManager.MarkRoom(_roomController.Room);
					_blackoutCooldowns[_roomController.netId] = global::Mirror.NetworkTime.time + (double)_cooldown;
					_roomController.ServerFlickerLights(_blackoutDuration);
					_successfulController = _roomController;
					ServerSendRpc(toAll: true);
				}
			}
			else
			{
				_successfulController = null;
				ServerSendRpc(toAll: false);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteNetworkBehaviour(writer, _successfulController);
			writer.WriteByte((byte)RoomsOnCooldown);
			foreach (global::System.Collections.Generic.KeyValuePair<uint, double> blackoutCooldown in _blackoutCooldowns)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, blackoutCooldown.Key);
				global::Mirror.NetworkWriterExtensions.WriteDouble(writer, blackoutCooldown.Value);
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_successfulController = global::Mirror.NetworkReaderExtensions.ReadNetworkBehaviour<FlickerableLightController>(reader);
			if (_successfulController != null)
			{
				PlaySoundForController(_successfulController);
			}
			int num = reader.ReadByte();
			_blackoutCooldowns.Clear();
			for (int i = 0; i < num; i++)
			{
				uint key = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
				double value = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
				_blackoutCooldowns.Add(key, value);
			}
		}

		public void PlaySoundForController(FlickerableLightController flc)
		{
			global::UnityEngine.Vector3 position = flc.transform.position + global::UnityEngine.Vector3.down * 15f;
			global::AudioPooling.AudioSourcePoolManager.PlaySound(ConfirmationSound, position, 37f, 1f, FalloffType.Linear).minDistance = 15f;
		}

		public override void OnFailMessageAssigned()
		{
			base.OnFailMessageAssigned();
			_hasFailMessage = true;
			_failMessage = Translations.Get(ErrorCode);
		}

		public bool WriteLevelUpNotification(global::System.Text.StringBuilder sb, int newLevel)
		{
			int capacityOfTier = GetCapacityOfTier(newLevel);
			int capacityOfTier2 = GetCapacityOfTier(newLevel - 1);
			if (capacityOfTier <= capacityOfTier2)
			{
				return false;
			}
			if (capacityOfTier2 > 0)
			{
				sb.AppendFormat(_textCapacityIncreased, capacityOfTier);
			}
			else
			{
				sb.AppendFormat(_textUnlock, $"[{new ReadableKeyCode(ActivationKey)}]");
			}
			return true;
		}
	}
}
