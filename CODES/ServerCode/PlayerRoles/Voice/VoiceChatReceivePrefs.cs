namespace PlayerRoles.Voice
{
	public class VoiceChatReceivePrefs : global::UnityEngine.MonoBehaviour
	{
		[global::System.Serializable]
		private class ToggleFlagPair
		{
			public global::UnityEngine.UI.Toggle Checkbox;

			public global::PlayerRoles.Voice.GroupMuteFlags Flag;
		}

		public struct GroupMuteFlagsMessage : global::Mirror.NetworkMessage
		{
			public byte Flags;
		}

		private const string PrefsKey = "ClientMuteFlags";

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.Voice.VoiceChatReceivePrefs.ToggleFlagPair[] _checkboxes;

		private static global::PlayerRoles.Voice.GroupMuteFlags _loadedFlags;

		private static readonly global::System.Collections.Generic.Dictionary<global::Mirror.NetworkConnection, global::PlayerRoles.Voice.GroupMuteFlags> RememberedFlags = new global::System.Collections.Generic.Dictionary<global::Mirror.NetworkConnection, global::PlayerRoles.Voice.GroupMuteFlags>();

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.Voice.VoiceChatReceivePrefs> ActiveInstances = new global::System.Collections.Generic.HashSet<global::PlayerRoles.Voice.VoiceChatReceivePrefs>();

		public static event global::System.Action<global::Mirror.NetworkConnection, global::PlayerRoles.Voice.GroupMuteFlags> OnFlagsReceived;

		private void OnEnable()
		{
			ActiveInstances.Add(this);
			SetCheckboxes();
		}

		private void OnDisable()
		{
			ActiveInstances.Remove(this);
		}

		private void SetCheckboxes()
		{
			_checkboxes.ForEach(delegate(global::PlayerRoles.Voice.VoiceChatReceivePrefs.ToggleFlagPair x)
			{
				x.Checkbox.SetIsOnWithoutNotify((_loadedFlags & x.Flag) != 0);
			});
		}

		private static void ClientSendMessage()
		{
			global::Mirror.NetworkClient.Send(new global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage
			{
				Flags = (byte)_loadedFlags
			});
		}

		public void OnToggled()
		{
			global::PlayerRoles.Voice.VoiceChatReceivePrefs.ToggleFlagPair[] checkboxes = _checkboxes;
			foreach (global::PlayerRoles.Voice.VoiceChatReceivePrefs.ToggleFlagPair toggleFlagPair in checkboxes)
			{
				if (toggleFlagPair.Checkbox.isOn)
				{
					_loadedFlags |= toggleFlagPair.Flag;
				}
				else
				{
					_loadedFlags &= ~toggleFlagPair.Flag;
				}
			}
			ClientSendMessage();
			PlayerPrefsSl.Set("ClientMuteFlags", (int)_loadedFlags);
			foreach (global::PlayerRoles.Voice.VoiceChatReceivePrefs activeInstance in ActiveInstances)
			{
				if (!(activeInstance == this))
				{
					activeInstance.SetCheckboxes();
				}
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				RememberedFlags.Clear();
				global::Mirror.NetworkServer.ReplaceHandler<global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage>(ProcessMessage);
				_loadedFlags = (global::PlayerRoles.Voice.GroupMuteFlags)PlayerPrefsSl.Get("ClientMuteFlags", 0);
				ClientSendMessage();
			};
		}

		private static void ProcessMessage(global::Mirror.NetworkConnection conn, global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage msg)
		{
			global::PlayerRoles.Voice.GroupMuteFlags flags = (global::PlayerRoles.Voice.GroupMuteFlags)msg.Flags;
			global::PlayerRoles.Voice.VoiceChatReceivePrefs.OnFlagsReceived?.Invoke(conn, flags);
			RememberedFlags[conn] = flags;
			if (!(conn.identity == null) && ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.Voice.IVoiceRole voiceRole)
			{
				voiceRole.VoiceModule.ReceiveFlags = flags;
			}
		}

		public static global::PlayerRoles.Voice.GroupMuteFlags GetFlagsForUser(ReferenceHub hub)
		{
			if (!RememberedFlags.TryGetValue(hub.connectionToClient, out var value))
			{
				return global::PlayerRoles.Voice.GroupMuteFlags.None;
			}
			return value;
		}
	}
}
