namespace PostProcessing
{
	public sealed class StaticRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.Static>
	{
		private global::UnityEngine.Shader shader;

		private global::UnityEngine.Texture2D staticTexture;

		private readonly int fadeId = global::UnityEngine.Shader.PropertyToID("_Fade");

		private readonly int fadeAdditiveId = global::UnityEngine.Shader.PropertyToID("_FadeAdditive");

		private readonly int fadeDistortionId = global::UnityEngine.Shader.PropertyToID("_FadeDistortion");

		private readonly int staticTexId = global::UnityEngine.Shader.PropertyToID("_StaticTex");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/Static");
			staticTexture = global::UnityEngine.Resources.Load("StaticTexture") as global::UnityEngine.Texture2D;
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(fadeId, base.settings.fade);
			propertySheet.properties.SetFloat(fadeAdditiveId, base.settings.fadeAdditive);
			propertySheet.properties.SetFloat(fadeDistortionId, base.settings.fadeDistortion);
			propertySheet.properties.SetTexture(staticTexId, staticTexture);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
