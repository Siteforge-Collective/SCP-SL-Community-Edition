namespace PostProcessing
{
	public sealed class ArcadeRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.Arcade>
	{
		private global::UnityEngine.Shader shader;

		private readonly int _fadeId = global::UnityEngine.Shader.PropertyToID("_Fade");

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/Arcade");
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			propertySheet.properties.SetFloat(_fadeId, base.settings.Fade);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}
	}
}
