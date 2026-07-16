namespace PostProcessing
{
	public sealed class DarkenRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.Darken>
	{
		private global::UnityEngine.Shader shader;

		private readonly int intensityId = global::UnityEngine.Shader.PropertyToID("_DarknessIntensity");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/Darken");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(intensityId, base.settings.intensity);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
