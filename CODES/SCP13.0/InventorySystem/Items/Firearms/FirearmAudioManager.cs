using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public static class FirearmAudioManager
	{
		[Serializable]
		private class AudibleShooterInfo
		{
			private readonly ReferenceHub _hub;

			private readonly Queue<FirearmAudioClip> _remainingClips;

			private readonly Stopwatch _lastShotStopwatch;

			private bool _isEmpty;

			private const float MinimalCooldown = 0.05f;

			private const int MaxQueueCount = 7;

			public AudibleShooterInfo(ReferenceHub hub)
			{
			}

			public void Enqueue(AudioClip clip, float distance, bool useDedicatedChannel)
			{
			}

			public void Play()
			{
			}
		}

		private static readonly Dictionary<uint, AudibleShooterInfo> Shooters;

		public static event Action<ReferenceHub, ItemType, FirearmAudioClip> OnAudioReceived
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

		public static event Action<GunAudioMessage> OnMessageReceived
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

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterHandler()
		{
		}

		private static void ClientAudioReceived(GunAudioMessage msg)
		{
		}

		private static void LateUpdate()
		{
		}

		public static void Serialize(this NetworkWriter writer, GunAudioMessage value)
		{
		}

		public static GunAudioMessage Deserialize(this NetworkReader reader)
		{
			return default(GunAudioMessage);
		}
	}
}
