using CustomRendering;
using System;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class FogControl : StatusEffectBase
    {
        public override byte MaxIntensity { get; } = (byte)Enum.GetValues(typeof(FogType)).Length;

        public override EffectClassification Classification => EffectClassification.Mixed;

        public void SetFogType(FogType fogType)
        {
            Intensity = (byte)(fogType + 1);
        }

        protected override void IntensityChanged(byte prevState, byte newState)
        {
            base.IntensityChanged(prevState, newState);

            if (!IsLocalPlayer && !IsSpectated)
            {
                return;
            }

            FogController singleton = FogController.Singleton;

            if (singleton == null)
            {
                return;
            }

            if (newState == 0)
            {
                singleton.ForcedFog = null;
                return;
            }

            FogType fogType = (FogType)(newState - 1);

            if (Enum.IsDefined(typeof(FogType), fogType))
            {
                singleton.ForcedFog = fogType;
            }
            else
            {
                singleton.ForcedFog = null;
            }
        }

        protected override void Disabled()
        {
            base.Disabled();

            if (IsLocalPlayer || IsSpectated)
            {
                FogController singleton = FogController.Singleton;
                if (singleton != null)
                {
                    singleton.ForcedFog = null;
                }
            }
        }
    }
}