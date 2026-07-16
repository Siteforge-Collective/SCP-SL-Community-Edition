using System.Diagnostics;
using CustomPlayerEffects;

namespace InventorySystem.Items.Usables
{
	public class Scp268 : UsableItem, IWearableItem
	{
		private bool _isWorn;

		private const float InvisibilityTime = 15f;

		private const float CooldownTime = 120f;

		private readonly Stopwatch _stopwatch;

		public override string AlertText => null;

		public bool IsWorn
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public override bool AllowHolster => false;

		private Invisible Effect => null;

		public override void ServerOnUsingCompleted()
		{
		}

		public override void OnHolstered()
		{
		}

		public override void EquipUpdate()
		{
		}

		private void SetState(bool state)
		{
		}
	}
}
