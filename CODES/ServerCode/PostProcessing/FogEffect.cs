namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.FogEffectRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.BeforeStack, "Custom Effects/Fog Effect", true)]
	public sealed class FogEffect : global::PostProcessing.DistanceEffect
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter fogBrightness = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			return enabled.value;
		}
	}
}
