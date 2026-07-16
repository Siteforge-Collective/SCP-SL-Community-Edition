namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieRole : global::PlayerRoles.PlayableScps.FpcStandardScp, global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole, global::PlayerRoles.PlayableScps.HUDs.IHudScp, global::PlayerRoles.IAdvancedCameraController, global::PlayerRoles.ICameraController
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _confirmBoxPrefab;

		[global::UnityEngine.Tooltip("The maximum amount of health the special zombie will have.")]
		[global::UnityEngine.SerializeField]
		private ushort _specialMaxHp = 600;

		[global::UnityEngine.Tooltip("Modifier applied based on how many times the zombie was revived for.")]
		[global::UnityEngine.SerializeField]
		private float _revivesModifier = 0.9f;

		private ushort _syncMaxHealth;

		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility _consumeAbility;

		public override float MaxHealth => (int)_syncMaxHealth;

		public override global::UnityEngine.Vector3 CameraPosition => _consumeAbility.ProcessCamPos(base.CameraPosition);

		public override float HorizontalRotation => _consumeAbility.ProcessRotation().y;

		public override float VerticalRotation => _consumeAbility.ProcessRotation().x;

		public float RollRotation => _consumeAbility.ProcessRotation().z;

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule SubroutineModule { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.HUDs.ScpHudBase HudPrefab { get; private set; }

		private void Awake()
		{
			SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility>(out _consumeAbility);
		}

		public override void WritePublicSpawnData(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, _syncMaxHealth);
			base.WritePublicSpawnData(writer);
		}

		public override void ReadSpawnData(global::Mirror.NetworkReader reader)
		{
			_syncMaxHealth = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			base.ReadSpawnData(reader);
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			if (!TryGetOwner(out var owner) || !global::Mirror.NetworkServer.active)
			{
				return;
			}
			if (global::Utils.NonAllocLINQ.HashsetExtensions.Any(ReferenceHub.AllHubs, (ReferenceHub x) => x.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp049.Scp049Role scp049Role && scp049Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049SenseAbility>(out var subroutine) && subroutine.DeadTargets.Contains(owner)))
			{
				_syncMaxHealth = _specialMaxHp;
			}
			else
			{
				int resurrectionsNumber = global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility.GetResurrectionsNumber(owner);
				float num = base.MaxHealth;
				for (int num2 = 0; num2 < resurrectionsNumber; num2++)
				{
					num *= _revivesModifier;
				}
				_syncMaxHealth = (ushort)(global::UnityEngine.Mathf.RoundToInt(num / 10f) * 10);
			}
			global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility.RegisterPlayerResurrection(owner);
		}

		public override void DisableRole(global::PlayerRoles.RoleTypeId newRole)
		{
			bool isLocalPlayer = base.IsLocalPlayer;
			base.DisableRole(newRole);
			_syncMaxHealth = 0;
			if (isLocalPlayer && newRole == global::PlayerRoles.RoleTypeId.Spectator && global::Utils.NonAllocLINQ.HashsetExtensions.Any(ReferenceHub.AllHubs, (ReferenceHub x) => x.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp049.Scp049Role))
			{
				global::UnityEngine.Object.Instantiate(_confirmBoxPrefab);
			}
		}
	}
}
