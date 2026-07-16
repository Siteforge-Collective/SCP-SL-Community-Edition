namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public struct FpcFromClientMessage : global::Mirror.NetworkMessage
	{
		private readonly global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData _data;

		public FpcFromClientMessage(global::RelativePositioning.RelativePosition pos, global::PlayerRoles.FirstPersonControl.PlayerMovementState state, bool jump, global::PlayerRoles.FirstPersonControl.FpcMouseLook mouseLook)
		{
			_data = new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData(default(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData), state, jump, pos, mouseLook);
		}

		public FpcFromClientMessage(global::Mirror.NetworkReader reader)
		{
			_data = new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData(reader);
		}

		public void Write(global::Mirror.NetworkWriter writer)
		{
			_data.Write(writer);
		}

		public void ProcessMessage(global::Mirror.NetworkConnection sender)
		{
			if (!sender.identity.isLocalPlayer && ReferenceHub.TryGetHubNetID(sender.identity.netId, out var hub) && _data.TryApply(hub, out var module, out var bit) && bit)
			{
				module.Motor.WantsToJump = true;
			}
		}
	}
}
