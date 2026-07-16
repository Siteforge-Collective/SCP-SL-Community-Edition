namespace PostProcessing
{
	public sealed class GrayscaleRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.Grayscale>
	{
		private readonly int _graySCaleIntensityId = global::UnityEngine.Shader.PropertyToID("_GrayScaleIntensity");

		private global::UnityEngine.Shader shader;

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/Grayscale");
		}

		public override void Release()
		{
			base.Release();
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			base.settings.SetDistanceProperties(context, propertySheet);
			context.command.SetGlobalFloat(_graySCaleIntensityId, base.settings.grayScaleIntensity);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}

		public override global::UnityEngine.DepthTextureMode GetCameraFlags()
		{
			return global::UnityEngine.DepthTextureMode.Depth;
		}
	}
}
