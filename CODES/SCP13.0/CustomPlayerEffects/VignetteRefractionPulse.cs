using CustomRendering;
using UnityEngine.Rendering;

namespace CustomPlayerEffects
{
	public class VignetteRefractionPulse : PostProcessEffectPulse
	{
		private VignetteRefraction _effect;

		protected override float EffectValue
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		protected override void SetEffectType(VolumeProfile profile)
		{
		}
	}
}
