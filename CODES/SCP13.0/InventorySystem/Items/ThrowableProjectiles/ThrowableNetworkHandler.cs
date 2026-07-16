using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
using RelativePositioning;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public static class ThrowableNetworkHandler
	{
		public readonly struct ThrowableItemRequestMessage : NetworkMessage
		{
			public readonly ushort Serial;

			public readonly RequestType Request;

			public readonly Quaternion CameraRotation;

			public readonly RelativePosition CameraPosition;

			public readonly Vector3 PlayerVelocity;

			public ThrowableItemRequestMessage(ushort serial, RequestType type, Quaternion rotation, RelativePosition position, Vector3 startVel)
			{
				Serial = 0;
				Request = default(RequestType);
				CameraRotation = default(Quaternion);
				CameraPosition = default(RelativePosition);
				PlayerVelocity = default(Vector3);
			}

			public ThrowableItemRequestMessage(ThrowableItem item, RequestType type, Vector3 startVel = default(Vector3))
			{
				Serial = 0;
				Request = default(RequestType);
				CameraRotation = default(Quaternion);
				CameraPosition = default(RelativePosition);
				PlayerVelocity = default(Vector3);
			}
		}

		public readonly struct ThrowableItemAudioMessage : NetworkMessage
		{
			public readonly ushort Serial;

			public readonly RequestType Request;

			public readonly float Time;

			public ThrowableItemAudioMessage(ushort itemSerial, RequestType rt)
			{
				Serial = 0;
				Request = default(RequestType);
				Time = 0f;
			}
		}

		public enum RequestType : byte
		{
			BeginThrow = 0,
			ConfirmThrowWeak = 1,
			ConfirmThrowFullForce = 2,
			CancelThrow = 3
		}

		public static readonly Dictionary<ushort, ThrowableItemAudioMessage> ReceivedRequests;

		private const float MaxPlayerSpeed = 10f;

		public static event Action<ThrowableItemAudioMessage> OnAudioMessageReceived
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

		private static void RegisterProjectiles()
		{
		}

		private static void ServerProcessRequest(NetworkConnection conn, ThrowableItemRequestMessage msg)
		{
		}

		private static void ClientProcessAudio(ThrowableItemAudioMessage msg)
		{
		}

		public static Vector3 GetLimitedVelocity(Vector3 plyVel)
		{
			return default(Vector3);
		}

		private static bool RequiresAdditionalData(RequestType rq)
		{
			return false;
		}

		public static void SerializeRequestMsg(this NetworkWriter writer, ThrowableItemRequestMessage value)
		{
		}

		public static ThrowableItemRequestMessage DeserializeRequestMsg(this NetworkReader reader)
		{
			return default(ThrowableItemRequestMessage);
		}

		public static void SerializeAudioMsg(this NetworkWriter writer, ThrowableItemAudioMessage value)
		{
		}

		public static ThrowableItemAudioMessage DeserializeAudioMsg(this NetworkReader reader)
		{
			return default(ThrowableItemAudioMessage);
		}
	}
}
