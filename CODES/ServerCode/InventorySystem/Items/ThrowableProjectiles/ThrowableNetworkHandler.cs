namespace InventorySystem.Items.ThrowableProjectiles
{
	public static class ThrowableNetworkHandler
	{
		public readonly struct ThrowableItemRequestMessage : global::Mirror.NetworkMessage
		{
			public readonly ushort Serial;

			public readonly global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType Request;

			public readonly global::UnityEngine.Quaternion CameraRotation;

			public readonly global::RelativePositioning.RelativePosition CameraPosition;

			public readonly global::UnityEngine.Vector3 PlayerVelocity;

			public ThrowableItemRequestMessage(ushort serial, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType type, global::UnityEngine.Quaternion rotation, global::RelativePositioning.RelativePosition position, global::UnityEngine.Vector3 startVel)
			{
				Serial = serial;
				Request = type;
				CameraRotation = rotation;
				CameraPosition = position;
				PlayerVelocity = startVel;
			}

			public ThrowableItemRequestMessage(global::InventorySystem.Items.ThrowableProjectiles.ThrowableItem item, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType type, global::UnityEngine.Vector3 startVel = default(global::UnityEngine.Vector3))
			{
				Serial = item.ItemSerial;
				Request = type;
				CameraRotation = item.Owner.PlayerCameraReference.rotation;
				CameraPosition = new global::RelativePositioning.RelativePosition(item.Owner.PlayerCameraReference.position);
				PlayerVelocity = startVel;
			}
		}

		public readonly struct ThrowableItemAudioMessage : global::Mirror.NetworkMessage
		{
			public readonly ushort Serial;

			public readonly global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType Request;

			public readonly float Time;

			public ThrowableItemAudioMessage(ushort itemSerial, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType rt)
			{
				Serial = itemSerial;
				Request = rt;
				Time = global::UnityEngine.Time.timeSinceLevelLoad;
			}
		}

		public enum RequestType : byte
		{
			BeginThrow = 0,
			ConfirmThrowWeak = 1,
			ConfirmThrowFullForce = 2,
			CancelThrow = 3
		}

		public static readonly global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage> ReceivedRequests = new global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage>();

		private const float MaxPlayerSpeed = 10f;

		public static event global::System.Action<global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage> OnAudioMessageReceived;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientStarted += RegisterProjectiles;
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemRequestMessage>(ServerProcessRequest);
				global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage>(ClientProcessAudio);
			};
		}

		private static void RegisterProjectiles()
		{
			foreach (global::System.Collections.Generic.KeyValuePair<ItemType, global::InventorySystem.Items.ItemBase> availableItem in global::InventorySystem.InventoryItemLoader.AvailableItems)
			{
				if (availableItem.Value is global::InventorySystem.Items.ThrowableProjectiles.ThrowableItem throwableItem && !(throwableItem.Projectile == null))
				{
					global::System.Guid assetId = throwableItem.Projectile.netIdentity.assetId;
					global::Mirror.NetworkClient.prefabs[assetId] = throwableItem.Projectile.gameObject;
				}
			}
		}

		private static void ServerProcessRequest(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemRequestMessage msg)
		{
			if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub) && hub.inventory.CurItem.SerialNumber == msg.Serial && hub.inventory.CurInstance is global::InventorySystem.Items.ThrowableProjectiles.ThrowableItem throwableItem)
			{
				switch (msg.Request)
				{
				case global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.BeginThrow:
					throwableItem.ServerProcessInitiation();
					break;
				case global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.CancelThrow:
					throwableItem.ServerProcessCancellation();
					break;
				case global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.ConfirmThrowFullForce:
					throwableItem.ServerProcessThrowConfirmation(fullForce: true, msg.CameraPosition.Position, msg.CameraRotation, msg.PlayerVelocity);
					break;
				case global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.ConfirmThrowWeak:
					throwableItem.ServerProcessThrowConfirmation(fullForce: false, msg.CameraPosition.Position, msg.CameraRotation, msg.PlayerVelocity);
					break;
				}
			}
		}

		private static void ClientProcessAudio(global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage msg)
		{
		}

		public static global::UnityEngine.Vector3 GetLimitedVelocity(global::UnityEngine.Vector3 plyVel)
		{
			float magnitude = plyVel.magnitude;
			if (magnitude > 10f)
			{
				plyVel /= magnitude;
				plyVel *= 10f;
			}
			return plyVel;
		}

		private static bool RequiresAdditionalData(global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType rq)
		{
			if (rq != global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.ConfirmThrowFullForce)
			{
				return rq == global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.ConfirmThrowWeak;
			}
			return true;
		}

		public static void SerializeRequestMsg(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemRequestMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, value.Serial);
			writer.WriteByte((byte)value.Request);
			if (RequiresAdditionalData(value.Request))
			{
				writer.WriteLowPrecisionQuaternion(new LowPrecisionQuaternion(value.CameraRotation));
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, value.CameraPosition);
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, value.PlayerVelocity);
			}
		}

		public static global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemRequestMessage DeserializeRequestMsg(this global::Mirror.NetworkReader reader)
		{
			ushort serial = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType requestType = (global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType)reader.ReadByte();
			bool num = RequiresAdditionalData(requestType);
			global::UnityEngine.Quaternion rotation = (num ? reader.ReadLowPrecisionQuaternion().Value : default(global::UnityEngine.Quaternion));
			global::RelativePositioning.RelativePosition position = (num ? global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader) : default(global::RelativePositioning.RelativePosition));
			global::UnityEngine.Vector3 startVel = (num ? global::Mirror.NetworkReaderExtensions.ReadVector3(reader) : default(global::UnityEngine.Vector3));
			return new global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemRequestMessage(serial, requestType, rotation, position, startVel);
		}

		public static void SerializeAudioMsg(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, value.Serial);
			writer.WriteByte((byte)value.Request);
		}

		public static global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage DeserializeAudioMsg(this global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage(global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), (global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType)reader.ReadByte());
		}
	}
}
