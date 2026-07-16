namespace CustomPlayerEffects
{
	public class SeveredHands : global::CustomPlayerEffects.TickingEffectBase, global::InventorySystem.Items.IInteractionBlocker
	{
		private const global::InventorySystem.Items.BlockedInteraction Interactions = global::InventorySystem.Items.BlockedInteraction.All;

		private static readonly int HashSeveredHands = global::UnityEngine.Animator.StringToHash("SeveredHands");

		[global::UnityEngine.SerializeField]
		private float _tickDamage;

		public bool CanBeCleared => !base.IsEnabled;

		public global::InventorySystem.Items.BlockedInteraction BlockedInteractions => global::InventorySystem.Items.BlockedInteraction.All;

		protected override void Enabled()
		{
			base.Enabled();
			base.Hub.interCoordinator.AddBlocker(this);
			ChangeHandsState(handsCut: true);
		}

		protected override void Disabled()
		{
			base.Disabled();
			ChangeHandsState(handsCut: false);
		}

		protected override void OnTick()
		{
			if (global::Mirror.NetworkServer.active)
			{
				global::InventorySystem.InventoryExtensions.ServerDropItem(base.Hub.inventory, base.Hub.inventory.CurItem.SerialNumber);
				base.Hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(_tickDamage, global::PlayerStatsSystem.DeathTranslations.SeveredHands));
			}
		}

		private void ChangeHandsState(bool handsCut)
		{
			if (base.Hub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole && humanRole.FpcModule.CharacterModelInstance is global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel humanCharacterModel)
			{
				humanCharacterModel.Animator.SetBool(HashSeveredHands, handsCut);
			}
		}
	}
}
