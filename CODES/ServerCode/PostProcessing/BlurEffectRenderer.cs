namespace PostProcessing
{
	public sealed class BlurEffectRenderer : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<global::PostProcessing.BlurEffect>
	{
		private global::UnityEngine.Shader shader;

		private int screenCopyID;

		public override void Init()
		{
			shader = global::UnityEngine.Shader.Find("Hidden/Custom Effects/BlurEffect");
			screenCopyID = global::UnityEngine.Shader.PropertyToID("_ScreenCopyTexture");
		}

		public override void Release()
		{
			base.Release();
		}

		public override void Render(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			global::UnityEngine.Rendering.PostProcessing.PropertySheet propertySheet = context.propertySheets.Get(shader);
			global::UnityEngine.Rendering.CommandBuffer command = context.command;
			command.GetTemporaryRT(screenCopyID, context.width, context.height, 0, global::UnityEngine.FilterMode.Bilinear, context.sourceFormat);
			command.Blit(context.source, screenCopyID);
			int num = global::UnityEngine.Shader.PropertyToID("_Temp1");
			int num2 = global::UnityEngine.Shader.PropertyToID("_Temp2");
			command.GetTemporaryRT(num, context.screenWidth / (int)base.settings.downscaling, context.screenHeight / (int)base.settings.downscaling, 0, global::UnityEngine.FilterMode.Bilinear);
			command.GetTemporaryRT(num2, context.screenWidth / (int)base.settings.downscaling, context.screenHeight / (int)base.settings.downscaling, 0, global::UnityEngine.FilterMode.Bilinear);
			command.Blit(screenCopyID, num);
			int pass = 1;
			for (int i = 0; i < (int)base.settings.iterations; i++)
			{
				if ((int)base.settings.iterations > 12)
				{
					return;
				}
				command.SetGlobalVector("_BlurOffsets", new global::UnityEngine.Vector4((float)base.settings.amount / (float)context.screenWidth, 0f, 0f, 0f));
				global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, num, num2, propertySheet, pass);
				command.SetGlobalVector("_BlurOffsets", new global::UnityEngine.Vector4(0f, (float)base.settings.amount / (float)context.screenHeight, 0f, 0f));
				global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, num2, num, propertySheet, pass);
				if ((bool)base.settings.highQuality)
				{
					command.SetGlobalVector("_BlurOffsets", new global::UnityEngine.Vector4((float)base.settings.amount / (float)context.screenWidth, 0f, 0f, 0f));
					global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, num, num2, propertySheet, pass);
					command.SetGlobalVector("_BlurOffsets", new global::UnityEngine.Vector4(0f, (float)base.settings.amount / (float)context.screenHeight, 0f, 0f));
					global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(context.command, num2, num, propertySheet, pass);
				}
			}
			command.SetGlobalTexture("_BlurredTex", num);
			base.settings.SetDistanceProperties(context, propertySheet);
			global::UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(command, context.source, context.destination, propertySheet, 0);
			command.ReleaseTemporaryRT(screenCopyID);
			command.ReleaseTemporaryRT(num);
			command.ReleaseTemporaryRT(num2);
		}

		public override global::UnityEngine.DepthTextureMode GetCameraFlags()
		{
			return global::UnityEngine.DepthTextureMode.Depth;
		}
	}
}
