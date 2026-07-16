namespace PlayerRoles
{
	public class NoneRole : global::PlayerRoles.PlayerRoleBase, global::PlayerRoles.Voice.IVoiceRole
	{
		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.Voice.VoiceModuleBase VoiceModule { get; private set; }

		public override global::PlayerRoles.RoleTypeId RoleTypeId => global::PlayerRoles.RoleTypeId.None;

		public override global::UnityEngine.Color RoleColor => global::UnityEngine.Color.white;

		public override global::PlayerRoles.Team Team => global::PlayerRoles.Team.Dead;
	}
}
