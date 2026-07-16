namespace InventorySystem.Items.Thirdperson
{
	public class IdleThirdpersonItem : global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationClip _idleOverride;

		internal override void Initialize(global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel model, global::InventorySystem.Items.ItemIdentifier id)
		{
			base.Initialize(model, id);
			global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationManager.SetAnimation(model, global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override0, _idleOverride);
			model.Animator.SetFloat(global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase.HashOverrideBlend, 0f);
		}
	}
}
