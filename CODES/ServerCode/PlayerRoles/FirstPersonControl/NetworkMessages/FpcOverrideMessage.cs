namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public struct FpcOverrideMessage : global::Mirror.NetworkMessage
	{
		public readonly global::UnityEngine.Vector3 Position;

		public readonly float DeltaRotation;

		private const float FullAngle = 360f;

		public FpcOverrideMessage(global::UnityEngine.Vector3 pos, float rot)
		{
			Position = pos;
			DeltaRotation = rot;
		}

		public FpcOverrideMessage(global::Mirror.NetworkReader reader)
		{
			Position = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position;
			DeltaRotation = global::UnityEngine.Mathf.Lerp(-360f, 360f, (float)(int)global::Mirror.NetworkReaderExtensions.ReadUInt16(reader) / 65535f);
		}

		public void Write(global::Mirror.NetworkWriter writer)
		{
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(Position));
			float num;
			for (num = DeltaRotation; num < -360f; num += 360f)
			{
			}
			while (num > 360f)
			{
				num -= 360f;
			}
			float num2 = global::UnityEngine.Mathf.InverseLerp(-360f, 360f, num);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, (ushort)global::UnityEngine.Mathf.RoundToInt(num2 * 65535f));
		}

		public void ProcessMessage()
		{
			if (ReferenceHub.TryGetLocalHub(out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole && fpcRole.FpcModule.ModuleReady)
			{
				fpcRole.FpcModule.Position = Position;
				fpcRole.FpcModule.MouseLook.CurrentHorizontal += DeltaRotation;
			}
		}
	}
}
