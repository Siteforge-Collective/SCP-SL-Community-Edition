namespace PlayerRoles.Voice
{
	public class IntercomDisplay : global::Mirror.NetworkBehaviour
	{
		private enum IcomText
		{
			Ready = 0,
			Transmitting = 1,
			TrasmittingBypass = 2,
			Restarting = 3,
			AdminUsing = 4,
			Muted = 5,
			Unknown = 6,
			Wait = 7
		}

		private static global::PlayerRoles.Voice.IntercomDisplay _singleton;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Text _text;

		[global::Mirror.SyncVar]
		private string _overrideText;

		private global::PlayerRoles.Voice.Intercom _icom;

		private string[] _translations;

		private bool[] _translationsSet;

		public string Network_overrideText
		{
			get
			{
				return _overrideText;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _overrideText))
				{
					string overrideText = _overrideText;
					SetSyncVar(value, ref _overrideText, 1uL);
				}
			}
		}

		private void Awake()
		{
			_icom = GetComponent<global::PlayerRoles.Voice.Intercom>();
			_singleton = this;
			int num = 0;
			foreach (int value in global::System.Enum.GetValues(typeof(global::PlayerRoles.Voice.IntercomDisplay.IcomText)))
			{
				num = global::UnityEngine.Mathf.Max(num, value + 1);
			}
			_translations = new string[num];
			_translationsSet = new bool[num];
		}

		private void Update()
		{
			if (!string.IsNullOrEmpty(_overrideText))
			{
				_text.text = _overrideText;
				return;
			}
			switch (global::PlayerRoles.Voice.Intercom.State)
			{
			case global::PlayerRoles.Voice.IntercomState.Ready:
			{
				bool flag = global::VoiceChat.VoiceChatMuteIndicator.ReceivedFlags != global::VoiceChat.VcMuteFlags.None;
				_text.text = GetTranslation(flag ? global::PlayerRoles.Voice.IntercomDisplay.IcomText.Muted : global::PlayerRoles.Voice.IntercomDisplay.IcomText.Ready);
				break;
			}
			case global::PlayerRoles.Voice.IntercomState.Starting:
				_text.text = GetTranslation(global::PlayerRoles.Voice.IntercomDisplay.IcomText.Wait);
				break;
			case global::PlayerRoles.Voice.IntercomState.InUse:
				_text.text = (_icom.BypassMode ? GetTranslation(global::PlayerRoles.Voice.IntercomDisplay.IcomText.TrasmittingBypass) : string.Format(GetTranslation(global::PlayerRoles.Voice.IntercomDisplay.IcomText.Transmitting), global::UnityEngine.Mathf.Round(_icom.RemainingTime)));
				break;
			case global::PlayerRoles.Voice.IntercomState.Cooldown:
				_text.text = string.Format(GetTranslation(global::PlayerRoles.Voice.IntercomDisplay.IcomText.Restarting), global::UnityEngine.Mathf.Round(_icom.RemainingTime));
				break;
			case global::PlayerRoles.Voice.IntercomState.NotFound:
				_text.text = GetTranslation(global::PlayerRoles.Voice.IntercomDisplay.IcomText.Unknown);
				break;
			}
		}

		private string GetTranslation(global::PlayerRoles.Voice.IntercomDisplay.IcomText val)
		{
			int num = (int)val;
			if (!_translationsSet[num])
			{
				_translations[num] = TranslationReader.Get("Intercom", num, val.ToString());
				_translationsSet[num] = true;
			}
			return _translations[num];
		}

		public static bool TrySetDisplay(string str)
		{
			if (_singleton == null)
			{
				return false;
			}
			_singleton.Network_overrideText = str;
			return true;
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteString(writer, _overrideText);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteString(writer, _overrideText);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				string overrideText = _overrideText;
				Network_overrideText = global::Mirror.NetworkReaderExtensions.ReadString(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				string overrideText2 = _overrideText;
				Network_overrideText = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			}
		}
	}
}
