using CameraShaking;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106PortalShake : IShakeEffect
	{
		private const float StalkHeight = -0.75f;

		private const float NormalizationLerp = 300f;

		private readonly Scp106Model _model;

		private readonly Scp106Role _role;

		private static Scp106PortalShake _latestEffect;

		public Scp106PortalShake(Scp106Role role, Scp106Model model)
		{
		}

		public bool GetEffect(ReferenceHub ply, out ShakeEffectValues values)
		{
            values = default(ShakeEffectValues);
            return false;
		}
	}
}
