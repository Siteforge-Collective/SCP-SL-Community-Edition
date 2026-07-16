using System.Collections.Generic;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173Role : FpcStandardScp, ISubroutinedRole, IArmoredRole, IHumeShieldedRole, IHudScp, ISpawnableScp
	{
		[SerializeField]
		private int _armorEfficacy;

		private ReferenceHub _owner;

		private Scp173AudioPlayer _audio;

		private bool _damagedEventAssigned;

		private bool DamagedEventActive
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public ScpDamageHandler DamageHandler => null;

		[field: SerializeField]
		public HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: SerializeField]
		public SubroutineManagerModule SubroutineModule { get; private set; }

		[field: SerializeField]
		public ScpHudBase HudPrefab { get; private set; }

		private void OnDamaged(DamageHandlerBase obj)
		{
		}

		private void Awake()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void DisableRole(RoleTypeId newRole)
		{
		}

		public int GetArmorEfficacy(HitboxType hitbox)
		{
			return 0;
		}

		public float GetSpawnChance(List<RoleTypeId> alreadySpawned)
		{
			return 0f;
		}
	}
}
