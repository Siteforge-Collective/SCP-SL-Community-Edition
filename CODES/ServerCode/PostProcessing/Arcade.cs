namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.ArcadeRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.AfterStack, "Custom Effects/Arcade", true)]
	public sealed class Arcade : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Fade = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			if (enabled.value)
			{
				return (float)Fade > 0f;
			}
			return false;
		}
	}
}
