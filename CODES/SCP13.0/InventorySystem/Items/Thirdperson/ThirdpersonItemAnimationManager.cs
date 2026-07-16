using System;
using System.Collections.Generic;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace InventorySystem.Items.Thirdperson
{
    public static class ThirdpersonItemAnimationManager
    {
        private static readonly List<KeyValuePair<AnimationClip, AnimationClip>> OverridesPuller = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        private static readonly Dictionary<ThirdpersonItemAnimationName, AnimationClip> CachedClips = new Dictionary<ThirdpersonItemAnimationName, AnimationClip>();

        public static bool TryGetDefaultAnimation(AnimatedCharacterModel target, ThirdpersonItemAnimationName name, out AnimationClip clip)
        {
            if (CachedClips.TryGetValue(name, out clip))
            {
                return true;
            }
            clip = null;
            OverridesPuller.Clear();
            target.AnimatorOverride.GetOverrides(OverridesPuller);
            foreach (KeyValuePair<AnimationClip, AnimationClip> item in OverridesPuller)
            {
                if (Enum.TryParse<ThirdpersonItemAnimationName>(item.Key.name, out var result))
                {
                    if (result == name)
                    {
                        clip = item.Key;
                    }
                    CachedClips[result] = item.Key;
                }
            }
            return clip != null;
        }

        public static void ResetOverrides(AnimatedCharacterModel target)
        {
            OverridesPuller.Clear();
            target.AnimatorOverride.GetOverrides(OverridesPuller);
            for (int i = 0; i < OverridesPuller.Count; i++)
            {
                OverridesPuller[i] = new KeyValuePair<AnimationClip, AnimationClip>(OverridesPuller[i].Key, null);
            }
            target.AnimatorOverride.ApplyOverrides(OverridesPuller);
        }

        public static void SetAnimation(AnimatedCharacterModel target, ThirdpersonItemAnimationName name, AnimationClip clip)
        {
            if (TryGetDefaultAnimation(target, name, out var clip2))
            {
                target.AnimatorOverride[clip2] = clip;
            }
        }
    }
}
