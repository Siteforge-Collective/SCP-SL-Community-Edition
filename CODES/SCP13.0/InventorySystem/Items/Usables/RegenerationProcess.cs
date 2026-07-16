using UnityEngine;

namespace InventorySystem.Items.Usables
{
	public class RegenerationProcess
	{
		private readonly AnimationCurve _regenCurve;

		private readonly float _maxTime;

		private readonly float _speedMultip;

		private readonly float _hpMultip;

		private float _healValue;

		private float _elapsed;

		public RegenerationProcess(AnimationCurve regenCurve, float speedMultiplier, float healthPointsMultiplier)
		{
		}

		public void GetValue(out bool isDone, out int value)
		{
			isDone = default(bool);
			value = default(int);
		}
	}
}
