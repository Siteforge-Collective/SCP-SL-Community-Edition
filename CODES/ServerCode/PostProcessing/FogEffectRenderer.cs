namespace PostProcessing
{
	public sealed class FogEffectRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.FogEffect>
	{
		private global::UnityEngine.Shader shader;

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/FogEffect");
		}

		public override void Release()
		{
			base.Release();
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			global::UnityEngine.Rendering.CommandBuffer command = context.command;
			_ = context.camera;
			base.settings.SetDistanceProperties(context, propertySheet);
			command.SetGlobalFloat("_FogBrightness", base.settings.fogBrightness);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0);
		}

		public override global::UnityEngine.DepthTextureMode GetCameraFlags()
		{
			return global::UnityEngine.DepthTextureMode.Depth;
		}
	}
}
