using System.Collections.Generic;
using MapGeneration.Spawnables;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class AudioLogRippleTrigger : RippleTriggerBase
	{
		private readonly Dictionary<AudioLog, float> _cooldowns;

		public float CooldownPerLog;

		public float RangeMultiplier;

		public override void ResetObject()
		{
		}

		private void Update()
		{
		}

		private bool CanTriggerRipple(AudioLog audioLog)
		{
			return false;
		}
	}
}
