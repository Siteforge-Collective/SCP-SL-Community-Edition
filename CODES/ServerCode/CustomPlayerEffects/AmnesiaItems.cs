namespace CustomPlayerEffects
{
	public class AmnesiaItems : global::CustomPlayerEffects.StatusEffectBase, global::CustomPlayerEffects.IUsableItemModifierEffect, global::CustomPlayerEffects.IWeaponModifierPlayerEffect, global::CustomPlayerEffects.IPulseEffect
	{
		private float _activeTime;

		[global::UnityEngine.SerializeField]
		private ItemType[] _blockedUsableItems;

		[global::UnityEngine.SerializeField]
		private float _blockDelay;

		public bool ParamsActive
		{
			get
			{
				if (base.IsEnabled)
				{
					return _activeTime >= _blockDelay;
				}
				return false;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (base.IsEnabled)
			{
				_activeTime += global::UnityEngine.Time.deltaTime;
			}
		}

		protected override void Enabled()
		{
			base.Enabled();
			_activeTime = 0f;
		}

		public bool TryGetSpeed(ItemType item, out float speed)
		{
			speed = 0f;
			if (!global::Mirror.NetworkServer.active || !_blockedUsableItems.Contains(item) || _activeTime < _blockDelay)
			{
				return false;
			}
			ServerSendPulse();
			return true;
		}

		public bool TryGetWeaponParam(global::InventorySystem.Items.Firearms.Attachments.AttachmentParam param, out float val)
		{
			val = 1f;
			if (!global::Mirror.NetworkServer.active || param != global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.PreventReload || _activeTime < _blockDelay)
			{
				return false;
			}
			ServerSendPulse();
			return true;
		}

		public void ExecutePulse()
		{
		}

		private void ServerSendPulse()
		{
			base.Hub.playerEffectsController.ServerSendPulse<global::CustomPlayerEffects.AmnesiaItems>();
		}
	}
}
