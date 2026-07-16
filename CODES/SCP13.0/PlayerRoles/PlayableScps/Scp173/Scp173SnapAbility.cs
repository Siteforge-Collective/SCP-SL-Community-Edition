using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173SnapAbility : KeySubroutine<Scp173Role>
	{
		private const float SnapRayDistance = 1.5f;

		private const float TargetBacktrackingTime = 0.4f;

		private const float OwnerBacktrackingTime = 0.1f;

		private const float ForetrackingTime = 0.2f;

		private Scp173ObserversTracker _observersTracker;

		private ReferenceHub _targetHub;

		private static int _snapMask;

		private static int SnapMask => 0;

		public bool IsSpeeding => false;

		protected override ActionName TargetKey => default(ActionName);

		protected override void OnKeyDown()
		{
		}

		private static bool TryHitTarget(Transform origin, out ReferenceHub target)
		{
			target = null;
			return false;
		}

		protected override void Awake()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}
	}
}
