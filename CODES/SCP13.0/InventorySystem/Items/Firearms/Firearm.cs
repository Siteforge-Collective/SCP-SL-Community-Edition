using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Footprinting;
using InventorySystem.Crosshairs;
using InventorySystem.Items.Autosync;
using InventorySystem.Items.Firearms.Attachments.Components;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Pickups;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using UserSettings;

namespace InventorySystem.Items.Firearms
{
	public abstract class Firearm : AutosyncItem, IAcquisitionConfirmationTrigger, IZoomModifyingItem, IUniqueItem, IMovementSpeedModifier, IStaminaModifier, ICustomCrosshairItem, IItemNametag, ILightEmittingItem, IDisarmingItem
	{
		[SerializeField]
		private Animator _animator;

		private const float UnloadTime = 0.6f;

		private readonly Stopwatch _unloadStopwatch;

		private KeyCode _fireKey;

		private KeyCode _inspectKey;

		private KeyCode _reloadKey;

		private KeyCode _toggleFlashlightKey;

		private FirearmStatus _status;

		private Footprint _lastFootprint;

		private bool _footprintValid;

		private bool _refillAmmo;

		private bool _sendStatusNextFrame;

		private bool _prevWasReloading;

		private bool _simulatedInstanceMode;

		private bool _hasViewmodel;

		private bool _viewmodelCacheSet;

		private static readonly ToggleOrHoldInput AdsInput;

		public Faction FirearmAffiliation;

		public float BaseWeight;

		public float BaseLength;

		public FirearmAudioClip[] AudioClips;

		public FirearmGlobalSettingsPreset GlobalSettingsPreset;

		public Attachment[] Attachments;

		public Texture BodyIconTexture;

		public abstract FirearmBaseStats BaseStats { get; }

		public new bool AcquisitionAlreadyReceived { get; set; }

		public bool AllowDisarming => false;

		public override ItemDescriptionType DescriptionType => default(ItemDescriptionType);

		public float ArmorPenetration => 0f;

		public abstract ItemType AmmoType { get; }

		public abstract IAmmoManagerModule AmmoManagerModule { get; set; }

		public abstract IEquipperModule EquipperModule { get; set; }

		public abstract IActionModule ActionModule { get; set; }

		public abstract IInspectorModule InspectorModule { get; set; }

		public abstract IHitregModule HitregModule { get; set; }

		public abstract IAdsModule AdsModule { get; set; }

		public virtual FirearmStatus Status
		{
			get
			{
				return default(FirearmStatus);
			}
			set
			{
			}
		}

		public Footprint Footprint => default(Footprint);

		public bool IsSpectated => false;

		public bool SimulatedInstanceMode
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float StaminaUsageMultiplier => 0f;

		public float StaminaRegenMultiplier => 0f;

		public float MovementSpeedMultiplier => 0f;

		public float MovementSpeedLimit => 0f;

		public bool SprintingDisabled => false;

		public Animator ServerSideAnimator => null;

		public AnimatedFirearmViewmodel ClientViewmodel => null;

		public float ZoomAmount => 0f;

		public string Name => null;

		public bool HasViewmodel => false;

		public float SensitivityScale => 0f;

		public override float Weight => 0f;

		public float Length => 0f;

		public bool IsEmittingLight => false;

		public virtual Type CrosshairType { get; protected set; }

		public bool MovementModifierActive => false;

		public bool StaminaModifierActive => false;

		public event Action OnEquipUpdateCalled
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

		public event Action OnEquippedCalled
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

		public event Action OnHolsteredCalled
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

		public event Action OnShotCalled
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

		public event Action OnDryfired
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

		public event Action<FirearmStatus, FirearmStatus> OnStatusChanged
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

		public override void EquipUpdate()
		{
		}

		public override void AlwaysUpdate()
		{
		}

		public override void OnEquipped()
		{
		}

		public override void OnHolstered()
		{
		}

		public virtual void OnWeaponShot()
		{
		}

		public virtual void OnWeaponDryfired()
		{
		}

		public abstract void UpdateAnims();

		protected virtual void OnSimulationModeEnabled()
		{
		}

		private void ProcessReceivedStatus(StatusMessage msg)
		{
		}

		private void OnDestroy()
		{
		}

		private void GetKeycodes()
		{
		}

		private void UpdateKeys()
		{
		}

		public virtual bool CompareIdentical(ItemBase other)
		{
			return false;
		}

		public new virtual void ServerConfirmAcqusition()
		{
		}
	}
}
