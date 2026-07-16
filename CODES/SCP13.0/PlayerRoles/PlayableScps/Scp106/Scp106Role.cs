using System.Collections.Generic;
using GameObjectPools;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Role : FpcStandardScp, ISubroutinedRole, IHudScp, IPoolResettable, IHumeShieldedRole, IDamageHandlerProcessingRole, ITeslaControllerRole, ISpawnableScp
	{
		public static readonly HashSet<Scp106Role> AllInstances;

		public const float EmergeTime = 1.9f;

		public const float SubmergeTime = 3.1f;

		private Scp106SinkholeController _sinkholeCtrl;

		private bool _sinkholeSet;

		[field: SerializeField]
		public HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: SerializeField]
		public SubroutineManagerModule SubroutineModule { get; private set; }

		[field: SerializeField]
		public ScpHudBase HudPrefab { get; private set; }

		[field: SerializeField]
		public AudioClip ItemSpawnSound { get; private set; }

		public Scp106SinkholeController Sinkhole => null;

		public bool CanActivateShock => false;

		public bool IsSubmerged => false;

		public override void SpawnObject()
		{
		}

		public void ResetObject()
		{
		}

		public DamageHandlerBase ProcessDamageHandler(DamageHandlerBase dhb)
		{
			return null;
		}

		public float GetSpawnChance(List<RoleTypeId> alreadySpawned)
		{
			return 0f;
		}

		public bool IsInIdleRange(TeslaGate teslaGate)
		{
			return false;
		}

		private bool ValidateDamageHandler(DamageHandlerBase dhb)
		{
			return false;
		}
	}
}
