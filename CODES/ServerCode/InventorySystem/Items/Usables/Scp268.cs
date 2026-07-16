namespace InventorySystem.Items.Usables
{
	public class Scp268 : global::InventorySystem.Items.Usables.UsableItem, global::InventorySystem.Items.IWearableItem
	{
		private bool _isWorn;

		private const float InvisibilityTime = 15f;

		private const float CooldownTime = 120f;

		private readonly global::System.Diagnostics.Stopwatch _stopwatch = new global::System.Diagnostics.Stopwatch();

		public bool IsWorn
		{
			get
			{
				if (!base.IsLocalPlayer || global::Mirror.NetworkServer.active)
				{
					return _isWorn;
				}
				return base.Owner.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invisible>().Intensity != 0;
			}
			set
			{
				_isWorn = value;
			}
		}

		public override bool AllowHolster
		{
			get
			{
				if (IsUsing)
				{
					return IsWorn;
				}
				return true;
			}
		}

		private global::CustomPlayerEffects.Invisible Effect => base.Owner.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invisible>();

		public override void ServerOnUsingCompleted()
		{
			IsUsing = false;
			IsWorn = true;
			SetState(state: true);
			ServerSetPersonalCooldown(120f);
		}

		public override void OnHolstered()
		{
			base.OnHolstered();
			if (global::Mirror.NetworkServer.active)
			{
				SetState(state: false);
			}
			if (base.IsLocalPlayer)
			{
				IsUsing = false;
			}
		}

		public override void EquipUpdate()
		{
			base.EquipUpdate();
			if (base.IsLocalPlayer && IsWorn && IsUsing)
			{
				IsUsing = false;
			}
			if (global::Mirror.NetworkServer.active && _stopwatch.IsRunning && (_stopwatch.Elapsed.TotalSeconds >= 15.0 || Effect.Intensity == 0))
			{
				SetState(state: false);
			}
		}

		private void SetState(bool state)
		{
			if (state)
			{
				Effect.Intensity = 1;
				_stopwatch.Restart();
			}
			else if (IsWorn)
			{
				Effect.Intensity = 0;
				_stopwatch.Stop();
				IsWorn = false;
				if (base.OwnerInventory.CurItem.TypeId == ItemTypeId)
				{
					base.OwnerInventory.ServerSelectItem(0);
				}
			}
		}
	}
}
