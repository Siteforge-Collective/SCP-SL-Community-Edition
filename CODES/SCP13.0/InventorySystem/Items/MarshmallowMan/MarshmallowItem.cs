using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InventorySystem.Items.Autosync;
using InventorySystem.Items.Pickups;
using Mirror;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace InventorySystem.Items.MarshmallowMan
{
	public class MarshmallowItem : AutosyncItem, IEquipDequipModifier, IInteractionBlocker
	{
		private enum RpcType
		{
			AttackStart = 0,
			Hit = 1,
			Holster = 2
		}

		public const float HolsterAnimTime = 1.1f;

		[SerializeField]
		private float _detectionRadius;

		[SerializeField]
		private float _detectionOffset;

		[SerializeField]
		private float _attackCooldown;

		[SerializeField]
		private float _attackDamage;

		private bool _markedAsRemoved;

		private bool _preventAttacks;

		private MarshmallowEffect _marshmallowEffect;

		private readonly TolerantAbilityCooldown _cooldown;

		private readonly HashSet<ReferenceHub> _detectedPlayers;

		private UniversalDamageHandler NewDamageHandler => null;

		public override float Weight => 0f;

		public bool AllowHolster => false;

		public bool AllowEquip => false;

		public BlockedInteraction BlockedInteractions => default(BlockedInteraction);

		public bool CanBeCleared => false;

		public static event Action<ushort> OnSwing
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public static event Action<ushort> OnHit
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public static event Action<ushort> OnHolsterRequested
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void ServerRequestHolster()
		{
		}

		public override ItemPickupBase ServerDropItem()
		{
			return null;
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void OnHolstered()
		{
		}

		public override void OnRemoved(ItemPickupBase pickup)
		{
		}

		public override void ClientProcessRpcTemplate(NetworkReader reader, ushort serial)
		{
		}

		public override void ClientProcessRpcLocally(NetworkReader reader)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void EquipUpdate()
		{
		}

		private void UpdateClientInput()
		{
		}

		private void ClientWriteAttackResult(NetworkWriter writer)
		{
		}

		private void ServerAttack(ReferenceHub syncTarget)
		{
		}

		private ArraySegment<IDestructible> DetectDestructibles()
		{
			return default(ArraySegment<IDestructible>);
		}
	}
}
