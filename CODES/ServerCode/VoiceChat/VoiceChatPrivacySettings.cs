namespace VoiceChat
{
	public class VoiceChatPrivacySettings : global::ToggleableMenus.ToggleableMenuBase
	{
		[global::System.Serializable]
		private class ToggleGroup
		{
			[field: global::UnityEngine.SerializeField]
			public global::UnityEngine.UI.Toggle AcceptToggle { get; private set; }

			[field: global::UnityEngine.SerializeField]
			public global::UnityEngine.UI.Toggle DenyToggle { get; private set; }

			[field: global::UnityEngine.SerializeField]
			public global::VoiceChat.VcPrivacyFlags Flags { get; private set; }

			public bool IsAccepted
			{
				get
				{
					return AcceptToggle.isOn;
				}
				set
				{
					AcceptToggle.SetIsOnWithoutNotify(value);
					DenyToggle.SetIsOnWithoutNotify(!value);
				}
			}
		}

		public struct VcPrivacyMessage : global::Mirror.NetworkMessage
		{
			public byte Flags;
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _simplifiedRoot;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _advancedRoot;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Canvas _hideHudCanvas;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _recordDim;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _dimmerBackground;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _returnButton;

		[global::UnityEngine.SerializeField]
		private global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup[] _groups;

		private readonly global::System.Collections.Generic.Dictionary<global::VoiceChat.VcPrivacyFlags, global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup> _groupsByFlags = new global::System.Collections.Generic.Dictionary<global::VoiceChat.VcPrivacyFlags, global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup>();

		private static readonly global::System.Collections.Generic.Dictionary<ReferenceHub, global::VoiceChat.VcPrivacyFlags> FlagsOfPlayers = new global::System.Collections.Generic.Dictionary<ReferenceHub, global::VoiceChat.VcPrivacyFlags>();

		private const string PrefsKey = "VcPrivacyFlags_1.1";

		private bool _forceOpen;

		private static global::VoiceChat.VcPrivacyFlags _loadedFlags;

		private static bool _flagsLoaded;

		public static global::VoiceChat.VoiceChatPrivacySettings Singleton { get; private set; }

		public override bool IsEnabled
		{
			get
			{
				return base.IsEnabled;
			}
			set
			{
				base.IsEnabled = value || _forceOpen;
			}
		}

		public override bool CanToggle => false;

		private static global::VoiceChat.VcPrivacyFlags PrefsFlags
		{
			get
			{
				return (global::VoiceChat.VcPrivacyFlags)global::UnityEngine.PlayerPrefs.GetInt("VcPrivacyFlags_1.1", 0);
			}
			set
			{
				global::UnityEngine.PlayerPrefs.SetInt("VcPrivacyFlags_1.1", (int)value);
			}
		}

		public static global::VoiceChat.VcPrivacyFlags PrivacyFlags
		{
			get
			{
				if (_flagsLoaded)
				{
					return _loadedFlags;
				}
				_loadedFlags = PrefsFlags;
				_flagsLoaded = true;
				return _loadedFlags;
			}
			set
			{
				if (_loadedFlags != value)
				{
					_loadedFlags = value;
					PrefsFlags = value;
				}
			}
		}

		public static event global::System.Action<ReferenceHub> OnUserFlagsChanged;

		protected override void Awake()
		{
			base.Awake();
			global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup[] groups = _groups;
			foreach (global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup toggleGroup in groups)
			{
				_groupsByFlags.Add(toggleGroup.Flags, toggleGroup);
			}
			Singleton = this;
			UpdateToggles();
		}

		protected override void OnToggled()
		{
		}

		public void UpdateToggles()
		{
			global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup[] groups = _groups;
			foreach (global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup toggleGroup in groups)
			{
				toggleGroup.IsAccepted = (PrivacyFlags & toggleGroup.Flags) == toggleGroup.Flags;
			}
			_recordDim.enabled = (PrivacyFlags & global::VoiceChat.VcPrivacyFlags.AllowRecording) == 0;
		}

		public void HandleToggle(global::UnityEngine.UI.Toggle checkbox)
		{
			global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup[] groups = _groups;
			foreach (global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup toggleGroup in groups)
			{
				if (toggleGroup.AcceptToggle == checkbox)
				{
					toggleGroup.IsAccepted = true;
					break;
				}
				if (toggleGroup.DenyToggle == checkbox)
				{
					toggleGroup.IsAccepted = false;
					break;
				}
			}
			if (_groupsByFlags[global::VoiceChat.VcPrivacyFlags.AllowMicCapture].IsAccepted)
			{
				_recordDim.enabled = false;
			}
			else
			{
				_recordDim.enabled = true;
				_groupsByFlags[global::VoiceChat.VcPrivacyFlags.AllowRecording].IsAccepted = false;
			}
			global::VoiceChat.VcPrivacyFlags vcPrivacyFlags = PrivacyFlags & global::VoiceChat.VcPrivacyFlags.SettingsSelected;
			groups = _groups;
			foreach (global::VoiceChat.VoiceChatPrivacySettings.ToggleGroup toggleGroup2 in groups)
			{
				if (toggleGroup2.IsAccepted)
				{
					vcPrivacyFlags |= toggleGroup2.Flags;
				}
			}
			PrivacyFlags = vcPrivacyFlags;
			if (global::Mirror.NetworkClient.ready && ReferenceHub.TryGetLocalHub(out var hub))
			{
				global::Mirror.NetworkClient.Send(new global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage
				{
					Flags = (byte)PrivacyFlags
				});
				global::VoiceChat.VoiceChatPrivacySettings.OnUserFlagsChanged?.Invoke(hub);
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.Inventory.OnServerStarted += delegate
			{
				global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage msg)
				{
					if (ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub))
					{
						FlagsOfPlayers[hub] = (global::VoiceChat.VcPrivacyFlags)msg.Flags;
						global::VoiceChat.VoiceChatPrivacySettings.OnUserFlagsChanged?.Invoke(hub);
					}
				});
			};
			ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, (global::System.Action<ReferenceHub>)delegate(ReferenceHub hub)
			{
				if (hub.isLocalPlayer && !global::Mirror.NetworkServer.active)
				{
					global::Mirror.NetworkClient.Send(new global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage
					{
						Flags = (byte)PrivacyFlags
					});
				}
			});
			ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, (global::System.Action<ReferenceHub>)delegate(ReferenceHub hub)
			{
				FlagsOfPlayers.Remove(hub);
			});
		}

		public static bool CheckUserFlags(ReferenceHub user, global::VoiceChat.VcPrivacyFlags flags)
		{
			if (FlagsOfPlayers.TryGetValue(user, out var value))
			{
				return (value & flags) == flags;
			}
			return false;
		}
	}
}
