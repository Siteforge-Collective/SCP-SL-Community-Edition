namespace Respawning
{
	[global::System.Serializable]
	public struct RespawnEffect
	{
		public global::Respawning.SpawnableTeamType TargetTeam;

		public bool WhitelistEnabled;

		public global::PlayerRoles.RoleTypeId[] WhitelistedRoles;

		public global::UnityEngine.Animator AnimatorEffects;

		public global::UnityEngine.AudioSource AudioAnnouncement;
	}
}
