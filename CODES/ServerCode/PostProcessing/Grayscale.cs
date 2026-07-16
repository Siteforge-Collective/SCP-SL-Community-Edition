namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.GrayscaleRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.BeforeStack, "Custom Effects/Grayscale", true)]
	public sealed class Grayscale : global::PostProcessing.DistanceEffect
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter grayScaleIntensity = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			return enabled.value;
		}
	}
}
