namespace CustomPlayerEffects
{
    public class Deafened : StatusEffectBase, IHealablePlayerEffect
    {
        public bool IsHealable(ItemType it)
        {
            return it == ItemType.SCP500;
        }

        private void OnDestroy()
        {
            base.Hub.playerEffectsController.mixer.SetFloat("MasterVolumeLowpassFreq", 22000f);
        }
    }
}
