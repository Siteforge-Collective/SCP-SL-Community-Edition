namespace PostProcessing
{
	[global::System.Serializable]
	[global::UnityEngine.Rendering.PostProcessing.PostProcess(typeof(global::PostProcessing.BloodHitRenderer), global::UnityEngine.Rendering.PostProcessing.PostProcessEvent.AfterStack, "Custom Effects/BloodHit", true)]
	public sealed class BloodHit : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Hit_Left = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 1f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Hit_Up = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Hit_Right = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Hit_Down = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Blood_Hit_Left = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Blood_Hit_Up = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Blood_Hit_Right = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Blood_Hit_Down = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Hit_Full = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Blood_Hit_Full_1 = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Blood_Hit_Full_2 = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter Blood_Hit_Full_3 = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter LightReflect = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0.5f
		};

		public override bool IsEnabledAndSupported(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context)
		{
			return enabled.value;
		}
	}
}
