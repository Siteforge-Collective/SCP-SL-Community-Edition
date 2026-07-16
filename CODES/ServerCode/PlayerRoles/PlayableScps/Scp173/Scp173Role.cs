namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173Role : global::PlayerRoles.PlayableScps.FpcStandardScp, global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole, global::PlayerRoles.IArmoredRole, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole, global::PlayerRoles.PlayableScps.HUDs.IHudScp, global::PlayerRoles.PlayableScps.ISpawnableScp
	{
		[global::UnityEngine.SerializeField]
		private int _armorEfficacy;

		private ReferenceHub _owner;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer _audio;

		private bool _damagedEventAssigned;

		private bool DamagedEventActive
		{
			get
			{
				return _damagedEventAssigned;
			}
			set
			{
				if (value != DamagedEventActive && (!value || global::Mirror.NetworkServer.active))
				{
					global::PlayerStatsSystem.PlayerStats playerStats = _owner.playerStats;
					if (value)
					{
						playerStats.OnThisPlayerDamaged += OnDamaged;
					}
					else
					{
						playerStats.OnThisPlayerDamaged -= OnDamaged;
					}
					_damagedEventAssigned = value;
				}
			}
		}

		public global::PlayerStatsSystem.ScpDamageHandler DamageHandler
		{
			get
			{
				if (!TryGetOwner(out var hub))
				{
					throw new global::System.InvalidOperationException("Damage handler could not be created for an inactive instance of SCP-173.");
				}
				return new global::PlayerStatsSystem.ScpDamageHandler(hub, global::PlayerStatsSystem.DeathTranslations.Scp173);
			}
		}

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule SubroutineModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HUDs.ScpHudBase HudPrefab { get; private set; }

		private void OnDamaged(global::PlayerStatsSystem.DamageHandlerBase obj)
		{
			if (obj is global::PlayerStatsSystem.FirearmDamageHandler)
			{
				_audio.ServerSendSound(global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173SoundId.Hit);
			}
		}

		private void Awake()
		{
			SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer>(out _audio);
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			TryGetOwner(out _owner);
			DamagedEventActive = true;
		}

		public override void DisableRole(global::PlayerRoles.RoleTypeId newRole)
		{
			base.DisableRole(newRole);
			DamagedEventActive = false;
		}

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
