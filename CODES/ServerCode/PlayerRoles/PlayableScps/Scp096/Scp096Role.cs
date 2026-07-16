namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096Role : global::PlayerRoles.PlayableScps.FpcStandardScp, global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole, global::PlayerRoles.PlayableScps.HUDs.IHudScp, global::PlayerRoles.PlayableScps.ISpawnableScp
	{
		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Scp096.Scp096StateController StateController { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule SubroutineModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HUDs.ScpHudBase HudPrefab { get; private set; }

		public float GetSpawnChance(global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> alreadySpawned)
		{
			return (alreadySpawned.Count != 0 && !alreadySpawned.Contains(global::PlayerRoles.RoleTypeId.Scp079)) ? 1 : 0;
		}
	}
}
