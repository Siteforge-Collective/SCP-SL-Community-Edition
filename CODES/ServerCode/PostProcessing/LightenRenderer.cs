namespace PostProcessing
{
	public sealed class LightenRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.Lighten>
	{
		private global::UnityEngine.Shader shader;

		private readonly int opacityId = global::UnityEngine.Shader.PropertyToID("_Opacity");

		private readonly int brightnessId = global::UnityEngine.Shader.PropertyToID("_Brightness");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/Lighten");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(opacityId, base.settings.opacity);
			propertySheet.properties.SetFloat(brightnessId, base.settings.brightness);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
