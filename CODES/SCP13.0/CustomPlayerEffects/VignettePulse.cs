using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomPlayerEffects
{
	public class VignettePulse : PostProcessEffectPulse
	{
		private Vignette _effect;

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
