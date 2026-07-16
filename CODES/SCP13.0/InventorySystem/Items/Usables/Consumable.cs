using System.Diagnostics;
using InventorySystem.Drawers;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.Usables
{
	public abstract class Consumable : UsableItem, IItemProgressbarDrawer, IItemDrawer
	{
		[SerializeField]
		private float _activationTime;

		private float _realActivationTime;

		[SerializeField]
		private bool _showProgressBar;

		private readonly Stopwatch _useStopwatch;

		private bool _alreadyActivated;

		public bool ProgressbarEnabled => false;

		public float ProgressbarMin => 0f;

		public float ProgressbarMax => 0f;

		public float ProgressbarValue { get; private set; }

		public float ProgressbarWidth => 0f;

		public override bool AllowHolster => false;

		private bool ActivationReady => false;

		public override void OnEquipped()
		{
		}

		public override void OnUsingStarted()
		{
		}

		public override void OnUsingCancelled()
		{
		}

		public override void ServerOnUsingCompleted()
		{
		}

		public override void EquipUpdate()
		{
		}

		public override void OnHolstered()
		{
		}

		public override void OnRemoved(ItemPickupBase pickup)
		{
		}

		private void ActivateEffects()
		{
		}

		protected abstract void OnEffectsActivated();
	}
}
