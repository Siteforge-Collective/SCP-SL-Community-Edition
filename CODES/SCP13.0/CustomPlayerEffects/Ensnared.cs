using PlayerRoles.FirstPersonControl;

namespace CustomPlayerEffects
{
    public class Ensnared : StatusEffectBase, IMovementSpeedModifier
    {
        public bool MovementModifierActive => base.IsEnabled;

        public float MovementSpeedMultiplier => 0f;

        public float MovementSpeedLimit => 0f;
    }
}
