namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public struct FpcPositionMessage : global::Mirror.NetworkMessage
	{
		private readonly ReferenceHub _receiver;

		private static readonly global::System.Collections.Generic.HashSet<uint> AssignedNetIds = new global::System.Collections.Generic.HashSet<uint>();

		public FpcPositionMessage(ReferenceHub receiver)
		{
			_receiver = receiver;
		}

		public FpcPositionMessage(global::Mirror.NetworkReader reader)
		{
			_receiver = null;
			ushort num = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			AssignedNetIds.Clear();
			for (int i = 0; i < num; i++)
			{
				int value = reader.ReadRecyclablePlayerId().Value;
				global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData fpcSyncData = new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData(reader);
				if (value != 0 && ReferenceHub.TryGetHub(value, out var hub))
				{
					AssignedNetIds.Add(hub.netId);
					if (fpcSyncData.TryApply(hub, out var module, out var bit))
					{
						module.IsGrounded = bit;
					}
				}
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
				{
					fpcRole.FpcModule.Motor.IsInvisible = !AssignedNetIds.Contains(allHub.netId);
				}
			}
		}

		public void Write(global::Mirror.NetworkWriter writer)
		{
			global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcServerPositionDistributor.WriteAll(_receiver, writer);
		}
	}
}
