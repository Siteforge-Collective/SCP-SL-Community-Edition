namespace InventorySystem.Items.Firearms
{
	public abstract class Firearm : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IAcquisitionConfirmationTrigger, global::InventorySystem.Items.IZoomModifyingItem, global::PlayerRoles.FirstPersonControl.IMovementSpeedModifier, global::PlayerRoles.FirstPersonControl.IStaminaModifier, global::InventorySystem.Crosshairs.ICustomCrosshairItem, global::InventorySystem.Items.IItemNametag, global::InventorySystem.Items.ILightEmittingItem, global::InventorySystem.Items.IDisarmingItem
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Animator _animator;

		private const float UnloadTime = 0.6f;

		private readonly global::System.Diagnostics.Stopwatch _unloadStopwatch = new global::System.Diagnostics.Stopwatch();

		private global::UnityEngine.KeyCode _fireKey;

		private global::UnityEngine.KeyCode _adsKey;

		private global::UnityEngine.KeyCode _inspectKey;

		private global::UnityEngine.KeyCode _reloadKey;

		private global::UnityEngine.KeyCode _toggleFlashlightKey;

		private global::InventorySystem.Items.Firearms.FirearmStatus _status;

		private global::Footprinting.Footprint _lastFootprint;

		private bool _footprintValid;

		private bool _refillAmmo;

		private bool _sendStatusNextFrame;

		private bool _prevWasReloading;

		private bool _adsToggled;

		private bool _simulatedInstanceMode;

		public global::PlayerRoles.Faction FirearmAffiliation;

		public float BaseWeight;

		public float BaseLength;

		public global::InventorySystem.Items.Firearms.FirearmAudioClip[] AudioClips;

		public global::InventorySystem.Items.Firearms.FirearmGlobalSettingsPreset GlobalSettingsPreset;

		public global::InventorySystem.Items.Firearms.Attachments.Components.Attachment[] Attachments;

		public global::UnityEngine.Texture BodyIconTexture;

		public abstract global::InventorySystem.Items.Firearms.FirearmBaseStats BaseStats { get; }

		public bool AcquisitionAlreadyReceived { get; set; }

		public bool AllowDisarming => true;

		public override global::InventorySystem.Items.ItemDescriptionType DescriptionType => global::InventorySystem.Items.ItemDescriptionType.Firearm;

		public float ArmorPenetration => global::UnityEngine.Mathf.Min((float)BaseStats.BasePenetrationPercent * global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(this, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.PenetrationMultiplier) / 100f, 1f);

		public abstract ItemType AmmoType { get; }

		public abstract global::InventorySystem.Items.Firearms.Modules.IAmmoManagerModule AmmoManagerModule { get; set; }

		public abstract global::InventorySystem.Items.Firearms.Modules.IEquipperModule EquipperModule { get; set; }

		public abstract global::InventorySystem.Items.Firearms.Modules.IActionModule ActionModule { get; set; }

		public abstract global::InventorySystem.Items.Firearms.Modules.IInspectorModule InspectorModule { get; set; }

		public abstract global::InventorySystem.Items.Firearms.Modules.IHitregModule HitregModule { get; set; }

		public abstract global::InventorySystem.Items.Firearms.Modules.IAdsModule AdsModule { get; set; }

		public virtual global::InventorySystem.Items.Firearms.FirearmStatus Status
		{
			get
			{
				return _status;
			}
			set
			{
				global::InventorySystem.Items.Firearms.FirearmStatus status = _status;
				if (!(status == value))
				{
					_status = value;
					if (status.Attachments != value.Attachments)
					{
						global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.ApplyAttachmentsCode(this, value.Attachments, reValidate: true);
					}
					this.OnStatusChanged?.Invoke(status, value);
					_sendStatusNextFrame = true;
				}
			}
		}

		public global::Footprinting.Footprint Footprint
		{
			get
			{
				if (!_footprintValid)
				{
					_footprintValid = true;
					_lastFootprint = new global::Footprinting.Footprint(base.Owner);
				}
				return _lastFootprint;
			}
		}

		public bool IsSpectated => global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner);

		public bool SimulatedInstanceMode
		{
			get
			{
				return _simulatedInstanceMode;
			}
			set
			{
				bool simulatedInstanceMode = _simulatedInstanceMode;
				_simulatedInstanceMode = value;
				if (!simulatedInstanceMode && value)
				{
					OnSimulationModeEnabled();
				}
			}
		}

		public float StaminaUsageMultiplier => GlobalSettingsPreset.WeightToStaminaUsage.Evaluate(Weight);

		public float MovementSpeedMultiplier => GlobalSettingsPreset.WeightToMovementSpeed.Evaluate(Weight);

		public float StaminaRegenMultiplier => 1f;

		public float MovementSpeedLimit => GlobalSettingsPreset.MaxWeaponMovementSpeed;

		public bool SprintingDisabled => false;

		public global::UnityEngine.Animator ServerSideAnimator => _animator;

		public override float Weight => BaseWeight + global::System.Linq.Enumerable.Sum(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Where(Attachments, (global::InventorySystem.Items.Firearms.Attachments.Components.Attachment x) => x.IsEnabled), (global::InventorySystem.Items.Firearms.Attachments.Components.Attachment x) => x.Weight));

		public float Length => BaseLength + global::System.Linq.Enumerable.Sum(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Where(Attachments, (global::InventorySystem.Items.Firearms.Attachments.Components.Attachment x) => x.IsEnabled), (global::InventorySystem.Items.Firearms.Attachments.Components.Attachment x) => x.Length));

		public bool IsEmittingLight
		{
			get
			{
				if (global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(Status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.FlashlightEnabled))
				{
					return true;
				}
				if (!global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.HasAdvantageFlag(this, global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveAdvantages.NightVision))
				{
					return false;
				}
				return AdsModule.ServerAds;
			}
		}

		public virtual global::System.Type CrosshairType { get; protected set; } = typeof(global::InventorySystem.Crosshairs.SingleBulletFirearmCrosshair);

		public bool MovementModifierActive => base.IsEquipped;

		public bool StaminaModifierActive => base.IsEquipped;

		public event global::System.Action OnEquipUpdateCalled;

		public event global::System.Action OnEquippedCalled;

		public event global::System.Action OnHolsteredCalled;

		public event global::System.Action OnShotCalled;

		public event global::System.Action OnDryfired;

		public event global::System.Action<global::InventorySystem.Items.Firearms.FirearmStatus, global::InventorySystem.Items.Firearms.FirearmStatus> OnStatusChanged;

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (pickup is global::InventorySystem.Items.Firearms.FirearmPickup firearmPickup)
				{
					Status = new global::InventorySystem.Items.Firearms.FirearmStatus(firearmPickup.Status.Ammo, firearmPickup.Status.Flags, global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.ValidateAttachmentsCode(this, firearmPickup.Status.Attachments));
					_refillAmmo = firearmPickup.Distributed;
				}
				else
				{
					Status = new global::InventorySystem.Items.Firearms.FirearmStatus(0, global::InventorySystem.Items.Firearms.FirearmStatusFlags.None, global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.ValidateAttachmentsCode(this, 0u));
				}
				_footprintValid = false;
			}
		}

		public override void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnRemoved(pickup);
			if (pickup is global::InventorySystem.Items.Firearms.FirearmPickup firearmPickup)
			{
				firearmPickup.NetworkStatus = Status;
			}
		}

		public override void EquipUpdate()
		{
			UpdateAnims();
			if (global::Mirror.NetworkServer.active)
			{
				if (AmmoManagerModule.Standby && _prevWasReloading)
				{
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage(base.ItemSerial, global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ReloadStop));
				}
				_prevWasReloading = !AmmoManagerModule.Standby;
			}
			this.OnEquipUpdateCalled?.Invoke();
		}

		public override void AlwaysUpdate()
		{
			if (global::Mirror.NetworkServer.active && _sendStatusNextFrame)
			{
				global::Utils.Networking.NetworkUtils.SendToHubsConditionally(new global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage(base.ItemSerial, Status), (ReferenceHub x) => !x.isLocalPlayer);
				_sendStatusNextFrame = false;
			}
		}

		public override void OnEquipped()
		{
			ServerSideAnimator.enabled = true;
			ServerSideAnimator.Rebind();
			AdsModule.ServerAds = false;
			EquipperModule?.OnEquipped();
			this.OnEquippedCalled?.Invoke();
			_sendStatusNextFrame = true;
			_footprintValid = false;
		}

		public override void OnHolstered()
		{
			this.OnHolsteredCalled?.Invoke();
			ServerSideAnimator.enabled = false;
			_adsToggled = false;
			if (global::Mirror.NetworkServer.active)
			{
				global::InventorySystem.Items.Armor.BodyArmorUtils.RemoveEverythingExceedingLimits(base.OwnerInventory, global::InventorySystem.Items.Armor.BodyArmorUtils.TryGetBodyArmor(base.OwnerInventory, out var bodyArmor) ? bodyArmor : null, removeItems: false);
			}
		}

		public virtual void OnWeaponShot()
		{
			this.OnShotCalled?.Invoke();
		}

		public virtual void OnWeaponDryfired()
		{
			this.OnDryfired?.Invoke();
		}

		public abstract void UpdateAnims();

		protected virtual void OnSimulationModeEnabled()
		{
		}

		public virtual void ServerConfirmAcqusition()
		{
			if (_refillAmmo)
			{
				Status = new global::InventorySystem.Items.Firearms.FirearmStatus(AmmoManagerModule.MaxAmmo, Status.Flags, Status.Attachments);
			}
			else
			{
				base.OwnerInventory.connectionToClient.Send(new global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage(base.ItemSerial, Status));
			}
		}
	}
}
