using System.Collections.Generic;
using PlayerRoles.Ragdolls;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049ResurrectAbility : RagdollAbilityBase<Scp049Role>
	{
		private enum ResurrectError
		{
			None = 0,
			TargetNull = 1,
			Expired = 2,
			MaxReached = 3,
			Refused = 4,
			TargetInvalid = 5
		}

		public const int MaxResurrections = 2;

		private const float TargetCorpseDuration = 18f;

		private const float HumanCorpseDuration = 12f;

		private const float ResurrectTargetReward = 200f;

		private static readonly Dictionary<uint, int> ResurrectedPlayers;

		private static readonly HashSet<uint> DeadZombies;

		private Scp049SenseAbility _senseAbility;

		[SerializeField]
		private AudioClip _surgeryClip;

		private AudioSource _surgerySource;

		protected override float RangeSqr => 0f;

		protected override float Duration => 0f;

		protected override void ServerComplete()
		{
		}

		protected override void Awake()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected override void OnKeyUp()
		{
		}

		public bool CheckRagdoll(BasicRagdoll ragdoll)
		{
			return false;
		}

		protected override byte ServerValidateBegin(BasicRagdoll ragdoll)
		{
			return 0;
		}

		protected override bool ServerValidateAny()
		{
			return false;
		}

		private ResurrectError CheckBeginConditions(BasicRagdoll ragdoll)
		{
			return default(ResurrectError);
		}

		private ResurrectError CheckMaxResurrections(ReferenceHub owner)
		{
			return default(ResurrectError);
		}

		private bool AnyConflicts(BasicRagdoll ragdoll)
		{
			return false;
		}

		private static bool IsSpawnableSpectator(ReferenceHub hub)
		{
			return false;
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		public static int GetResurrectionsNumber(ReferenceHub hub)
		{
			return 0;
		}

		public static void RegisterPlayerResurrection(ReferenceHub hub, int amount = 1)
		{
		}

		public static void ClearPlayerResurrections(ReferenceHub hub)
		{
		}
	}
}
