namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.StaticRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.AfterStack, "Custom Effects/Static", true)]
	public sealed class Static : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter fade = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter fadeAdditive = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter fadeDistortion = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			if (enabled.value)
			{
				if ((float)fade == 0f && (float)fadeAdditive == 0f)
				{
					return (float)fadeDistortion != 0f;
				}
				return true;
			}
			return false;
		}
	}
}
