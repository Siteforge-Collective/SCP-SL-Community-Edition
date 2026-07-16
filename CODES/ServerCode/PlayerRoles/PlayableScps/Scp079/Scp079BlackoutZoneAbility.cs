namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079BlackoutZoneAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase, global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079LevelUpNotifier
	{
		private enum ValidationError
		{
			None = 0,
			NotEnoughAux = 1,
			Cooldown = 59,
			Unavailable = 61
		}

		[global::UnityEngine.SerializeField]
		private int _cost;

		[global::UnityEngine.SerializeField]
		private float _duration;

		[global::UnityEngine.SerializeField]
		private float _cooldown;

		[global::UnityEngine.SerializeField]
		private int _minTierIndex;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.FacilityZone[] _availableZones;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _cooldownTimer = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private string _nameFormat;

		private string _textUnlock;

		private string _failMessage;

		private bool _hasFailMessage;

		private global::MapGeneration.FacilityZone _syncZone;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation _failReason;

		public override ActionName ActivationKey => ActionName.Shoot;

		public override bool IsReady => ErrorCode == global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;

		public override string AbilityName => string.Format(_nameFormat, _cost);

		public override bool IsVisible
		{
			get
			{
				if (global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive && Unlocked && !global::UnityEngine.Cursor.visible && global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapState)
				{
					return _syncZone != global::MapGeneration.FacilityZone.None;
				}
				return false;
			}
		}

		public bool Unlocked => base.TierManager.AccessTierIndex >= _minTierIndex;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation ErrorCode
		{
			get
			{
				if (!Unlocked)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ZoneBlackoutUnavailable;
				}
				if (!_availableZones.Contains(_syncZone))
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ZoneBlackoutUnavailable;
				}
				if (!_cooldownTimer.IsReady)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ZoneBlackoutCooldown;
				}
				if (base.AuxManager.CurrentAuxFloored < _cost)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux;
				}
				return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
			}
		}

		public override string FailMessage
		{
			get
			{
				if (!_hasFailMessage)
				{
					return null;
				}
				switch (_failReason)
				{
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom:
					return null;
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux:
					if (base.AuxManager.CurrentAuxFloored >= _cost)
					{
						return null;
					}
					return GetNoAuxMessage(_cost);
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ZoneBlackoutCooldown:
					if (_cooldownTimer.IsReady)
					{
						return null;
					}
					return _failMessage + "\n" + base.AuxManager.GenerateCustomETA(global::UnityEngine.Mathf.CeilToInt(_cooldownTimer.Remaining));
				default:
					return _failMessage;
				}
			}
		}

		protected override void Start()
		{
			base.Start();
			_nameFormat = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ActivateZoneBlackout);
			_textUnlock = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ZoneBlackoutAvailable);
		}

		protected override void Update()
		{
			base.Update();
			if (base.Owner.isLocalPlayer)
			{
				_syncZone = global::PlayerRoles.PlayableScps.Scp079.Map.ZoneBlackoutIcon.HighlightedZone;
			}
		}

		public override void OnFailMessageAssigned()
		{
			base.OnFailMessageAssigned();
			_hasFailMessage = true;
			_failReason = ErrorCode;
			_failMessage = Translations.Get(_failReason);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_cooldownTimer.Clear();
		}

		public bool WriteLevelUpNotification(global::System.Text.StringBuilder sb, int newLevel)
		{
			if (newLevel != _minTierIndex)
			{
				return false;
			}
			sb.Append(_textUnlock);
			return true;
		}

		protected override void Trigger()
		{
			ClientSendCmd();
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			writer.WriteByte((byte)_syncZone);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			_syncZone = (global::MapGeneration.FacilityZone)reader.ReadByte();
			if (ErrorCode != global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079BlackoutZone, base.Owner, _syncZone))
			{
				return;
			}
			foreach (FlickerableLightController instance in FlickerableLightController.Instances)
			{
				if (instance.Room.Zone == _syncZone)
				{
					instance.ServerFlickerLights(_duration);
				}
			}
			_cooldownTimer.Trigger(_cooldown);
			base.AuxManager.CurrentAux -= _cost;
			ServerSendRpc(toAll: true);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)_syncZone);
			_cooldownTimer.WriteCooldown(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
		}
	}
}
