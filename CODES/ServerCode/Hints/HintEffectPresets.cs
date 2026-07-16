namespace Hints
{
	public static class HintEffectPresets
	{
		public static global::UnityEngine.Keyframe[] CreateBumpKeyframes(float floorValue, float bumpValue, int count, float duration = 1f)
		{
			global::UnityEngine.Keyframe[] array = new global::UnityEngine.Keyframe[count * 2 + 1];
			float num = duration / (float)array.Length;
			float num2 = 0f;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new global::UnityEngine.Keyframe(num2, (i % 2 == 0) ? floorValue : bumpValue);
				num2 += num;
			}
			return array;
		}

		public static global::UnityEngine.AnimationCurve CreateBumpCurve(float floorValue, float bumpValue, int count, float duration = 1f)
		{
			return new global::UnityEngine.AnimationCurve(CreateBumpKeyframes(floorValue, bumpValue, count, duration))
			{
				postWrapMode = global::UnityEngine.WrapMode.Loop
			};
		}

		public static global::UnityEngine.AnimationCurve CreateTrailingBumpCurve(float floorValue, float bumpValue, int count, float startTrailPercent, float duration = 1f)
		{
			global::UnityEngine.Keyframe[] array = CreateBumpKeyframes(floorValue, bumpValue, count, duration * startTrailPercent);
			global::System.Array.Resize(ref array, array.Length + 1);
			array[array.Length - 1] = new global::UnityEngine.Keyframe(duration, floorValue);
			return new global::UnityEngine.AnimationCurve(array)
			{
				postWrapMode = global::UnityEngine.WrapMode.Loop
			};
		}

		public static global::Hints.AlphaCurveHintEffect FadeIn(float durationScalar = 1f, float startScalar = 0f, float iterations = 1f)
		{
			global::UnityEngine.AnimationCurve animationCurve = global::UnityEngine.AnimationCurve.EaseInOut(0f, 0f, iterations, 1f);
			animationCurve.postWrapMode = global::UnityEngine.WrapMode.Loop;
			return new global::Hints.AlphaCurveHintEffect(animationCurve, startScalar, durationScalar);
		}

		public static global::Hints.AlphaCurveHintEffect FadeOut(float durationScalar = 1f, float startScalar = 0f, float iterations = 1f)
		{
			global::UnityEngine.AnimationCurve animationCurve = global::UnityEngine.AnimationCurve.EaseInOut(0f, 1f, iterations, 0f);
			animationCurve.postWrapMode = global::UnityEngine.WrapMode.Loop;
			return new global::Hints.AlphaCurveHintEffect(animationCurve, startScalar, durationScalar);
		}

		public static global::Hints.HintEffect[] FadeInAndOut(float window, float durationScalar = 1f, float startScalar = 0f)
		{
			float num = (durationScalar - window) / 2f;
			return new global::Hints.HintEffect[2]
			{
				FadeIn(num, startScalar),
				FadeOut(num, startScalar + durationScalar - num)
			};
		}

		public static global::Hints.AlphaCurveHintEffect PulseAlpha(float floorValue, float peakValue, float iterationScalar = 1f, float startOffset = 0f)
		{
			return new global::Hints.AlphaCurveHintEffect(CreateBumpCurve(floorValue, peakValue, 1, iterationScalar), startOffset);
		}

		public static global::Hints.AlphaCurveHintEffect TrailingPulseAlpha(float floorValue, float peakValue, float startTrailScalar, float iterationScalar = 1f, float startScalar = 0f, int count = 1)
		{
			return new global::Hints.AlphaCurveHintEffect(CreateTrailingBumpCurve(floorValue, peakValue, count, startTrailScalar, iterationScalar), startScalar);
		}
	}
}
