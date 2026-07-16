namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.ArtefactRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.AfterStack, "Custom Effects/Artefact", true)]
	public sealed class Artefact : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Fade = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0.01f
		};

		[global::UnityEngine.Range(-10f, 10f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Colorization = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0.9f
		};

		[global::UnityEngine.Range(-10f, 10f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Parasite = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 5.3f
		};

		[global::UnityEngine.Range(-10f, 10f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Noise = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 10f
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
