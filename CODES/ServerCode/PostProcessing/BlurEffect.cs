namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.BlurEffectRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.BeforeStack, "Custom Effects/Blur Effect", true)]
	public sealed class BlurEffect : global::PostProcessing.DistanceEffect
	{
		[global::UnityEngine.Space]
		[global::UnityEngine.Tooltip("When enabled, the amount of blur passes is doubled")]
		public global::UnityEngine.Rendering.PostProcessing.BoolParameter highQuality = new global::UnityEngine.Rendering.PostProcessing.BoolParameter
		{
			value = false
		};

		[global::UnityEngine.Range(0f, 5f)]
		[global::UnityEngine.Tooltip("The amount of blurring that must be performed")]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter amount = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(1f, 12f)]
		[global::UnityEngine.Tooltip("The number of times the effect is blurred. More iterations provide a smoother effect but induce more drawcalls.")]
		public global::UnityEngine.Rendering.PostProcessing.IntParameter iterations = new global::UnityEngine.Rendering.PostProcessing.IntParameter
		{
			value = 6
		};

		[global::UnityEngine.Range(1f, 4f)]
		[global::UnityEngine.Tooltip("Every step halfs the resolution of the blur effect. Lower resolution provides a smoother blur but may induce flickering.")]
		public global::UnityEngine.Rendering.PostProcessing.IntParameter downscaling = new global::UnityEngine.Rendering.PostProcessing.IntParameter
		{
			value = 2
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			return enabled.value;
		}
	}
}
