namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public struct FpcFallDamageMessage : global::Mirror.NetworkMessage
	{
		private const float SoundDistance = 14f;

		private readonly ReferenceHub _hub;

		private readonly global::UnityEngine.Vector3 _prevPos;

		private readonly global::PlayerRoles.RoleTypeId _role;

		public FpcFallDamageMessage(ReferenceHub hub, global::UnityEngine.Vector3 prevPos, global::PlayerRoles.RoleTypeId role)
		{
			_hub = hub;
			_prevPos = prevPos;
			_role = role;
		}

		public FpcFallDamageMessage(global::Mirror.NetworkReader reader)
		{
			int value = reader.ReadRecyclablePlayerId().Value;
			if (value == 0)
			{
				_hub = null;
				_prevPos = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position;
				_role = reader.ReadRoleType();
			}
			else
			{
				_hub = ReferenceHub.GetHub(value);
				_prevPos = global::UnityEngine.Vector3.zero;
				_role = ((_hub != null) ? _hub.GetRoleId() : global::PlayerRoles.RoleTypeId.None);
			}
		}

		public void Write(global::Mirror.NetworkWriter writer)
		{
			if (_hub == null || !_hub.IsAlive())
			{
				global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, null);
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(_prevPos));
				writer.WriteRoleType(_role);
			}
			else
			{
				global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, _hub);
			}
		}

		public void ProcessMessage()
		{
		}
	}
}
