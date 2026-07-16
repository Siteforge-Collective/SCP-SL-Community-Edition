namespace InventorySystem.Items.Firearms
{
	[global::System.Serializable]
	public class FirearmRecoilPattern
	{
		private float _currentBulletsShot;

		private float _lastReading;

		private float _totalCutoff;

		public float SingleShotTolerance = 1.1f;

		public global::UnityEngine.AnimationCurve DropOverTime;

		public global::UnityEngine.AnimationCurve InaccuracyOverShots;

		public float GetEstimatedState(float timeBetweenShots)
		{
			if (_totalCutoff == 0f)
			{
				_totalCutoff = DropOverTime.keys[DropOverTime.keys.Length - 1].time;
			}
			float num = _currentBulletsShot;
			float num2 = global::UnityEngine.Time.timeSinceLevelLoad - _lastReading;
			float num3 = timeBetweenShots * SingleShotTolerance;
			if (num2 > num3)
			{
				num = ((!(num2 - num3 > _totalCutoff)) ? (num - DropOverTime.Evaluate(num2 - num3)) : 0f);
			}
			return global::UnityEngine.Mathf.Max(1f, num + 1f);
		}

		public void ApplyShot(float timeBetweenShots)
		{
			if (_lastReading != global::UnityEngine.Time.timeSinceLevelLoad)
			{
				_currentBulletsShot = GetEstimatedState(timeBetweenShots);
				_lastReading = global::UnityEngine.Time.timeSinceLevelLoad;
			}
		}

		public float GetInaccuracy()
		{
			return InaccuracyOverShots.Evaluate(_currentBulletsShot);
		}
	}
}
