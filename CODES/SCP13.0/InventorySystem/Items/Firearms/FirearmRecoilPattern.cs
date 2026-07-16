using System;
using CameraShaking;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	[Serializable]
	public class FirearmRecoilPattern
	{
		private float _currentBulletsShot;

		private float _lastReading;

		private float _totalCutoff;

		public float SingleShotTolerance;

		public AnimationCurve DropOverTime;

		public AnimationCurve ZAxisScale;

		public AnimationCurve FovKickScale;

		public AnimationCurve HorizontalKickScale;

		public AnimationCurve VerticalKickScale;

		public AnimationCurve InaccuracyOverShots;

		public float GetEstimatedState(float timeBetweenShots)
		{
			return 0f;
		}

		public void ApplyShot(float timeBetweenShots)
		{
		}

		public RecoilSettings GetRecoil(RecoilSettings startRecoil)
		{
			return default(RecoilSettings);
		}

		public float GetInaccuracy()
		{
			return 0f;
		}
	}
}
