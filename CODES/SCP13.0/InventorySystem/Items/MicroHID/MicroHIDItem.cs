using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using InventorySystem.Items.Pickups;
using PlayerRoles.FirstPersonControl;
using Scp914;
using UnityEngine;

namespace InventorySystem.Items.MicroHID
{
	public class MicroHIDItem : ItemBase, IEquipDequipModifier, IStaminaModifier, IItemDescription, IItemNametag, IAcquisitionConfirmationTrigger, IUpgradeTrigger, IUniqueItem
	{
		public AudioClip PowerUpClip;

		public AudioClip PowerDownClip;

		public AudioClip PrimedClip;

		public AudioClip FireClip;

		public AudioClip FireToPrimedClip;

		public AudioClip FireToPowerDownClip;

		public float RemainingEnergy;

		public HidUserInput UserInput;

		public HidState State;

		[SerializeField]
		private AnimationCurve _energyConsumtionCurve;

		public const float SoundMaxDistance = 30f;

		public const float PreFireTime = 1.7f;

		private const float StaminaUsageMultp = 2f;

		private const float MinimalTimeToSwitchState = 0.35f;

		private const float PowerupTime = 5.95f;

		private const float PowerdownTime = 3.1f;

		private const float FireEnergyConsumption = 0.13f;

		private const float EnemyDotProductThreshold = 0.75f;

		private const float FriendlyDotProductThreshold = 0.98f;

		private const float DamagePerSecond = 1150f;

		private const float DamageOmnidirectionalDistance = 0.7f;

		private const float DamageMaxDistance = 6.3f;

		private const float HitsPerSecond = 8f;

		private static LayerMask _mask;

		private float _damageTimer;

		private readonly Stopwatch _stopwatch;

		private KeyCode _primaryKey;

		private KeyCode _secondaryKey;

		public override float Weight => 0f;

		public bool AcquisitionAlreadyReceived { get; set; }

		public bool AllowEquip => false;

		public bool AllowHolster => false;

		public bool StaminaModifierActive => false;

		public float StaminaUsageMultiplier => 0f;

		public float StaminaRegenMultiplier => 0f;

		public bool SprintingDisabled => false;

		public float Readiness => 0f;

		public string Description => null;

		public string Name => null;

		public static LayerMask WallMask => default(LayerMask);

		private byte EnergyToByte => 0;

		public static event Action<MicroHIDItem> OnStopCharging
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

		public void Recharge()
		{
		}

		public void ServerConfirmAcqusition()
		{
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public void ServerOnUpgraded(Scp914KnobSetting setting)
		{
		}

		public override void OnRemoved(ItemPickupBase pickup)
		{
		}

		public override void OnEquipped()
		{
		}

		public override void EquipUpdate()
		{
		}

		private void ExecuteClientside()
		{
		}

		private void ExecuteServerside()
		{
		}

		private void ServerSendStatus(HidStatusMessageType msgType, byte code)
		{
		}

		private void Fire()
		{
		}

		public bool CompareIdentical(ItemBase other)
		{
			return false;
		}
	}
}
