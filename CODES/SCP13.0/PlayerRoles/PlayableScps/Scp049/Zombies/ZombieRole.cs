using Mirror;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieRole : FpcStandardScp, ISubroutinedRole, IHumeShieldedRole, IHudScp, IAdvancedCameraController, ICameraController
	{
		[SerializeField]
		private GameObject _confirmBoxPrefab;

		[SerializeField]
		[Tooltip("The maximum amount of health the special zombie will have.")]
		private ushort _specialMaxHp;

		[Tooltip("Modifier applied based on how many times the zombie was revived for.")]
		[SerializeField]
		private float _revivesModifier;

		private ushort _syncMaxHealth;

		private bool _showConfirmationBox;

		private ZombieConsumeAbility _consumeAbility;

		public override float MaxHealth => 0f;

		public override Vector3 CameraPosition => default(Vector3);

		public override float HorizontalRotation => 0f;

		public override float VerticalRotation => 0f;

		public float RollRotation => 0f;

		[field: SerializeField]
		public HumeShieldModuleBase HumeShieldModule { get; private set; }

		[field: SerializeField]
		public SubroutineManagerModule SubroutineModule { get; private set; }

		[field: SerializeField]
		public ScpHudBase HudPrefab { get; private set; }

		private void Awake()
		{
		}

		public override void WritePublicSpawnData(NetworkWriter writer)
		{
		}

		public override void ReadSpawnData(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void DisableRole(RoleTypeId newRole)
		{
		}
	}
}
