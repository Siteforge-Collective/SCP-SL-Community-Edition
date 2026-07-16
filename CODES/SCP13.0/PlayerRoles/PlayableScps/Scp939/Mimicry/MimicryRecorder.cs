using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Footprinting;
using Mirror;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryRecorder : StandardSubroutine<Scp939Role>
	{
		public readonly struct MimicryRecording
		{
			public readonly Footprint Owner;

			public readonly PlaybackBuffer Buffer;

			public MimicryRecording(ReferenceHub owner, PlaybackBuffer buffer)
			{
				Owner = default(Footprint);
				Buffer = null;
			}
		}

		private readonly Dictionary<HumanRole, PlaybackBuffer> _received;

		private readonly HashSet<ReferenceHub> _serverSentVoices;

		private readonly HashSet<ReferenceHub> _serverSentConfirmations;

		private bool _wasLocal;

		private bool _syncMute;

		private ReferenceHub _syncPlayer;

		[SerializeField]
		private int _maxDurationSamples;

		[SerializeField]
		private int _minDurationSamples;

		[SerializeField]
		private GameObject _confirmationBox;

		private const double MarkUptime = 5.0;

		private readonly List<Footprint> _lastAttackedPlayers;

		public readonly List<MimicryRecording> SavedVoices;

		[field: SerializeField]
		public int MaxRecordings { get; private set; }

		[field: SerializeField]
		public MimicryPreviewPlayback PreviewPlayback { get; private set; }

		[field: SerializeField]
		public MimicryTransmitter Transmitter { get; private set; }

		public event Action OnSavedVoicesModified
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public event Action<Scp939HudTranslation> OnSavedVoicesItemAdded
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void OnRoleChanged(ReferenceHub ply, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void ServerRemoveClient(ReferenceHub ply)
		{
		}

		private void RemoveRecordingsOfPlayer(ReferenceHub ply)
		{
		}

		private void OnAnyPlayerWasDamaged(ReferenceHub ply, DamageHandlerBase dh)
		{
		}

		private void ClearExpiredTargets()
		{
		}

		private bool WasAttackedRecently(ReferenceHub ply)
		{
			return false;
		}

		private void OnAnyPlayerKilled(ReferenceHub ply, DamageHandlerBase dh)
		{
		}

		private bool WasKilledByTeammate(HumanRole ply, DamageHandlerBase dh)
		{
			return false;
		}

		private void OnPlayerMuteChanges(ReferenceHub ply, VcMuteFlags _)
		{
		}

		private void OnPlayerPrivacyChanges(ReferenceHub ply)
		{
		}

		private bool IsPrivacyAccepted(ReferenceHub hub)
		{
			return false;
		}

		private void UnregisterRole(HumanRole role)
		{
		}

		private void RegisterRole(HumanRole role)
		{
		}

		private void SaveRecording(ReferenceHub ply)
		{
		}

		public void RemoveVoice(PlaybackBuffer voiceRecord)
		{
		}

		public void RemoveIndex(int id)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
