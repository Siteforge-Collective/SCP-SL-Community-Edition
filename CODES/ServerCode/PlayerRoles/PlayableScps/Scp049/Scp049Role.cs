namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049Role : global::PlayerRoles.PlayableScps.FpcStandardScp, global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole, global::PlayerRoles.IArmoredRole, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole, global::PlayerRoles.PlayableScps.HUDs.IHudScp, global::PlayerRoles.PlayableScps.ISpawnableScp
	{
		[global::UnityEngine.SerializeField]
		private int _armorEfficacy;

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule SubroutineModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HUDs.ScpHudBase HudPrefab { get; private set; }

		public int GetArmorEfficacy(HitboxType hitbox)
		{
			if (!(HumeShieldModule.HsCurrent > 0f))
			{
				return _armorEfficacy;
			}
			return 0;
		}

		public float GetSpawnChance(global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> alreadySpawned)
		{
			return 1f;
		}
	}
}
