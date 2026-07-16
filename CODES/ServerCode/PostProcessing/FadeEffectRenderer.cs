namespace PostProcessing
{
	public sealed class FadeEffectRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.FadeEffect>
	{
		private global::UnityEngine.Shader shader;

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/FadeEffect");
		}

		public override void Release()
		{
			base.Release();
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			_ = context.command;
			_ = context.camera;
			base.settings.SetDistanceProperties(context, propertySheet);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}

		public override global::UnityEngine.DepthTextureMode GetCameraFlags()
		{
			return global::UnityEngine.DepthTextureMode.Depth;
		}
	}
}
