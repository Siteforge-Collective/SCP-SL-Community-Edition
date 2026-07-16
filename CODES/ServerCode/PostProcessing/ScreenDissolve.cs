namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.ScreenDissolveRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.AfterStack, "Custom Effects/ScreenDissolve", true)]
	public sealed class ScreenDissolve : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter DissolveAmount = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		public global::UnityEngine.Rendering.PostProcessing.TextureParameter BlendTexture = new global::UnityEngine.Rendering.PostProcessing.TextureParameter();

		public global::UnityEngine.Rendering.PostProcessing.TextureParameter OverlayTexture = new global::UnityEngine.Rendering.PostProcessing.TextureParameter();

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			if (enabled.value && BlendTexture != null && OverlayTexture != null)
			{
				return (float)DissolveAmount != 0f;
			}
			return false;
		}
	}
}
