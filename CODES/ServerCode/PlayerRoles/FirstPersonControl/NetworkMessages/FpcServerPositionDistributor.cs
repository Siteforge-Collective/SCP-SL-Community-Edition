namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public static class FpcServerPositionDistributor
	{
		private const int MinTickrate = 10;

		private const int MaxTickrate = 60;

		private const int ArrayStartSize = 30;

		private const int ArrayAddAmount = 10;

		private const int ArrayAddThreshold = 5;

		private static readonly global::System.Collections.Generic.Dictionary<uint, global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData>> PreviouslySent = new global::System.Collections.Generic.Dictionary<uint, global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData>>();

		private static int[] _bufferPlayerIDs = new int[30];

		private static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData[] _bufferSyncData = new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData[30];

		private static float _sendCooldown;

		private static float SendRate => 1f / (float)global::UnityEngine.Mathf.Clamp(ServerStatic.ServerTickrate, 10, 60);

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			StaticUnityMethods.OnLateUpdate += LateUpdate;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += ResetPlayer;
			global::InventorySystem.Inventory.OnServerStarted += PreviouslySent.Clear;
			ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, (global::System.Action<ReferenceHub>)delegate
			{
				EnsureArrayCapacity();
			});
		}

		private static void ResetPlayer(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (!(prevRole is global::PlayerRoles.FirstPersonControl.IFpcRole))
			{
				return;
			}
			uint netId = userHub.netId;
			foreach (global::System.Collections.Generic.KeyValuePair<uint, global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData>> item in PreviouslySent)
			{
				item.Value.Remove(netId);
			}
		}

		private static void LateUpdate()
		{
			if (!global::Mirror.NetworkServer.active || !StaticUnityMethods.IsPlaying)
			{
				return;
			}
			_sendCooldown += global::UnityEngine.Time.deltaTime;
			if (_sendCooldown < SendRate)
			{
				return;
			}
			_sendCooldown -= SendRate;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.Mode != ClientInstanceMode.Unverified && !allHub.isLocalPlayer)
				{
					allHub.connectionToClient.Send(new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcPositionMessage(allHub));
				}
			}
		}

		private static void EnsureArrayCapacity()
		{
			int num = global::UnityEngine.Mathf.Min(_bufferPlayerIDs.Length, _bufferSyncData.Length);
			int count = ReferenceHub.AllHubs.Count;
			if (count > num - 5)
			{
				_bufferPlayerIDs = new int[count + 10];
				_bufferSyncData = new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData[count + 10];
			}
		}

		public static void WriteAll(ReferenceHub receiver, global::Mirror.NetworkWriter writer)
		{
			ushort num = 0;
			bool flag;
			global::PlayerRoles.Visibility.VisibilityController visibilityController;
			if (receiver.roleManager.CurrentRole is global::PlayerRoles.Visibility.ICustomVisibilityRole customVisibilityRole)
			{
				flag = true;
				visibilityController = customVisibilityRole.VisibilityController;
			}
			else
			{
				flag = false;
				visibilityController = null;
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.netId != receiver.netId && allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
				{
					bool flag2 = flag && !visibilityController.ValidateVisibility(allHub);
					global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData newSyncData = GetNewSyncData(receiver, allHub, fpcRole.FpcModule, flag2);
					if (!flag2)
					{
						_bufferPlayerIDs[num] = allHub.PlayerId;
						_bufferSyncData[num] = newSyncData;
						num++;
					}
				}
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, num);
			for (int i = 0; i < num; i++)
			{
				writer.WriteRecyclablePlayerId(new RecyclablePlayerId(_bufferPlayerIDs[i]));
				_bufferSyncData[i].Write(writer);
			}
		}

		private static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData GetNewSyncData(ReferenceHub receiver, ReferenceHub target, global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpmm, bool isInvisible)
		{
			global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData prevSyncData = GetPrevSyncData(receiver, target);
			global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData fpcSyncData = (isInvisible ? default(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData) : new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData(prevSyncData, fpmm.SyncMovementState, fpmm.IsGrounded, new global::RelativePositioning.RelativePosition(target.transform.position), fpmm.MouseLook));
			PreviouslySent[receiver.netId][target.netId] = fpcSyncData;
			return fpcSyncData;
		}

		private static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData GetPrevSyncData(ReferenceHub receiver, ReferenceHub target)
		{
			if (!PreviouslySent.TryGetValue(receiver.netId, out var value))
			{
				PreviouslySent.Add(receiver.netId, new global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData>());
				return default(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData);
			}
			if (!value.TryGetValue(target.netId, out var value2))
			{
				return default(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData);
			}
			return value2;
		}
	}
}
