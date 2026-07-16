namespace InventorySystem.Items.Firearms.Modules
{
	public class ChambercheckEquipper : global::InventorySystem.Items.Firearms.Modules.IEquipperModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private int _equips;

		private bool _allowInteraction;

		private float _targetTime;

		private readonly int _pickupParamHash;

		private readonly float _normalTime;

		private readonly float _pickupTime;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly global::System.Diagnostics.Stopwatch _stopwatch;

		private readonly int _randomParamHash;

		public bool Standby
		{
			get
			{
				if (!_allowInteraction && _stopwatch.Elapsed.TotalSeconds >= (double)_targetTime)
				{
					_stopwatch.Stop();
					_allowInteraction = true;
				}
				return _allowInteraction;
			}
		}

		public ChambercheckEquipper(global::InventorySystem.Items.Firearms.Firearm firearm, string pickupParamName, float normalAnimationTime, float pickupAnimationTime)
		{
			_firearm = firearm;
			_pickupParamHash = global::UnityEngine.Animator.StringToHash(pickupParamName);
			_randomParamHash = global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Random;
			_normalTime = normalAnimationTime;
			_pickupTime = pickupAnimationTime;
			_stopwatch = new global::System.Diagnostics.Stopwatch();
		}

		public void OnEquipped()
		{
			_equips++;
			_allowInteraction = false;
			_targetTime = ((_equips == 1) ? _pickupTime : ((_normalTime + global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.DrawTimeModifier)) / global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.DrawSpeedMultiplier)));
			if (!_firearm.IsLocalPlayer && global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(_firearm.Status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked))
			{
				_targetTime = global::UnityEngine.Mathf.Max(0f, _targetTime - 0.1f);
			}
			_stopwatch.Restart();
		}
	}
}
