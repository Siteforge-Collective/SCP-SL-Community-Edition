using MapGeneration.Distributors;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class GeneratorRippleTrigger : RippleTriggerBase
	{
		private readonly Vector3 _offset;

		private const float RangeSqr = 100f;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void OnCount(Scp079Generator generator)
		{
		}
	}
}
