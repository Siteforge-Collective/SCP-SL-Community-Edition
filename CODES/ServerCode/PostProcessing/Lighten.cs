namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.LightenRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.AfterStack, "Custom Effects/Lighten", true)]
	public sealed class Lighten : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter brightness = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 1f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter opacity = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			if (enabled.value)
			{
				return (float)opacity != 0f;
			}
			return false;
		}
	}
}
