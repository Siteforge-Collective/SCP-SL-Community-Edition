using InventorySystem.Items.Pickups;
using Scp914;
using UnityEngine;

namespace InventorySystem.Items.Radio
{
	public class RadioItem : ItemBase, IAcquisitionConfirmationTrigger, IItemDescription, IItemNametag, IUpgradeTrigger, IUniqueItem
	{
		private const float DrainMultiplier = 0.5f;

		public RadioRangeMode[] Ranges;

		public AnimationCurve VoiceVolumeCurve;

		public AnimationCurve NoiseLevelCurve;

		private bool _enabled;

		private float _battery;

		private byte _lastSentBatteryLevel;

		private byte _rangeId;

		private static KeyCode _circleModeKey;

		private static KeyCode _toggleKey;

		public bool AcquisitionAlreadyReceived { get; set; }

		public override float Weight => 0f;

		public bool IsUsable => false;

		public string Description => null;

		public string Name => null;

		public byte BatteryPercent
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public RadioMessages.RadioRangeLevel RangeLevel => default(RadioMessages.RadioRangeLevel);

		public void ServerConfirmAcqusition()
		{
		}

		public bool CompareIdentical(ItemBase other)
		{
			return false;
		}

		public void ServerOnUpgraded(Scp914KnobSetting setting)
		{
		}

		public override void OnAdded(ItemPickupBase ipb)
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

		public void ServerProcessCmd(RadioMessages.RadioCommand command)
		{
		}

		public void UserReceiveInfo(RadioStatusMessage info)
		{
		}

		private void Update()
		{
		}

		private void SendStatusMessage()
		{
		}
	}
}
