namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.FadeEffectRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.BeforeStack, "Custom Effects/Fade Effect", true)]
	public sealed class FadeEffect : global::PostProcessing.DistanceEffect
	{
		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			return enabled.value;
		}
	}
}
