using PlayerStatsSystem;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class RainbowTaste : StatusEffectBase, ISpectatorDataPlayerEffect
    {
        private static readonly float[] Multipliers = new float[] { 1f, 0.8f, 0.6f, 0.55f };

        public override EffectClassification Classification => EffectClassification.Positive;

        public override byte MaxIntensity => 255;

        public bool GetSpectatorText(out string s)
        {
            s = "Rainbow Taste";
            return true;
        }

        public static float CurrentMultiplier(ReferenceHub ply)
        {
            if (ply == null)
            {
                return 1f;
            }

            PlayerEffectsController controller = ply.playerEffectsController;
            if (controller == null)
            {
                return 1f;
            }

            RainbowTaste effect = controller.GetEffect<RainbowTaste>();
            if (effect == null || !effect.IsEnabled)
            {
                return 1f;
            }

            byte intensity = effect.Intensity;
            int index = Mathf.FloorToInt(intensity / 64f); // 0-63 → 0, 64-127 → 1, 128-191 → 2, 192-255 → 3
            index = Mathf.Clamp(index, 0, Multipliers.Length - 1);

            return Multipliers[index];
        }

        public static bool CheckPlayer(ReferenceHub ply)
        {
            if (ply == null)
            {
                return false;
            }

            PlayerEffectsController controller = ply.playerEffectsController;
            if (controller == null)
            {
                return false;
            }

            RainbowTaste effect = controller.GetEffect<RainbowTaste>();
            return effect != null && effect.IsEnabled;
        }
    }
}