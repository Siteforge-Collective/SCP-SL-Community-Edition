public class HitboxIdentity : global::UnityEngine.MonoBehaviour, IDestructible
{
	public static readonly global::System.Collections.Generic.HashSet<HitboxIdentity> Instances = new global::System.Collections.Generic.HashSet<HitboxIdentity>();

	[global::UnityEngine.SerializeField]
	private HitboxType _dmgMultiplier;

	public ReferenceHub TargetHub => CharacterModel.OwnerHub;

	public HitboxType HitboxType => _dmgMultiplier;

	public global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel CharacterModel { get; private set; }

	public global::UnityEngine.Collider[] TargetColliders { get; private set; }

	public uint NetworkId => TargetHub.inventory.netId;

	public global::UnityEngine.Vector3 CenterOfMass => base.transform.position;

	public bool Damage(float damage, global::PlayerStatsSystem.DamageHandlerBase handler, global::UnityEngine.Vector3 exactPos)
	{
		if (TargetHub == null)
		{
			return false;
		}
		if (handler is global::PlayerStatsSystem.StandardDamageHandler standardDamageHandler)
		{
			standardDamageHandler.Hitbox = _dmgMultiplier;
		}
		return TargetHub.playerStats.DealDamage(handler);
	}

	public void SetColliders(bool newState)
	{
		global::UnityEngine.Collider[] targetColliders = TargetColliders;
		for (int i = 0; i < targetColliders.Length; i++)
		{
			targetColliders[i].enabled = newState;
		}
	}

	private void Awake()
	{
		CharacterModel = GetComponentInParent<global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel>();
		TargetColliders = GetComponents<global::UnityEngine.Collider>();
	}

	private void OnDestroy()
	{
		Instances.Remove(this);
	}

	public static bool CheckFriendlyFire(ReferenceHub attacker, ReferenceHub victim, bool ignoreConfig = false)
	{
		return CheckFriendlyFire(global::PlayerRoles.PlayerRolesUtils.GetTeam(attacker), global::PlayerRoles.PlayerRolesUtils.GetTeam(victim), ignoreConfig);
	}

	public static bool CheckFriendlyFire(global::PlayerRoles.RoleTypeId attacker, global::PlayerRoles.RoleTypeId victim, bool ignoreConfig = false)
	{
		global::PlayerRoles.Team team = global::PlayerRoles.PlayerRolesUtils.GetTeam(attacker);
		global::PlayerRoles.Team team2 = global::PlayerRoles.PlayerRolesUtils.GetTeam(victim);
		return CheckFriendlyFire(team, team2, ignoreConfig);
	}

	public static bool CheckFriendlyFire(global::PlayerRoles.Team attackerTeam, global::PlayerRoles.Team victimTeam, bool ignoreConfig = false)
	{
		if (attackerTeam == global::PlayerRoles.Team.Dead || victimTeam == global::PlayerRoles.Team.Dead)
		{
			return false;
		}
		if (attackerTeam == global::PlayerRoles.Team.SCPs && victimTeam == global::PlayerRoles.Team.SCPs)
		{
			return false;
		}
		if (ignoreConfig || (ServerConfigSynchronizer.Singleton.MainBoolsSync & 1) != 1)
		{
			return global::PlayerRoles.PlayerRolesUtils.GetFaction(attackerTeam) != global::PlayerRoles.PlayerRolesUtils.GetFaction(victimTeam);
		}
		return true;
	}
}
