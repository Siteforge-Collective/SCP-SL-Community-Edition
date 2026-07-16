using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using VoiceChat;

namespace PlayerRoles.Spectating
{
    public class OverwatchVoiceChannelSelector : MonoBehaviour
    {
        private struct ChannelMuteFlagsMessage : NetworkMessage
        {
            public bool SpatialAudio;
            public uint EnabledChannels;
        }

        [SerializeField]
        private OverwatchVoiceChatButton[] _buttons;

        [SerializeField]
        private OverwatchVoiceChatButton _spatialButton;

        public static readonly CachedValue<VoiceChatChannel[]> AllChannels = new CachedValue<VoiceChatChannel[]>(() => Enum.GetValues(typeof(VoiceChatChannel)) as VoiceChatChannel[]);

        private static readonly HashSet<VoiceChatChannel> ActiveMutes = new HashSet<VoiceChatChannel>();

        public void OnToggled()
        {
            ActiveMutes.Clear();

            foreach (var button in _buttons)
            {
                if (!button.IsToggled)
                {
                    ActiveMutes.Add(button.VoiceChatChannel);
                }
            }

            ClientSendMessage(SerializeChannels(), _spatialButton.IsToggled);
        }

        private void OnEnable()
        {
            ActiveMutes.Clear();
            ActiveMutes.Add((VoiceChatChannel)3);

            foreach (var button in _buttons)
            {
                button.IsToggled = !ActiveMutes.Contains(button.VoiceChatChannel);
            }

            _spatialButton.IsToggled = true;
            ClientSendMessage(SerializeChannels(), _spatialButton.IsToggled);
        }

        private static uint SerializeChannels()
        {
            uint mask = 0u;
            foreach (VoiceChatChannel channel in ActiveMutes)
            {
                mask |= (1u << (int)channel);
            }
            return mask;
        }

        private static void DeserializeChannels(uint channels)
        {
            ActiveMutes.Clear();
            foreach (VoiceChatChannel channel in AllChannels.Value)
            {
                if ((channels & (1u << (int)channel)) != 0)
                {
                    ActiveMutes.Add(channel);
                }
            }
        }

        private static void ClientSendMessage(uint channels, bool spatialAudio)
        {
            NetworkClient.Send(new ChannelMuteFlagsMessage
            {
                EnabledChannels = channels,
                SpatialAudio = spatialAudio
            });
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += () =>
            {
                NetworkServer.ReplaceHandler<ChannelMuteFlagsMessage>(ProcessMessage);
            };
        }

        private static void ProcessMessage(NetworkConnection conn, ChannelMuteFlagsMessage msg)
        {
            if (conn.identity != null &&
                ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) &&
                hub.roleManager.CurrentRole is OverwatchRole overwatchRole)
            {
                var voiceModule = overwatchRole.VoiceModule as OverwatchVoiceModule;
                if (voiceModule != null)
                {
                    DeserializeChannels(msg.EnabledChannels);

                    voiceModule.DisabledChannels = ActiveMutes.ToArray();
                    voiceModule.UseSpatialAudio = msg.SpatialAudio;
                }
            }
        }
    }
}