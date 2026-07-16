using CameraShaking;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096BecomeTargetShake : IShakeEffect
	{
		private float _remaining;

		private const float FovKick = 0.9f;

		private const float LerpSpeed = 0.92f;

		private const float RemoveThreshold = 0.001f;

		public bool GetEffect(ReferenceHub ply, out ShakeEffectValues values)
		{
            values = default(ShakeEffectValues);
			return false;
		}
	}
}
