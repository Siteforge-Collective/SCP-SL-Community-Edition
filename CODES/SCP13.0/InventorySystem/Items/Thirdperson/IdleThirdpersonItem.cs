using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace InventorySystem.Items.Thirdperson
{
    public class IdleThirdpersonItem : ThirdpersonItemBase
    {
        [SerializeField]
        private AnimationClip _idleOverride;

        internal override void Initialize(HumanCharacterModel model, ItemIdentifier id)
        {
            base.Initialize(model, id);
            ThirdpersonItemAnimationManager.SetAnimation(model, ThirdpersonItemAnimationName.Override0, _idleOverride);
            model.Animator.SetFloat(ThirdpersonItemBase.HashOverrideBlend, 0f);
        }
    }
}
