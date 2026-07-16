namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079AuxManager : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079Role>, global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079LevelUpNotifier
	{
		[global::UnityEngine.SerializeField]
		private float[] _regenerationPerTier;

		[global::UnityEngine.SerializeField]
		private float[] _maxPerTier;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager _tierManager;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079AbilityBase[] _abilities;

		private int _abilitiesCount;

		private float _aux;

		private bool _valueDirty;

		private ushort _prevSent;

		private static string _textEtaFormat;

		private static string _textHigherTierRequired;

		private static string _textNewMaxAux;

		private ushort Compressed => (ushort)global::UnityEngine.Mathf.Min(CurrentAuxFloored, 65535);

		private float RegenSpeed
		{
			get
			{
				float num = _regenerationPerTier[_tierManager.AccessTierIndex];
				for (int i = 0; i < _abilitiesCount; i++)
				{
					num *= _abilities[i].AuxRegenMultiplier;
				}
				return num;
			}
		}

		public float CurrentAux
		{
			get
			{
				return _aux;
			}
			set
			{
				value = global::UnityEngine.Mathf.Clamp(value, 0f, MaxAux);
				if (value != _aux)
				{
					_aux = value;
					_valueDirty = true;
				}
			}
		}

		public int CurrentAuxFloored => global::UnityEngine.Mathf.FloorToInt(CurrentAux);

		public float MaxAux => _maxPerTier[_tierManager.AccessTierIndex];

		private void Start()
		{
			_textEtaFormat = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.EtaTimer);
			_textHigherTierRequired = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.HigherTierRequired);
			_textNewMaxAux = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.AuxPowerLimitIncreased);
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active)
			{
				Regenerate();
				SyncValues();
			}
		}

		private void Regenerate()
		{
			CurrentAux += global::UnityEngine.Time.deltaTime * RegenSpeed;
		}

		private void OnRoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prev, global::PlayerRoles.PlayerRoleBase cur)
		{
			if (global::Mirror.NetworkServer.active && cur is global::PlayerRoles.Spectating.SpectatorRole)
			{
				ServerSendRpc(hub);
			}
		}

		private void SyncValues()
		{
			if (!_valueDirty)
			{
				return;
			}
			ushort compressed = Compressed;
			_valueDirty = false;
			if (compressed != _prevSent)
			{
				_prevSent = compressed;
				ServerSendRpc((ReferenceHub x) => x == base.Owner || global::PlayerRoles.Spectating.SpectatorNetworking.IsSpectatedBy(base.Owner, x));
			}
		}

		public string GenerateETA(float requiredAux)
		{
			if (requiredAux > MaxAux)
			{
				return _textHigherTierRequired;
			}
			float regenSpeed = RegenSpeed;
			if (regenSpeed <= 0f)
			{
				return string.Empty;
			}
			float num = global::UnityEngine.Mathf.Max(0f, requiredAux - CurrentAux);
			return GenerateCustomETA(global::UnityEngine.Mathf.CeilToInt(num / regenSpeed));
		}

		public string GenerateCustomETA(int secondsRemaining)
		{
			return string.Format(_textEtaFormat, secondsRemaining);
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule subroutineModule = base.ScpRole.SubroutineModule;
			subroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager>(out _tierManager);
			int num = subroutineModule.AllSubroutines.Length;
			_abilities = new global::PlayerRoles.PlayableScps.Scp079.Scp079AbilityBase[num];
			_abilitiesCount = 0;
			for (int i = 0; i < num; i++)
			{
				if (subroutineModule.AllSubroutines[i] is global::PlayerRoles.PlayableScps.Scp079.Scp079AbilityBase scp079AbilityBase)
				{
					_abilities[_abilitiesCount] = scp079AbilityBase;
					_abilitiesCount++;
				}
			}
			CurrentAux = _maxPerTier[0];
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, _prevSent);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (!global::Mirror.NetworkServer.active)
			{
				CurrentAux = (int)global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			}
		}

		public bool WriteLevelUpNotification(global::System.Text.StringBuilder sb, int newLevel)
		{
			sb.AppendFormat(_textNewMaxAux, _maxPerTier[newLevel]);
			return true;
		}
	}
}
