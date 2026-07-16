using Mirror;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096RageManager : StandardSubroutine<Scp096Role>, IHumeShieldBlocker
	{
		public const float NormalHumeRegenerationRate = 15f;

		public const float MaxRageTime = 35f;

		public const float MinimumEnrageTime = 20f;

		private const float TimePerExtraTarget = 3f;

		private const float CalmingShieldMultiplier = 1f;

		private const float EnragingShieldMultiplier = 1f;

		public readonly AbilityCooldown HudRageDuration;

		private DynamicHumeShieldController _shieldController;

		private Scp096TargetsTracker _targetsTracker;

		private float _enragedTimeLeft;

		public bool HumeShieldBlocked { get; set; }

		public bool IsEnragedOrDistressed => false;

		public bool IsEnraged => false;

		public bool IsDistressed => false;

		public float EnragedTimeLeft
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public float TotalRageTime { get; private set; }

		public void ServerEnrage(float initialDuration = 20f)
		{
		}

		public void ServerEndEnrage(bool clearTime = true)
		{
		}

		public void ServerIncreaseDuration(ReferenceHub ownerHub, float addedDuration = 3f)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		protected override void Awake()
		{
		}

		private void OnRageUpdate(Scp096RageState newState)
		{
		}

		private void Update()
		{
		}

		private void UpdateRage()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
