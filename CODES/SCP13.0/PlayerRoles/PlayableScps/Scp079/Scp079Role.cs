using System.Collections.Generic;
using GameObjectPools;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using PlayerRoles.Spectating;
using PlayerRoles.Subroutines;
using PlayerRoles.Voice;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079Role : PlayerRoleBase, ISubroutinedRole, ISpectatableRole, ISpawnableScp, ITeslaControllerRole, IAdvancedCameraController, ICameraController, IPoolResettable, IPoolSpawnable, IVoiceRole, IAvatarRole, IHudScp, IAmbientLightRole, IAFKRole
	{
		public static readonly HashSet<Scp079Role> ActiveInstances;

		private Scp079CurrentCameraSync _curCamSync;

		private Vector3 _lastCamPos;

		public static Scp079Role LocalInstance { get; private set; }

		public static bool LocalInstanceActive { get; private set; }

		[field: SerializeField]
		public ScpHudBase HudPrefab { get; private set; }

		[field: SerializeField]
		public SubroutineManagerModule SubroutineModule { get; private set; }

		[field: SerializeField]
		public VoiceModuleBase VoiceModule { get; private set; }

		[field: SerializeField]
		public Texture RoleAvatar { get; private set; }

		[field: SerializeField]
		public SpectatableModuleBase SpectatorModule { get; private set; }

		public override RoleTypeId RoleTypeId => default(RoleTypeId);

		public override Team Team => default(Team);

		public override Color RoleColor => default(Color);

		public Vector3 CameraPosition => default(Vector3);

		public float VerticalRotation => 0f;

		public float HorizontalRotation => 0f;

		public float RollRotation => 0f;

		public Scp079Camera CurrentCamera => null;

		public bool IsSpectated => false;

		public float AmbientLight => 0f;

		public bool InsufficientLight => false;

		public bool IsAFK => false;

		public bool CanActivateShock { get; }

		public float GetSpawnChance(List<RoleTypeId> alreadySpawned)
		{
			return 0f;
		}

		public bool IsInIdleRange(TeslaGate teslaGate)
		{
			return false;
		}

		public void ResetObject()
		{
		}

		public void SpawnObject()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
