namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Role : global::PlayerRoles.PlayableScps.FpcStandardScp, global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole, global::PlayerRoles.PlayableScps.HUDs.IHudScp, global::GameObjectPools.IPoolResettable, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole, global::PlayerRoles.IDamageHandlerProcessingRole, global::PlayerRoles.ITeslaControllerRole, global::PlayerRoles.PlayableScps.ISpawnableScp
	{
		public static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp106.Scp106Role> AllInstances = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp106.Scp106Role>();

		private global::PlayerRoles.PlayableScps.Scp106.Scp106SinkholeController _sinkholeCtrl;

		private bool _sinkholeSet;

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule SubroutineModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HUDs.ScpHudBase HudPrefab { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.AudioClip ItemSpawnSound { get; private set; }

		public global::PlayerRoles.PlayableScps.Scp106.Scp106SinkholeController Sinkhole
		{
			get
			{
				if (!_sinkholeSet)
				{
					SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106SinkholeController>(out _sinkholeCtrl);
					_sinkholeSet = true;
				}
				return _sinkholeCtrl;
			}
		}

		public bool CanActivateIdle => !IsSubmerged;

		public bool CanActivateShock => !IsSubmerged;

		public bool IsSubmerged
		{
			get
			{
				if (SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106StalkAbility>(out var subroutine))
				{
					return subroutine.IsSubmerged;
				}
				return false;
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			AllInstances.Add(this);
		}

		public void ResetObject()
		{
			AllInstances.Remove(this);
		}

		public global::PlayerStatsSystem.DamageHandlerBase ProcessDamageHandler(global::PlayerStatsSystem.DamageHandlerBase dhb)
		{
			if (!_sinkholeCtrl.IsHidden)
			{
				return dhb;
			}
			if (!ValidateDamageHandler(dhb))
			{
				dhb = new global::PlayerStatsSystem.UniversalDamageHandler();
			}
			return dhb;
		}

		private bool ValidateDamageHandler(global::PlayerStatsSystem.DamageHandlerBase dhb)
		{
			if (dhb is global::PlayerStatsSystem.UniversalDamageHandler universalDamageHandler && universalDamageHandler.TranslationId == global::PlayerStatsSystem.DeathTranslations.Tesla.Id)
			{
				return false;
			}
			if (dhb is global::PlayerStatsSystem.ExplosionDamageHandler || dhb is global::PlayerStatsSystem.MicroHidDamageHandler || dhb is global::PlayerStatsSystem.FirearmDamageHandler || (dhb is global::PlayerStatsSystem.Scp018DamageHandler && IsSubmerged))
			{
				return false;
			}
			return true;
		}

		public float GetSpawnChance(global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> alreadySpawned)
		{
			return 1f;
		}
	}
}
