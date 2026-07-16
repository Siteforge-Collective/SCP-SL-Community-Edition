namespace PostProcessing
{
	public sealed class ScreenDissolveRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.ScreenDissolve>
	{
		private global::UnityEngine.Shader shader;

		private readonly int _dissolveAmount = global::UnityEngine.Shader.PropertyToID("_DissolveAmount");

		private readonly int _blendTex2Id = global::UnityEngine.Shader.PropertyToID("_BlendTex");

		private readonly int _overlayTex2Id = global::UnityEngine.Shader.PropertyToID("_OverlayTex");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/ScreenDissolve");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(_dissolveAmount, base.settings.DissolveAmount);
			propertySheet.properties.SetTexture(_blendTex2Id, base.settings.BlendTexture);
			propertySheet.properties.SetTexture(_overlayTex2Id, base.settings.OverlayTexture);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
