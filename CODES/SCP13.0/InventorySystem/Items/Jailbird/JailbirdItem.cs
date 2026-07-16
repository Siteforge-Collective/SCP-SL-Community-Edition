using System;
using System.Runtime.CompilerServices;
using InventorySystem.Drawers;
using InventorySystem.Items.Autosync;
using InventorySystem.Items.Pickups;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Subroutines;
using Scp914;
using UnityEngine;

namespace InventorySystem.Items.Jailbird
{
	public class JailbirdItem : AutosyncItem, IItemDescription, IItemNametag, IUpgradeTrigger, IUniqueItem, IMovementInputOverride, IMovementSpeedModifier, IItemAlertDrawer, IItemDrawer, IEquipDequipModifier
	{
		private const ActionName TriggerMelee = ActionName.Shoot;

		private const ActionName TriggerCharge = ActionName.Zoom;

		private const ActionName InspectKey = ActionName.InspectItem;

		private const float ServerChargeTolerance = 0.4f;

		private const float HintDuration = 10f;

		private double _chargeResetTime;

		private bool _chargeLoading;

		private bool _charging;

		private bool _chargeAnyDetected;

		private bool _firstChargeFrame;

		private float _chargeLoadElapsed;

		private bool _attackTriggered;

		private static float _localRemainingHint;

		private readonly TolerantAbilityCooldown _serverAttackCooldown;

		private readonly AbilityCooldown _clientAttackCooldown;

		private readonly AbilityCooldown _clientDelayCooldown;

		[SerializeField]
		private AudioClip _hitClip;

		[SerializeField]
		private float _meleeDelay;

		[SerializeField]
		private float _meleeCooldown;

		[SerializeField]
		private float _chargeDuration;

		[SerializeField]
		private float _chargeReadyTime;

		[SerializeField]
		private float _chargeMovementSpeedMultiplier;

		[SerializeField]
		private float _chargeMovementSpeedLimiter;

		[SerializeField]
		private float _chargeCancelVelocitySqr;

		[SerializeField]
		private float _chargeAutoengageTime;

		[SerializeField]
		private float _chargeDetectionDelay;

		[SerializeField]
		private float _brokenRemoveTime;

		[SerializeField]
		private JailbirdHitreg _hitreg;

		[SerializeField]
		private JailbirdDeteriorationTracker _deterioration;

		public override float Weight => 0f;

		public int TotalChargesPerformed { get; private set; }

		public bool MovementOverrideActive => false;

		public Vector3 MovementOverrideDirection => default(Vector3);

		public bool MovementModifierActive => false;

		public float MovementSpeedMultiplier => 0f;

		public float MovementSpeedLimit => 0f;

		public bool AllowHolster => false;

		public bool AllowEquip => false;

		public string Description => null;

		public string Name => null;

		public string AlertText => null;

		public static event Action<ushort, JailbirdMessageType> OnRpcReceived
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

		public event Action<JailbirdMessageType> OnCmdSent
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

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void OnRemoved(ItemPickupBase pickup)
		{
		}

		public override void OnHolstered()
		{
		}

		public override void EquipUpdate()
		{
		}

		public void ServerReset()
		{
		}

		public void ServerOnUpgraded(Scp914KnobSetting setting)
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

		private void UpdateCharging()
		{
		}

		private void ServerAttack(NetworkReader reader)
		{
		}

		private void ClientAttack()
		{
		}

		private void SendRpc(JailbirdMessageType header, Action<NetworkWriter> extraData = null)
		{
		}

		private void SendCmd(JailbirdMessageType msg)
		{
		}

		public bool CompareIdentical(ItemBase other)
		{
			return false;
		}
	}
}
