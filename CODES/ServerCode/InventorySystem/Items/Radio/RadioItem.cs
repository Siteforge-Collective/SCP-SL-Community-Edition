namespace InventorySystem.Items.Radio
{
	public class RadioItem : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IAcquisitionConfirmationTrigger, global::InventorySystem.Items.IItemDescription, global::InventorySystem.Items.IItemNametag, global::InventorySystem.Items.IUpgradeTrigger
	{
		public global::InventorySystem.Items.Radio.RadioRangeMode[] Ranges;

		public global::UnityEngine.AnimationCurve VoiceVolumeCurve;

		public global::UnityEngine.AnimationCurve NoiseLevelCurve;

		private bool _enabled;

		private float _battery;

		private byte _lastSentBatteryLevel;

		private byte _rangeId;

		private static global::UnityEngine.KeyCode _circleModeKey;

		private static global::UnityEngine.KeyCode _toggleKey;

		public bool AcquisitionAlreadyReceived { get; set; }

		public override float Weight => 1.7f;

		public bool IsUsable
		{
			get
			{
				if (_enabled)
				{
					return _battery > 0f;
				}
				return false;
			}
		}

		public byte BatteryPercent
		{
			get
			{
				return (byte)global::UnityEngine.Mathf.RoundToInt(_battery * 100f);
			}
			set
			{
				_battery = (float)(int)value / 100f;
			}
		}

		public global::InventorySystem.Items.Radio.RadioMessages.RadioRangeLevel RangeLevel => (global::InventorySystem.Items.Radio.RadioMessages.RadioRangeLevel)(_enabled ? _rangeId : (-1));

		public void ServerConfirmAcqusition()
		{
			SendStatusMessage();
		}

		public void ServerOnUpgraded(global::Scp914.Scp914KnobSetting setting)
		{
			BatteryPercent = 100;
			SendStatusMessage();
		}

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (ipb is global::InventorySystem.Items.Radio.RadioPickup radioPickup)
				{
					_enabled = radioPickup.SavedEnabled;
					_rangeId = radioPickup.SavedRange;
					_battery = radioPickup.SavedBattery;
				}
				else
				{
					_enabled = true;
					_battery = 1f;
					_rangeId = 1;
				}
			}
		}

		public override void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			if (global::Mirror.NetworkServer.active && pickup is global::InventorySystem.Items.Radio.RadioPickup radioPickup)
			{
				radioPickup.NetworkSavedEnabled = _enabled;
				radioPickup.NetworkSavedRange = _rangeId;
				radioPickup.SavedBattery = _battery;
			}
		}

		public override void OnEquipped()
		{
			if (global::Mirror.NetworkServer.active)
			{
				SendStatusMessage();
			}
		}

		public override void EquipUpdate()
		{
		}

		public void ServerProcessCmd(global::InventorySystem.Items.Radio.RadioMessages.RadioCommand command)
		{
			switch (command)
			{
			case global::InventorySystem.Items.Radio.RadioMessages.RadioCommand.Enable:
				if (_enabled || _battery <= 0f || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerRadioToggle, base.Owner, this, true))
				{
					return;
				}
				_enabled = true;
				break;
			case global::InventorySystem.Items.Radio.RadioMessages.RadioCommand.Disable:
				if (!_enabled || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerRadioToggle, base.Owner, this, false))
				{
					return;
				}
				_enabled = false;
				break;
			case global::InventorySystem.Items.Radio.RadioMessages.RadioCommand.ChangeRange:
			{
				byte b = (byte)(_rangeId + 1);
				if (b >= Ranges.Length)
				{
					b = 0;
				}
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerChangeRadioRange, base.Owner, this, b))
				{
					return;
				}
				_rangeId = b;
				break;
			}
			}
			SendStatusMessage();
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active && IsUsable)
			{
				float num = (global::VoiceChat.Playbacks.PersonalRadioPlayback.IsTransmitting(base.Owner) ? ((float)Ranges[_rangeId].MinuteCostWhenTalking) : Ranges[_rangeId].MinuteCostWhenIdle);
				float num2 = global::UnityEngine.Time.deltaTime * (num / 60f / 100f);
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerUsingRadio, base.Owner, this, num2))
				{
					_battery = global::UnityEngine.Mathf.Clamp01(_battery - num2);
				}
				if (_battery == 0f)
				{
					_enabled = false;
				}
				if (global::UnityEngine.Mathf.Abs(_lastSentBatteryLevel - BatteryPercent) >= 1 && base.OwnerInventory.CurItem.TypeId == ItemType.Radio)
				{
					SendStatusMessage();
				}
			}
		}

		private void SendStatusMessage()
		{
			_lastSentBatteryLevel = BatteryPercent;
			global::Mirror.NetworkServer.SendToReady(new global::InventorySystem.Items.Radio.RadioStatusMessage(this));
		}
	}
}
