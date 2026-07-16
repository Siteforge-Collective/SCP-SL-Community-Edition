public static class AnimationCurveUtils
{
	public static global::UnityEngine.AnimationCurve MakeLinearCurve(global::UnityEngine.Keyframe[] keyframes, global::UnityEngine.WrapMode preWrapMode = global::UnityEngine.WrapMode.Once, global::UnityEngine.WrapMode postWrapMode = global::UnityEngine.WrapMode.Once)
	{
		return new global::UnityEngine.AnimationCurve(MakeLinearKeyframes(keyframes))
		{
			preWrapMode = preWrapMode,
			postWrapMode = postWrapMode
		};
	}

	public static global::UnityEngine.Keyframe[] MakeLinearKeyframes(params global::UnityEngine.Keyframe[] keyframes)
	{
		for (int i = 0; i < keyframes.Length; i++)
		{
			keyframes[i] = MakeLinearKeyframe(keyframes[i]);
		}
		return keyframes;
	}

	public static global::UnityEngine.Keyframe MakeLinearKeyframe(float time, float value)
	{
		return new global::UnityEngine.Keyframe(time, value, 0f, 0f, 0f, 0f);
	}

	public static global::UnityEngine.Keyframe MakeLinearKeyframe(global::UnityEngine.Keyframe keyframe)
	{
		return MakeLinearKeyframe(keyframe.time, keyframe.value);
	}
}
