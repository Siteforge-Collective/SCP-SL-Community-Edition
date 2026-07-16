namespace PostProcessing
{
	public sealed class VignetteRefractionRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.VignetteRefraction>
	{
		private global::UnityEngine.Shader _shader;

		private readonly int _refractionTexId = global::UnityEngine.Shader.PropertyToID("_RefractionTex");

		private readonly int _valueId = global::UnityEngine.Shader.PropertyToID("_Values");

		private readonly int _color1Id = global::UnityEngine.Shader.PropertyToID("_Color1");

		private readonly int _color2Id = global::UnityEngine.Shader.PropertyToID("_Color2");

		public override void Init()
		{
			_shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/VignetteRefraction");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(_shader);
			propertySheet.properties.SetVector(_valueId, new global::UnityEngine.Vector4(base.settings.Intensity, base.settings.RefractionPower, base.settings.Frosting, base.settings.Fade));
			propertySheet.properties.SetTexture(_refractionTexId, base.settings.RefractionTexture);
			propertySheet.properties.SetColor(_color1Id, base.settings.ColorStart);
			propertySheet.properties.SetColor(_color2Id, base.settings.ColorEnd);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
