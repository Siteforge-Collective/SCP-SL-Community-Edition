using System.Collections.Generic;
using System.Diagnostics;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106StalkVisibilityController : StandardSubroutine<Scp106Role>
	{
		private const float AbsoluteDistance = 4f;

		private const float HealthToDistance = 0.3f;

		private const float InvisibleHeight = 8000f;

		private const float TransitionSpeed = 11.5f;

		private const float ServerTolerance = 5f;

		private const float SendCooldown = 0.08f;

		private const float SubmergeTolerance = 0.8f;

		private Scp106StalkAbility _stalk;

		private bool _anyFaded;

		private readonly Stopwatch _sendStopwatch;

		private readonly HashSet<CharacterModel> _affectedModels;

		public readonly Dictionary<int, byte> SyncDamage;

		private void UpdateAll()
		{
		}

		private void UpdateClient()
		{
		}

		private void UpdateSpectator()
		{
		}

		private void CleanupFade()
		{
		}

		private void UpdateServer()
		{
		}

		private bool GetVisibilityForPlayer(ReferenceHub hub, IFpcRole role)
		{
			return false;
		}

		private void RefreshDamageDictionary()
		{
		}

		protected override void Awake()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
