public static class RagdollDataReaderWriter
{
	public static void WriteRagdollData(this global::Mirror.NetworkWriter writer, RagdollData info)
	{
		writer.WriteByte((byte)info.RoleType);
		global::Mirror.NetworkWriterExtensions.WriteString(writer, info.Nickname);
		global::PlayerStatsSystem.DamageHandlerReaderWriter.WriteDamageHandler(writer, info.Handler);
		global::Mirror.NetworkWriterExtensions.WriteVector3(writer, info.StartPosition);
		writer.WriteLowPrecisionQuaternion(new LowPrecisionQuaternion(info.StartRotation));
		global::Mirror.NetworkWriterExtensions.WriteDouble(writer, info.CreationTime);
		global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, info.OwnerHub);
	}

	public static RagdollData ReadRagdollData(this global::Mirror.NetworkReader reader)
	{
		global::PlayerRoles.RoleTypeId roleType = (global::PlayerRoles.RoleTypeId)reader.ReadByte();
		string nick = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		global::PlayerStatsSystem.DamageHandlerBase handler = global::PlayerStatsSystem.DamageHandlerReaderWriter.ReadDamageHandler(reader);
		global::UnityEngine.Vector3 pos = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
		global::UnityEngine.Quaternion value = reader.ReadLowPrecisionQuaternion().Value;
		double creationTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		return new RagdollData(global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader), handler, roleType, pos, value, nick, creationTime);
	}
}
