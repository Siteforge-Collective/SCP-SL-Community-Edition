namespace PostProcessing
{
	public sealed class GlitchRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.Glitch>
	{
		private global::UnityEngine.Shader shader;

		private readonly int glitchId = global::UnityEngine.Shader.PropertyToID("_Glitch");

		private readonly int noiseId = global::UnityEngine.Shader.PropertyToID("_Noise");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/Glitch");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(glitchId, base.settings.glitch);
			propertySheet.properties.SetFloat(noiseId, base.settings.noise);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
