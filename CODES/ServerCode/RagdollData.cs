[global::System.Serializable]
public struct RagdollData : global::Mirror.NetworkMessage, global::System.IEquatable<RagdollData>
{
	public readonly global::PlayerRoles.RoleTypeId RoleType;

	public readonly string Nickname;

	public readonly global::PlayerStatsSystem.DamageHandlerBase Handler;

	public readonly global::UnityEngine.Vector3 StartPosition;

	public readonly global::UnityEngine.Quaternion StartRotation;

	public readonly double CreationTime;

	public readonly ReferenceHub OwnerHub;

	public float ExistenceTime => (float)(global::Mirror.NetworkTime.time - CreationTime);

	public RagdollData(ReferenceHub hub, global::PlayerStatsSystem.DamageHandlerBase handler, global::UnityEngine.Vector3 positionOffset, global::UnityEngine.Quaternion rotationOffset)
	{
		OwnerHub = hub;
		RoleType = global::PlayerRoles.PlayerRolesUtils.GetRoleId(hub);
		global::UnityEngine.Transform transform = hub.transform;
		StartPosition = transform.position + positionOffset;
		StartRotation = transform.rotation * rotationOffset;
		Nickname = hub.nicknameSync.DisplayName;
		Handler = handler;
		CreationTime = global::Mirror.NetworkTime.time;
	}

	public RagdollData(ReferenceHub hub, global::PlayerStatsSystem.DamageHandlerBase handler, global::PlayerRoles.RoleTypeId roleType, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot, string nick, double creationTime)
	{
		OwnerHub = hub;
		RoleType = roleType;
		StartPosition = pos;
		StartRotation = rot;
		Nickname = nick;
		Handler = handler;
		CreationTime = creationTime;
	}

	public bool Equals(RagdollData other)
	{
		if (RoleType == other.RoleType && string.Equals(Nickname, other.Nickname))
		{
			return Handler == other.Handler;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is RagdollData other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((((((byte)RoleType * 397) ^ Nickname.GetHashCode()) * 397) ^ Handler.GetHashCode()) * 397) ^ CreationTime.GetHashCode();
	}

	public static bool operator ==(RagdollData left, RagdollData right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(RagdollData left, RagdollData right)
	{
		return !left.Equals(right);
	}
}
