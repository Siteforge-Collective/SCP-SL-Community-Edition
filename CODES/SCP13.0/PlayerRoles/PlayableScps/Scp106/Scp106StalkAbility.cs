using GameObjectPools;
using InventorySystem.Items;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106StalkAbility : Scp106VigorAbilityBase, IPoolResettable, IInteractionBlocker
	{
		private const float VigorRegeneration = 0.03f;

		private const float VigorStalkCostStationary = 0.06f;

		private const float VigorStalkCostMoving = 0.09f;

		private const float MinVigorToSubmerge = 0.25f;

		private const BlockedInteraction Interactions = BlockedInteraction.All;

		private const float MovementRange = 5f;

		private const double MovementTimer = 2.0;

		private bool _isActive;

		private bool _valueDirty;

		private Scp106SinkholeController _sinkhole;

		private double _movementTime;

		private Vector3 _lastPosition;

		private bool _isMoving;

		private bool _wasMoving;

		private bool _isAppendingCooldown;

		protected override ActionName TargetKey => default(ActionName);

		public override bool IsSubmerged => false;

		public bool IsActive
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public BlockedInteraction BlockedInteractions => default(BlockedInteraction);

		public bool CanBeCleared => false;

		private void UpdateServerside()
		{
		}

		private void UpdateMovementState()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		protected override void OnKeyDown()
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}
	}
}
