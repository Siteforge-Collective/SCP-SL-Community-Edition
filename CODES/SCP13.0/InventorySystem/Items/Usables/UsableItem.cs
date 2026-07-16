using System;
using InventorySystem.Drawers;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.Usables
{
	public abstract class UsableItem : ItemBase, IEquipDequipModifier, IItemAlertDrawer, IItemDrawer, IItemDescription, IItemNametag
	{
		[NonSerialized]
		public float RemainingCooldown;

		[NonSerialized]
		public bool IsUsing;

		public float UseTime;

		public float MaxCancellableTime;

		[SerializeField]
		private float _weight;

		private static KeyCode _useKey;

		private static KeyCode _cancelKey;

		private static string _cooldownFormat;

		public const float AudibleSfxRange = 15f;

		public AudioClip UsingSfxClip;

		public virtual bool CanStartUsing => false;

		public override float Weight => 0f;

		public virtual string AlertText => null;

		public virtual string Description => null;

		public virtual string Name => null;

		public virtual bool AllowHolster => false;

		public virtual bool AllowEquip => false;

		public abstract void ServerOnUsingCompleted();

		public virtual void OnUsingStarted()
		{
		}

		public virtual void OnUsingCancelled()
		{
		}

		protected void ServerRemoveSelf()
		{
		}

		protected void ServerSetPersonalCooldown(float timeSeconds)
		{
		}

		protected void ServerSetGlobalItemCooldown(float timeSeconds)
		{
		}

		protected void ServerAddRegeneration(AnimationCurve regenCurve, float speedMultiplier = 1f, float hpMultiplier = 1f)
		{
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void OnEquipped()
		{
		}

		public override void EquipUpdate()
		{
		}

		public virtual bool ServerValidateCancelRequest(PlayerHandler handler)
		{
			return false;
		}

		public virtual bool ServerValidateStartRequest(PlayerHandler handler)
		{
			return false;
		}
	}
}
