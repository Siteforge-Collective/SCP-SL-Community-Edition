using Mirror;
using TMPro;
using UnityEngine;

namespace VoiceChat
{
	public class VoiceChatMuteIndicator : MonoBehaviour
	{
		public struct SyncMuteMessage : NetworkMessage
		{
			public byte Flags;
		}

		[SerializeField]
		private GameObject _root;

		[SerializeField]
		private GameObject _locally;

		[SerializeField]
		private GameObject _globally;

		[SerializeField]
		private GameObject _unauthed;

		[SerializeField]
		private GameObject _privacy;

		[SerializeField]
		private TextMeshProUGUI _privacyText;

		private static VoiceChatMuteIndicator _singleton;

		private const VcMuteFlags Filter = VcMuteFlags.LocalRegular | VcMuteFlags.GlobalRegular;

		public static VcMuteFlags ReceivedFlags { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += delegate
            {
                NetworkClient.ReplaceHandler<SyncMuteMessage>(ReceiveMessage);
            };
            VoiceChatMutes.OnFlagsSet += delegate (ReferenceHub hub, VcMuteFlags flags)
            {
                if (NetworkServer.active)
                {
                    hub.connectionToClient.Send(new SyncMuteMessage
                    {
                        Flags = (byte)flags
                    });
                }
            };
        }

        private static void ReceiveMessage(SyncMuteMessage smm)
		{
		}

		private void Start()
		{
		}

		private static void RefreshIndicator(ReferenceHub hub)
		{
		}

		private void RefreshIndicator()
		{
		}
	}
}
