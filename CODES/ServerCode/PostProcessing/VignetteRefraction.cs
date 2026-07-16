namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.VignetteRefractionRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.AfterStack, "Custom Effects/VignetteRefraction", true)]
	public sealed class VignetteRefraction : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.ColorUsage(false, true)]
		public global::UnityEngine.Rendering.PostProcessing.ColorParameter ColorStart = new global::UnityEngine.Rendering.PostProcessing.ColorParameter
		{
			value = global::UnityEngine.Color.white
		};

		[global::UnityEngine.ColorUsage(false, true)]
		public global::UnityEngine.Rendering.PostProcessing.ColorParameter ColorEnd = new global::UnityEngine.Rendering.PostProcessing.ColorParameter
		{
			value = global::UnityEngine.Color.white
		};

		public global::UnityEngine.Rendering.PostProcessing.TextureParameter RefractionTexture = new global::UnityEngine.Rendering.PostProcessing.TextureParameter();

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter RefractionPower = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0.5f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Intensity = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Frosting = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Fade = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0.05f
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			if (RefractionTexture.value != null && (float)Intensity > 0f)
			{
				return enabled.value;
			}
			return false;
		}
	}
}
