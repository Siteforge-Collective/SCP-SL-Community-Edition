namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079TierManager : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079Role>
	{
		private readonly global::System.Collections.Generic.Queue<global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation, int>> _expGainQueue = new global::System.Collections.Generic.Queue<global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation, int>>();

		private int _totalExp;

		private bool _valueDirty;

		private int _accessTier;

		private int _thresholdsCount;

		[global::UnityEngine.SerializeField]
		private int[] _levelupThresholds;

		public global::System.Action OnLevelledUp;

		public global::System.Action OnTierChanged;

		public global::System.Action OnExpChanged;

		public int[] AbsoluteThresholds { get; private set; }

		public int TotalExp
		{
			get
			{
				return _totalExp;
			}
			set
			{
				_totalExp = value;
				OnExpChanged?.Invoke();
				int num = 0;
				for (int i = 0; i < _thresholdsCount && _totalExp >= AbsoluteThresholds[i]; i++)
				{
					num++;
				}
				AccessTierIndex = num;
				if (global::Mirror.NetworkServer.active)
				{
					_valueDirty = true;
				}
			}
		}

		public int RelativeExp
		{
			get
			{
				int num = AccessTierIndex - 1;
				if (num < 0)
				{
					return global::UnityEngine.Mathf.FloorToInt(TotalExp);
				}
				float f = TotalExp - AbsoluteThresholds[num];
				return global::UnityEngine.Mathf.Min(NextLevelThreshold, global::UnityEngine.Mathf.FloorToInt(f));
			}
		}

		public int NextLevelThreshold
		{
			get
			{
				if (AccessTierIndex >= _thresholdsCount)
				{
					return -1;
				}
				return _levelupThresholds[AccessTierIndex];
			}
		}

		public int AccessTierIndex
		{
			get
			{
				return global::UnityEngine.Mathf.Clamp(_accessTier, 0, _thresholdsCount);
			}
			private set
			{
				if (_accessTier != value)
				{
					int num = value - _accessTier;
					for (int i = 0; i < num; i++)
					{
						_accessTier++;
						OnLevelledUp?.Invoke();
					}
					if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079LevelUpTier, base.Owner, value + 1))
					{
						_accessTier = value;
						OnTierChanged?.Invoke();
					}
				}
			}
		}

		public int AccessTierLevel => AccessTierIndex + 1;

		private void Update()
		{
			if (global::Mirror.NetworkServer.active && _valueDirty)
			{
				ServerSendRpc(toAll: true);
				_valueDirty = false;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			int num = 0;
			_thresholdsCount = _levelupThresholds.Length;
			AbsoluteThresholds = new int[_thresholdsCount];
			for (int i = 0; i < _thresholdsCount; i++)
			{
				num += _levelupThresholds[i];
				AbsoluteThresholds[i] = num;
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			TotalExp = 0;
		}

		public void ServerGrantExperience(int amount, global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation reason)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("SCP-079 experience cannot be granted by local player!");
			}
			if (amount > 0)
			{
				_expGainQueue.Enqueue(new global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation, int>(reason, amount));
				TotalExp += amount;
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, (ushort)TotalExp);
			global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation, int> element;
			while (CollectionExtensions.TryDequeue(_expGainQueue, out element))
			{
				writer.WriteByte((byte)element.Key);
				writer.WriteByte((byte)global::UnityEngine.Mathf.Min(element.Value, 255));
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			ushort totalExp = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			if (!global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive && !base.ScpRole.IsSpectated && !global::Mirror.NetworkServer.active)
			{
				TotalExp = totalExp;
				return;
			}
			while (reader.Position < reader.Length)
			{
				byte translation = reader.ReadByte();
				int num = reader.ReadByte();
				global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationManager.AddNotification((global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation)translation, num);
			}
			if (!global::Mirror.NetworkServer.active)
			{
				TotalExp = totalExp;
			}
		}
	}
}
