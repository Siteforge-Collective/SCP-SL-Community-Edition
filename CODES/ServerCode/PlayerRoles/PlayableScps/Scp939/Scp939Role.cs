namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939Role : global::PlayerRoles.PlayableScps.FpcStandardScp, global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole, global::PlayerRoles.PlayableScps.HUDs.IHudScp, global::PlayerRoles.PlayableScps.ISpawnableScp
	{
		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule SubroutineModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HUDs.ScpHudBase HudPrefab { get; private set; }

		public float GetSpawnChance(global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> alreadySpawned)
		{
			return 1f;
		}
	}
}
