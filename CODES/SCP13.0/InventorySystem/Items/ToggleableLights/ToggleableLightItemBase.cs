using System;
using UnityEngine;

namespace InventorySystem.Items.ToggleableLights
{
	public abstract class ToggleableLightItemBase : ItemBase, IItemDescription, IItemNametag, ILightEmittingItem
	{
		protected const float ToggleCooldownTime = 0.13f;

		protected const float EquipCooldownTime = 0.6f;

		private bool _isEmitting;

		[NonSerialized]
		public float NextAllowedTime;

		public AudioClip OnClip;

		public AudioClip OffClip;

		private static readonly ActionName[] ToggleKeys;

		public override float Weight => 0f;

		public string Name => null;

		public string Description => null;

		public virtual bool IsEmittingLight
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		protected abstract void SetLightSourceStatus(bool value);

		public override void OnEquipped()
		{
		}

		private void ForceEnable()
		{
		}

		public override void EquipUpdate()
		{
		}

		protected virtual void OnToggled()
		{
		}

		public void ClientSendRequest(bool value)
		{
		}
	}
}
