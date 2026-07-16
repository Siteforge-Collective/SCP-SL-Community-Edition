namespace PostProcessing
{
	[global::System.Serializable]
	public abstract class DistanceEffect : global::UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
	{
		[global::UnityEngine.Space]
		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter maxIntensity = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 1f
		};

		public global::UnityEngine.Rendering.PostProcessing.FloatParameter fogStartDistance = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 1.6f
		};

		public global::UnityEngine.Rendering.PostProcessing.FloatParameter fogFadeDistance = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0.5f
		};

		public global::UnityEngine.Rendering.PostProcessing.FloatParameter fogDistanceOffset = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 0f
		};

		[global::UnityEngine.Range(0f, 1f)]
		public global::UnityEngine.Rendering.PostProcessing.FloatParameter skyboxInfluence = new global::UnityEngine.Rendering.PostProcessing.FloatParameter
		{
			value = 1f
		};

		public void SetDistanceProperties(global::UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context, global::UnityEngine.Rendering.PostProcessing.PropertySheet sheet)
		{
			global::UnityEngine.Rendering.CommandBuffer command = context.command;
			global::UnityEngine.Camera camera = context.camera;
			global::UnityEngine.Matrix4x4 gPUProjectionMatrix = global::UnityEngine.GL.GetGPUProjectionMatrix(camera.projectionMatrix, renderIntoTexture: false);
			float value = (gPUProjectionMatrix[3, 2] = 0f);
			gPUProjectionMatrix[2, 3] = value;
			gPUProjectionMatrix[3, 3] = 1f;
			global::UnityEngine.Matrix4x4 value2 = global::UnityEngine.Matrix4x4.Inverse(gPUProjectionMatrix * camera.worldToCameraMatrix) * global::UnityEngine.Matrix4x4.TRS(new global::UnityEngine.Vector3(0f, 0f, 0f - gPUProjectionMatrix[2, 2]), global::UnityEngine.Quaternion.identity, global::UnityEngine.Vector3.one);
			sheet.properties.SetMatrix("clipToWorld", value2);
			float num2 = (float)fogStartDistance + (float)fogDistanceOffset;
			float num3 = num2 + (float)fogFadeDistance + (float)fogFadeDistance;
			num2 -= (float)fogFadeDistance;
			float num4 = num3 - num2;
			float num5 = ((global::UnityEngine.Mathf.Abs(num4) > 0.0001f) ? (1f / num4) : 0f);
			command.SetGlobalVector("_SceneFogParams", new global::UnityEngine.Vector4(0f - (float)fogStartDistance, maxIntensity, 0f - num5, num3 * num5));
			command.SetGlobalFloat("_SkyboxInfluence", skyboxInfluence);
		}
	}
}
